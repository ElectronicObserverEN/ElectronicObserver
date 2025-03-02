using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NAudio.Wave;


namespace ElectronicObserver.Utility;

public class EOMediaPlayer
{
	private WaveOutEvent MediaPlayer { get; } = new();
	private AudioFileReader? AudioFile { get; set; }

	public event Action MediaEnded = delegate { };

	public List<string> Playlist { get; private set; } = [];
	private List<string> RealPlayList { get; set; } = [];


	/// <summary>
	/// 対応している拡張子リスト
	/// </summary>
	public static readonly ReadOnlyCollection<string> SupportedExtensions =
		new(
		[
			"asf",
			"wma",
			"mp2",
			"mp3",
			"mid",
			"midi",
			"rmi",
			"aif",
			"aifc",
			"aiff",
			"au",
			"snd",
			"wav",
			"m4a",
			"aac",
			"flac",
			"mka",
		]);

	private static readonly Regex SupportedFileName = new(".*\\.(" + string.Join("|", SupportedExtensions) + ")", RegexOptions.Compiled, TimeSpan.FromSeconds(30));


	public EOMediaPlayer()
	{
		MediaPlayer.PlaybackStopped += WMP_MediaEnded;

		MediaEnded += MediaPlayer_MediaEnded;
	}

	/// <summary>
	/// メディアファイルのパス。
	/// 再生中に変更された場合停止します。
	/// </summary>
	public string SourcePath
	{
		get => AudioFile?.FileName ?? string.Empty;
		set
		{
			if (AudioFile?.FileName != value && !string.IsNullOrEmpty(value))
			{
				AudioFile = new(value);
				AudioFile.Volume = NormalizedInternalVolume;
				MediaPlayer.Init(AudioFile);
			}
		}
	}

	/// <summary>
	/// Mute sets volume to 0, this property tracks the actual volume.
	/// </summary>
	private int InternalVolume { get; set; }

	/// <summary>
	/// For setting the AudioFile volume value.
	/// </summary>
	private float NormalizedInternalVolume => IsMute switch
	{
		true => 0,
		_ => (float)InternalVolume / 100,
	};

	/// <summary>
	/// 音量
	/// 0-100
	/// 注: システムの音量設定と連動しているようなので注意が必要
	/// </summary>
	public int Volume
	{
		get => InternalVolume;
		set
		{
			InternalVolume = value;

			if (AudioFile is not null)
			{
				AudioFile.Volume = NormalizedInternalVolume;
			}
		}
	}

	/// <summary>
	/// ミュート
	/// </summary>
	public bool IsMute
	{
		get;
		set
		{
			field = value;

			if (AudioFile is not null)
			{
				AudioFile.Volume = NormalizedInternalVolume;
			}
		}
	}


	/// <summary>
	/// ループするか
	/// </summary>
	public bool IsLoop { get; set; }

	/// <summary>
	/// ループ時の先頭 (秒単位)
	/// </summary>
	public TimeSpan LoopHeadPosition { get; set; } = TimeSpan.Zero;


	/// <summary>
	/// 現在の再生地点 (秒単位)
	/// </summary>
	private TimeSpan CurrentPosition
	{
		get => AudioFile?.CurrentTime ?? TimeSpan.Zero;
		set
		{
			if (AudioFile is null) return;

			AudioFile.CurrentTime = value;
		}
	}

	/// <summary>
	/// 再生状態
	/// </summary>
	public PlayState PlayState { get; private set; }

	/// <summary>
	/// プレイリストを設定します。
	/// </summary>
	/// <param name="list"></param>
	public void SetPlaylist(IEnumerable<string>? list)
	{
		Playlist = list switch
		{
			null => [],
			_ => list.Distinct().ToList()
		};

		UpdateRealPlaylist();
	}


	private static IEnumerable<string> SearchSupportedFiles(string path, System.IO.SearchOption option = System.IO.SearchOption.TopDirectoryOnly)
	{
		return System.IO.Directory
			.EnumerateFiles(path, "*", option)
			.Where(s => SupportedFileName.IsMatch(s));
	}

	/// <summary>
	/// フォルダを検索し、音楽ファイルをプレイリストに設定します。
	/// </summary>
	/// <param name="path">フォルダへのパス。</param>
	/// <param name="option">検索オプション。既定ではサブディレクトリは検索されません。</param>
	public void SetPlaylistFromDirectory(string path, System.IO.SearchOption option = System.IO.SearchOption.TopDirectoryOnly)
	{
		SetPlaylist(SearchSupportedFiles(path, option));
	}



	/// <summary>
	/// 現在再生中の曲のプレイリスト中インデックス
	/// </summary>
	private int PlayingIndex
	{
		get;
		set
		{
			if (field != value)
			{

				if (value < 0 || RealPlayList.Count <= value) return;

				field = value;
				SourcePath = RealPlayList[field];
				if (AutoPlay)
				{
					Play();
				}
			}
		}
	}

	/// <summary>
	/// シャッフル再生するか
	/// </summary>
	public bool IsShuffle
	{
		get;
		init
		{
			bool changed = field != value;

			field = value;

			if (changed)
			{
				UpdateRealPlaylist();
			}
		}
	}

	/// <summary>
	/// 曲が終了したとき自動で次の曲を再生するか
	/// </summary>
	public bool AutoPlay { get; init; }





	/// <summary>
	/// 再生
	/// </summary>
	public void Play()
	{
		if (RealPlayList.Count > 0 && SourcePath != RealPlayList[PlayingIndex])
		{
			SourcePath = RealPlayList[PlayingIndex];
		}

		if (AudioFile is null) return;

		AudioFile.CurrentTime = LoopHeadPosition;
		MediaPlayer.Play();

		PlayState = PlayState.Playing;
	}

	/// <summary>
	/// 停止
	/// </summary>
	public void Stop()
	{
		MediaPlayer.Stop();
	}

	/// <summary>
	/// ファイルを閉じる
	/// </summary>
	public void Close()
	{
		MediaPlayer.Stop();
		AudioFile?.Dispose();
		AudioFile = null;
	}


	/// <summary>
	/// 次の曲へ
	/// </summary>
	public void Next()
	{
		if (PlayingIndex >= RealPlayList.Count - 1)
		{
			if (IsShuffle)
			{
				UpdateRealPlaylist();
			}

			PlayingIndex = 0;
		}
		else
		{
			PlayingIndex++;
		}

		if (AutoPlay)     // Playing
		{
			Play();
		}
	}


	private void UpdateRealPlaylist()
	{
		if (!IsShuffle)
		{
			RealPlayList = [.. Playlist];
		}
		else
		{
			// shuffle
			RealPlayList = Playlist.OrderBy(s => Guid.NewGuid()).ToList();

			// 同じ曲が連続で流れるのを防ぐ
			if (RealPlayList.Count > 1 && SourcePath == RealPlayList[0])
			{
				RealPlayList = RealPlayList.Skip(1).ToList();
				RealPlayList.Insert(Random.Shared.Next(1, RealPlayList.Count + 1), SourcePath);
			}
		}

		PlayingIndex = RealPlayList.IndexOf(SourcePath) switch
		{
			-1 => 0,
			int index => index,
		};
	}

	private void WMP_MediaEnded(object? sender, EventArgs e)
	{
		if (IsLoop && AudioFile is not null)
		{
			AudioFile.CurrentTime = CurrentPosition;
			CurrentPosition = LoopHeadPosition;
		}

		PlayState = PlayState.None;

		OnMediaEnded();
	}


	void MediaPlayer_MediaEnded()
	{
		// プレイリストの処理
		if (!IsLoop && AutoPlay)
		{
			Next();
		}

		if (IsLoop)
		{
			MediaPlayer.Play();
		}
	}


	// 即時変化させるとイベント終了直後に書き換えられて next が無視されるので苦肉の策
	private async void OnMediaEnded()
	{
		await Task.Delay(10);
		MediaEnded();
	}
}
