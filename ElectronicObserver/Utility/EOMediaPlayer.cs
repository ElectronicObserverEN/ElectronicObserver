using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;


namespace ElectronicObserver.Utility;

/// <summary>
/// Windows Media Player コントロールを利用して、音楽を再生するためのクラスです。(outdated)
/// </summary>
public class EOMediaPlayer
{
	private MediaPlayer WMP { get; }

	public event Action MediaEnded = delegate { };

	private List<string> _playlist;
	private List<string> _realPlaylist;

	private Random Rand { get; }


	/// <summary>
	/// 対応している拡張子リスト
	/// </summary>
	public static readonly ReadOnlyCollection<string> SupportedExtensions =
		new ReadOnlyCollection<string>(new List<string>() {
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

	private static readonly Regex SupportedFileName = new Regex(".*\\.(" + string.Join("|", SupportedExtensions) + ")", RegexOptions.Compiled);


	public EOMediaPlayer()
	{
		try
		{
			WMP = new MediaPlayer
			{
				
			};
			//WMP.settings.autoStart = false;
			WMP.MediaOpened += WMP_MediaOpened;
			WMP.MediaEnded += WMP_MediaEnded;

		}
		catch (Exception e)
		{
			WMP = null;
		}

		IsLoop = false;
		_isShuffle = false;
		IsMute = false;
		LoopHeadPosition = TimeSpan.Zero;
		AutoPlay = false;
		_playlist = new List<string>();
		_realPlaylist = new List<string>();
		Rand = new Random();

		MediaEnded += MediaPlayer_MediaEnded;
	}


	/// <summary>
	/// 利用可能かどうか
	/// false の場合全機能が使用不可能
	/// </summary>
	public bool IsAvailable => WMP != null;

	/// <summary>
	/// メディアファイルのパス。
	/// 再生中に変更された場合停止します。
	/// </summary>
	public string SourcePath
	{
		get { return !IsAvailable ? string.Empty : WMP.Source?.ToString() ?? string.Empty; }
		set
		{
			if (IsAvailable && WMP.Source?.ToString() != value && !string.IsNullOrEmpty(value))
				WMP.Open(new(value));
		}
	}


	/// <summary>
	/// 音量
	/// 0-100
	/// 注: システムの音量設定と連動しているようなので注意が必要
	/// </summary>
	public int Volume
	{
		get { return !IsAvailable ? 0 : (int)(WMP.Volume * 100); }
		set { if (IsAvailable) WMP.Volume = (double)value / 100; }
	}

	/// <summary>
	/// ミュート
	/// </summary>
	public bool IsMute
	{
		get { return IsAvailable && WMP.IsMuted; }
		set { if (IsAvailable) WMP.IsMuted = value; }
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
		get { return !IsAvailable ? TimeSpan.Zero : WMP.Position; }
		set { if (IsAvailable) WMP.Position = value; }
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
		return new List<string>(_playlist);
	}

	/// <summary>
	/// プレイリストを設定します。
	/// </summary>
	/// <param name="list"></param>
	public void SetPlaylist(IEnumerable<string> list)
	{
		if (list == null)
			_playlist = new List<string>();
		else
			_playlist = list.Distinct().ToList();

		UpdateRealPlaylist();
	}


	public IEnumerable<string> SearchSupportedFiles(string path, System.IO.SearchOption option = System.IO.SearchOption.TopDirectoryOnly)
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
		get { return _playingIndex; }
		set
		{
			if (_playingIndex != value)
			{

				if (value < 0 || _realPlaylist.Count <= value)
					return;

				_playingIndex = value;
				SourcePath = _realPlaylist[_playingIndex];
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
		get { return _isShuffle; }
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
		if (!IsAvailable) return;

		if (_realPlaylist.Count > 0 && SourcePath != _realPlaylist[_playingIndex])
			SourcePath = _realPlaylist[_playingIndex];

		WMP.Play();
	}

	/// <summary>
	/// ポーズ
	/// </summary>
	public void Pause()
	{
		if (!IsAvailable) return;

		WMP.Pause();
	}

	/// <summary>
	/// 停止
	/// </summary>
	public void Stop()
	{
		if (!IsAvailable) return;

		WMP.Stop();
	}

	/// <summary>
	/// ファイルを閉じる
	/// </summary>
	public void Close()
	{
		if (!IsAvailable) return;

		WMP.Close();
	}


	/// <summary>
	/// 次の曲へ
	/// </summary>
	public void Next()
	{
		if (!IsAvailable) return;

		if (PlayingIndex >= _realPlaylist.Count - 1)
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
		if (!IsAvailable) return;

		if (!IsShuffle)
		{
			_realPlaylist = new List<string>(_playlist);

		}
		else
		{
			// shuffle
			_realPlaylist = _playlist.OrderBy(s => Guid.NewGuid()).ToList();

			// 同じ曲が連続で流れるのを防ぐ
			if (_realPlaylist.Count > 1 && SourcePath == _realPlaylist[0])
			{
				_realPlaylist = _realPlaylist.Skip(1).ToList();
				_realPlaylist.Insert(Rand.Next(1, _realPlaylist.Count + 1), SourcePath);
			}
		}

		int index = _realPlaylist.IndexOf(SourcePath);
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
			WMP.Position = CurrentPosition;
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
			WMP.Play();
		}
	}


	// 即時変化させるとイベント終了直後に書き換えられて next が無視されるので苦肉の策
	private async void OnMediaEnded()
	{
		await Task.Run(() => Task.WaitAll(Task.Delay(10)));
		MediaEnded();
	}
}

public enum PlayState
{
	None,
	Playing
}
