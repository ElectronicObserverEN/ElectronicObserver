using System;
using System.IO;
using System.Linq;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Dialog;

namespace ElectronicObserver.Notifier;

/// <summary>
/// 通知を扱います。
/// </summary>
public abstract class NotifierBase
{
	private Configuration.ConfigurationData.ConfigNotifierBase Config { get; }

	/// <summary>
	/// 通知ダイアログに渡す設定データ
	/// </summary>
	public NotifierDialogData DialogData { get; }

	/// <summary>
	/// 有効かどうか
	/// </summary>
	public bool IsEnabled { get; set; }

	/// <summary>
	/// ミュート状態かどうか
	/// </summary>
	public bool IsSilenced { get; set; }


	/// <summary>
	/// 通知音
	/// </summary>
	public EOMediaPlayer Sound { get; }

	/// <summary>
	/// 通知音のパス
	/// </summary>
	public string SoundPath => Config.SoundPath;

	/// <summary>
	/// 通知音を再生するか
	/// </summary>
	public bool PlaysSound { get; set; }


	private bool _loopsSound;
	/// <summary>
	/// 通知音をループさせるか
	/// </summary>
	public bool LoopsSound
	{
		get => _loopsSound;
		set
		{
			_loopsSound = value;
			SetIsLoop();
		}
	}

	private int _soundVolume;
	/// <summary>
	/// 通知音の音量 (0-100)
	/// </summary>
	public int SoundVolume
	{
		get => _soundVolume;
		set
		{
			_soundVolume = value;
			if (!Utility.Configuration.Config.Control.UseSystemVolume)
				Sound.Volume = _soundVolume;
		}
	}

	private bool _showsDialog;
	/// <summary>
	/// 通知ダイアログを表示するか
	/// </summary>
	public bool ShowsDialog
	{
		get { return _showsDialog; }
		set
		{
			_showsDialog = value;
			SetIsLoop();
		}
	}

	private void SetIsLoop()
	{
		Sound.IsLoop = LoopsSound && ShowsDialog;
	}


	/// <summary>
	/// 通知を早める時間(ミリ秒)
	/// </summary>
	public int AccelInterval { get; set; }


	protected NotifierBase(Configuration.ConfigurationData.ConfigNotifierBase config)
	{
		Config = config;

		SystemEvents.UpdateTimerTick += UpdateTimerTick;

		Sound = new EOMediaPlayer
		{
			IsShuffle = true,
		};
		Sound.MediaEnded += Sound_MediaEnded;

		DialogData = new NotifierDialogData(config);

		if (config.PlaysSound && !string.IsNullOrEmpty(config.SoundPath))
		{
			LoadSound(config.SoundPath);
		}

		IsEnabled = config.IsEnabled;
		IsSilenced = config.IsSilenced;
		PlaysSound = config.PlaysSound;
		SoundVolume = config.SoundVolume;
		LoopsSound = config.LoopsSound;
		ShowsDialog = config.ShowsDialog;
		AccelInterval = config.AccelInterval;
	}


	protected virtual void UpdateTimerTick() { }


	#region 通知音

	/// <summary>
	/// 通知音を読み込みます。
	/// </summary>
	/// <param name="path">音声ファイルへのパス。</param>
	/// <returns>成功すれば true 、失敗すれば false を返します。</returns>
	public bool LoadSound(string path)
	{
		try
		{
			DisposeSound();

			if (File.Exists(path))
			{
				Sound.SetPlaylist(null);
				Sound.SourcePath = path;

			}
			else if (Directory.Exists(path))
			{
				Sound.SetPlaylistFromDirectory(path);

			}
			else
			{
				throw new FileNotFoundException("指定されたファイルまたはディレクトリが見つかりませんでした。");
			}

			Config.SoundPath = path;

			return true;

		}
		catch (Exception ex)
		{

			Utility.ErrorReporter.SendErrorReport(ex, string.Format(NotifierRes.FailedToLoadSound, path));
			DisposeSound();

		}

		return false;
	}

	/// <summary>
	/// 通知音を再生します。
	/// </summary>
	public void PlaySound()
	{
		try
		{

			if (Sound != null && PlaysSound)
			{
				if (Sound.PlayState == PlayState.Playing)
				{       //playing
					if (Sound.Playlist.Any())
					{
						Sound.Next();
					}

					Sound.Stop();
				}

				//音量の再設定(システム側の音量変更によって設定が変わることがあるので)
				SoundVolume = _soundVolume;
				Sound.Play();
			}

		}
		catch (Exception ex)
		{

			Utility.Logger.Add(3, NotifierRes.FailedPlaySound + ex.Message);
		}
	}

	/// <summary>
	/// 通知音を破棄します。
	/// </summary>
	private void DisposeSound()
	{
		Sound.Close();
	}


	void Sound_MediaEnded()
	{
		if (Sound.Playlist.Count > 0 && !LoopsSound)
		{
			Sound.Next();
		}
	}


	#endregion



	/// <summary>
	/// 通知ダイアログを表示します。
	/// </summary>
	private void ShowDialog(System.Windows.Forms.FormClosingEventHandler? customClosingHandler = null)
	{

		if (ShowsDialog)
		{
			var dialog = new DialogNotifier(DialogData);
			dialog.FormClosing += dialog_FormClosing;
			if (customClosingHandler != null)
			{
				dialog.FormClosing += customClosingHandler;
			}
			NotifierManager.Instance.ShowNotifier(dialog);
		}
	}

	void dialog_FormClosing(object? sender, System.Windows.Forms.FormClosingEventArgs e)
	{
		Sound.Stop();
	}

	/// <summary>
	/// 通知を行います。
	/// </summary>
	public virtual void Notify()
	{
		Notify(null);
	}

	/// <summary>
	/// 終了時のイベントハンドラを指定して通知を行います。
	/// </summary>
	public virtual void Notify(System.Windows.Forms.FormClosingEventHandler? customClosingHandler)
	{

		if (!IsEnabled || IsSilenced) return;

		ShowDialog(customClosingHandler);
		PlaySound();

	}


	public virtual void ApplyToConfiguration(Utility.Configuration.ConfigurationData.ConfigNotifierBase config)
	{

		DialogData.ApplyToConfiguration(config);
		config.PlaysSound = PlaysSound;
		config.SoundPath = SoundPath;
		config.SoundVolume = SoundVolume;
		config.LoopsSound = LoopsSound;
		config.IsEnabled = IsEnabled;
		config.IsSilenced = IsSilenced;
		config.ShowsDialog = ShowsDialog;
		config.AccelInterval = AccelInterval;

	}

}
