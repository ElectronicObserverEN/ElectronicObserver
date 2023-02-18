using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;


namespace ElectronicObserver.Utility;

public class EOMediaPlayer
{
	private MediaPlayer MediaPlayer { get; }

	public event Action MediaEnded = delegate { };

	private List<string> Playlist { get; set; }
	private List<string> RealPlayList { get; set; }

	private Random Rand { get; }


	/// <summary>
	/// 対応している拡張子リスト
	/// </summary>
	public static readonly ReadOnlyCollection<string> SupportedExtensions =
		new(new List<string>() {
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
		});

	private static readonly Regex SupportedFileName = new(".*\\.(" + string.Join("|", SupportedExtensions) + ")", RegexOptions.Compiled, TimeSpan.FromSeconds(30));


	public EOMediaPlayer()
	{
		MediaPlayer = new MediaPlayer();

		MediaPlayer.MediaOpened += WMP_MediaOpened;
		MediaPlayer.MediaEnded += WMP_MediaEnded;

		IsLoop = false;
		_isShuffle = false;
		IsMute = false;
		LoopHeadPosition = TimeSpan.Zero;
		AutoPlay = false;
		Playlist = new List<string>();
		RealPlayList = new List<string>();
		Rand = new Random();

		MediaEnded += MediaPlayer_MediaEnded;
	}

	/// <summary>
	/// メディアファイルのパス。
	/// 再生中に変更された場合停止します。
	/// </summary>
	public string SourcePath
	{
		get => MediaPlayer.Source?.ToString() ?? string.Empty;
		set
		{
			if (MediaPlayer.Source?.ToString() != value && !string.IsNullOrEmpty(value))
			{
				MediaPlayer.Open(new(value, UriKind.RelativeOrAbsolute));
			}
		}
	}


	/// <summary>
	/// 音量
	/// 0-100
	/// 注: システムの音量設定と連動しているようなので注意が必要
	/// </summary>
	public int Volume
	{
		get => (int)(MediaPlayer.Volume * 100);
		set => MediaPlayer.Volume = (double)value / 100;
	}

	/// <summary>
	/// ミュート
	/// </summary>
	public bool IsMute
	{
		get => MediaPlayer.IsMuted;
		set => MediaPlayer.IsMuted = value;
	}


	/// <summary>
	/// ループするか
	/// </summary>
	public bool IsLoop { get; set; }

	/// <summary>
	/// ループ時の先頭 (秒単位)
	/// </summary>
	public TimeSpan LoopHeadPosition { get; set; }


	/// <summary>
	/// 現在の再生地点 (秒単位)
	/// </summary>
	public TimeSpan CurrentPosition
	{
		get => MediaPlayer.Position;
		set => MediaPlayer.Position = value;
	}

	/// <summary>
	/// 再生状態
	/// </summary>
	public PlayState PlayState { get; set; }

	/// <summary>
	/// プレイリストのコピーを取得します。
	/// </summary>
	/// <returns></returns>
	public List<string> GetPlaylist()
	{
		return new List<string>(Playlist);
	}

	/// <summary>
	/// プレイリストを設定します。
	/// </summary>
	/// <param name="list"></param>
	public void SetPlaylist(IEnumerable<string>? list)
	{
		if (list is null)
			Playlist = new List<string>();
		else
			Playlist = list.Distinct().ToList();

		UpdateRealPlaylist();
	}


	public static IEnumerable<string> SearchSupportedFiles(string path, System.IO.SearchOption option = System.IO.SearchOption.TopDirectoryOnly)
	{
		return System.IO.Directory.EnumerateFiles(path, "*", option).Where(s => SupportedFileName.IsMatch(s));
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



	private int _playingIndex;
	/// <summary>
	/// 現在再生中の曲のプレイリスト中インデックス
	/// </summary>
	private int PlayingIndex
	{
		get => _playingIndex;
		set
		{
			if (_playingIndex != value)
			{

				if (value < 0 || RealPlayList.Count <= value)
					return;

				_playingIndex = value;
				SourcePath = RealPlayList[_playingIndex];
				if (AutoPlay)
					Play();
			}
		}
	}

	private bool _isShuffle;
	/// <summary>
	/// シャッフル再生するか
	/// </summary>
	public bool IsShuffle
	{
		get => _isShuffle; 
		set
		{
			bool changed = _isShuffle != value;

			_isShuffle = value;

			if (changed)
			{
				UpdateRealPlaylist();
			}
		}
	}

	/// <summary>
	/// 曲が終了したとき自動で次の曲を再生するか
	/// </summary>
	public bool AutoPlay { get; set; }





	/// <summary>
	/// 再生
	/// </summary>
	public void Play()
	{
		if (RealPlayList.Count > 0 && SourcePath != RealPlayList[_playingIndex])
			SourcePath = RealPlayList[_playingIndex];

		MediaPlayer.Position = LoopHeadPosition;
		MediaPlayer.Play();
	}

	/// <summary>
	/// ポーズ
	/// </summary>
	public void Pause()
	{
		MediaPlayer.Pause();
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
		MediaPlayer.Close();
	}


	/// <summary>
	/// 次の曲へ
	/// </summary>
	public void Next()
	{
		if (PlayingIndex >= RealPlayList.Count - 1)
		{
			if (IsShuffle)
				UpdateRealPlaylist();
			PlayingIndex = 0;
		}
		else
		{
			PlayingIndex++;
		}

		if (AutoPlay)     // Playing
			Play();
	}


	private void UpdateRealPlaylist()
	{
		if (!IsShuffle)
		{
			RealPlayList = new List<string>(Playlist);

		}
		else
		{
			// shuffle
			RealPlayList = Playlist.OrderBy(s => Guid.NewGuid()).ToList();

			// 同じ曲が連続で流れるのを防ぐ
			if (RealPlayList.Count > 1 && SourcePath == RealPlayList[0])
			{
				RealPlayList = RealPlayList.Skip(1).ToList();
				RealPlayList.Insert(Rand.Next(1, RealPlayList.Count + 1), SourcePath);
			}
		}

		int index = RealPlayList.IndexOf(SourcePath);
		PlayingIndex = index != -1 ? index : 0;
	}

	private void WMP_MediaOpened(object? sender, EventArgs e)
	{
		PlayState = PlayState.Playing;
	}

	private void WMP_MediaEnded(object? sender, EventArgs e)
	{
		if (IsLoop)
		{
			MediaPlayer.Position = CurrentPosition;
			CurrentPosition = LoopHeadPosition;
		}

		PlayState = PlayState.None;

		OnMediaEnded();
	}


	void MediaPlayer_MediaEnded()
	{
		// プレイリストの処理
		if (!IsLoop && AutoPlay)
			Next();

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
