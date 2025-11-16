using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using ElectronicObserver.Core.Types.Data;
using ElectronicObserver.Observer;

namespace ElectronicObserver.Utility;

public sealed class SyncBGMPlayer
{

	#region Singleton

	private static readonly SyncBGMPlayer instance = new SyncBGMPlayer();

	public static SyncBGMPlayer Instance => instance;

	#endregion


	[DataContract(Name = "SoundHandle")]
	public class SoundHandle : IIdentifiable, ICloneable
	{

		[DataMember]
		public SoundHandleID HandleID { get; set; }

		[DataMember]
		public bool Enabled { get; set; }

		[DataMember]
		public string Path { get; set; }

		[DataMember]
		public bool IsLoop { get; set; }

		[DataMember]
		public double LoopHeadPosition { get; set; }

		[DataMember]
		public int Volume { get; set; }

		public SoundHandle(SoundHandleID id)
		{
			HandleID = id;
			Enabled = true;
			Path = "";
			IsLoop = true;
			LoopHeadPosition = 0.0;
			Volume = 100;
		}

		[IgnoreDataMember]
		[JsonIgnore]
		public int ID => (int)HandleID;

		public override string ToString() => Enum.GetName(typeof(SoundHandleID), HandleID) + " : " + Path;


		public SoundHandle Clone()
		{
			return (SoundHandle)MemberwiseClone();
		}

		object ICloneable.Clone()
		{
			return Clone();
		}
	}

	public enum SoundHandleID
	{
		Port = 1,
		Sortie = 101,
		BattleDay = 201,
		BattleNight,
		BattleAir,
		BattleBoss,
		BattlePracticeDay,
		BattlePracticeNight,
		ResultWin = 301,
		ResultLose,
		ResultBossWin,
		Record = 401,
		Item,
		Quest,
		Album,
		ImprovementArsenal,
	}

	private IDDictionary<SoundHandle> Handles { get; set; } = [];
	private bool Enabled { get; set; }
	public bool IsMute
	{
		get => MediaPlayer.IsMute;
		set => MediaPlayer.IsMute = value;
	}

	private EOMediaPlayer MediaPlayer { get; }
	private SoundHandleID CurrentSoundHandleId { get; set; } = (SoundHandleID)(-1);
	private bool IsBoss { get; set; }


	private SyncBGMPlayer()
	{

		MediaPlayer = new EOMediaPlayer
		{
			AutoPlay = false,
			IsShuffle = true
		};

		foreach (SoundHandleID id in Enum.GetValues<SoundHandleID>())
		{
			Handles.Add(new SoundHandle(id));
		}

		#region API register
		APIObserver o = APIObserver.Instance;

		o.ApiPort_Port.ResponseReceived += PlayPort;

		o.ApiReqMap_Start.ResponseReceived += PlaySortie;
		o.ApiReqMap_Next.ResponseReceived += PlaySortie;

		o.ApiReqSortie_Battle.ResponseReceived += PlayBattleDay;
		o.ApiReqCombinedBattle_Battle.ResponseReceived += PlayBattleDay;
		o.ApiReqCombinedBattle_BattleWater.ResponseReceived += PlayBattleDay;
		o.ApiReqCombinedBattle_EcBattle.ResponseReceived += PlayBattleDay;
		o.ApiReqCombinedBattle_EachBattle.ResponseReceived += PlayBattleDay;
		o.ApiReqCombinedBattle_EachBattleWater.ResponseReceived += PlayBattleDay;

		o.ApiReqBattleMidnight_Battle.ResponseReceived += PlayBattleNight;
		o.ApiReqBattleMidnight_SpMidnight.ResponseReceived += PlayBattleNight;
		o.ApiReqSortie_NightToDay.ResponseReceived += PlayBattleNight;
		o.ApiReqSortie_LdShooting.ResponseReceived += PlayBattleNight;
		o.ApiReqCombinedBattle_MidnightBattle.ResponseReceived += PlayBattleNight;
		o.ApiReqCombinedBattle_SpMidnight.ResponseReceived += PlayBattleNight;
		o.ApiReqCombinedBattle_EcMidnightBattle.ResponseReceived += PlayBattleNight;
		o.ApiReqCombinedBattle_EcNightToDay.ResponseReceived += PlayBattleNight;
		o.ApiReqCombinedBattle_LdShooting.ResponseReceived += PlayBattleNight;

		o.ApiReqSortie_AirBattle.ResponseReceived += PlayBattleAir;
		o.ApiReqCombinedBattle_AirBattle.ResponseReceived += PlayBattleAir;
		o.ApiReqSortie_LdAirBattle.ResponseReceived += PlayBattleAir;
		o.ApiReqCombinedBattle_LdAirBattle.ResponseReceived += PlayBattleAir;

		o.ApiReqPractice_Battle.ResponseReceived += PlayPracticeDay;

		o.ApiReqPractice_MidnightBattle.ResponseReceived += PlayPracticeNight;

		o.ApiReqSortie_BattleResult.ResponseReceived += PlayBattleResult;
		o.ApiReqCombinedBattle_BattleResult.ResponseReceived += PlayBattleResult;
		o.ApiReqPractice_BattleResult.ResponseReceived += PlayBattleResult;

		o.ApiGetMember_Record.ResponseReceived += PlayRecord;

		o.ApiGetMember_PayItem.ResponseReceived += PlayItem;

		o.ApiGetMember_QuestList.ResponseReceived += PlayQuest;

		o.ApiGetMember_PictureBook.ResponseReceived += PlayAlbum;

		o.ApiReqKousyou_RemodelSlotList.ResponseReceived += PlayImprovementArsenal;

		#endregion

		Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		SystemEvents.SystemShuttingDown += SystemEvents_SystemShuttingDown;
	}

	public void ConfigurationChanged()
	{
		var c = Utility.Configuration.Config.BGMPlayer;

		Enabled = c.Enabled;

		if (c.Handles != null)
			Handles = new IDDictionary<SoundHandle>(c.Handles);

		if (!c.SyncBrowserMute)
			IsMute = false;

		// 設定変更を適用するためいったん閉じる
		MediaPlayer.Close();
		CurrentSoundHandleId = (SoundHandleID)(-1);
	}

	void SystemEvents_SystemShuttingDown()
	{
		var c = Utility.Configuration.Config.BGMPlayer;

		c.Enabled = Enabled;
		c.Handles = Handles.Values.ToList();
	}


	public void SetInitialVolume(int volume)
	{
		MediaPlayer.Volume = volume;
	}



	void PlayPort(string apiname, dynamic data)
	{
		IsBoss = false;
		Play(Handles[(int)SoundHandleID.Port]);
	}

	void PlaySortie(string apiname, dynamic data)
	{
		Play(Handles[(int)SoundHandleID.Sortie]);
		IsBoss = (int)data.api_event_id == 5;
	}

	void PlayBattleDay(string apiname, dynamic data)
	{
		if (IsBoss)
			Play(Handles[(int)SoundHandleID.BattleBoss]);
		else
			Play(Handles[(int)SoundHandleID.BattleDay]);
	}

	void PlayBattleNight(string apiname, dynamic data)
	{
		if (IsBoss)
			Play(Handles[(int)SoundHandleID.BattleBoss]);
		else
			Play(Handles[(int)SoundHandleID.BattleNight]);
	}

	void PlayBattleAir(string apiname, dynamic data)
	{
		if (IsBoss)
			Play(Handles[(int)SoundHandleID.BattleBoss]);
		else
			Play(Handles[(int)SoundHandleID.BattleAir]);
	}

	void PlayPracticeDay(string apiname, dynamic data)
	{
		Play(Handles[(int)SoundHandleID.BattlePracticeDay]);
	}

	void PlayPracticeNight(string apiname, dynamic data)
	{
		Play(Handles[(int)SoundHandleID.BattlePracticeNight]);

	}

	void PlayBattleResult(string apiname, dynamic data)
	{
		switch ((string)data.api_win_rank)
		{
			case "S":
			case "A":
			case "B":
				if (IsBoss)
					Play(Handles[(int)SoundHandleID.ResultBossWin]);
				else
					Play(Handles[(int)SoundHandleID.ResultWin]);
				break;
			default:
				Play(Handles[(int)SoundHandleID.ResultLose]);
				break;
		}
	}


	void PlayRecord(string apiname, dynamic data)
	{
		Play(Handles[(int)SoundHandleID.Record]);
	}

	void PlayItem(string apiname, dynamic data)
	{
		Play(Handles[(int)SoundHandleID.Item]);
	}

	void PlayQuest(string apiname, dynamic data)
	{
		Play(Handles[(int)SoundHandleID.Quest]);
	}

	void PlayAlbum(string apiname, dynamic data)
	{
		Play(Handles[(int)SoundHandleID.Album]);
	}

	void PlayImprovementArsenal(string apiname, dynamic data)
	{
		Play(Handles[(int)SoundHandleID.ImprovementArsenal]);
	}


	private void Play(SoundHandle sh)
	{
		if (Enabled &&
			sh != null &&
			sh.Enabled &&
			!string.IsNullOrWhiteSpace(sh.Path) &&
			sh.HandleID != CurrentSoundHandleId)
		{


			if (File.Exists(sh.Path))
			{
				MediaPlayer.Close();
				MediaPlayer.SetPlaylist(null);
				MediaPlayer.SourcePath = sh.Path;

			}
			else if (Directory.Exists(sh.Path))
			{
				MediaPlayer.Close();
				MediaPlayer.SetPlaylistFromDirectory(sh.Path);

			}
			else
			{
				return;
			}

			CurrentSoundHandleId = sh.HandleID;

			MediaPlayer.IsLoop = sh.IsLoop;
			MediaPlayer.LoopHeadPosition = TimeSpan.FromSeconds(sh.LoopHeadPosition);
			if (!Utility.Configuration.Config.Control.UseSystemVolume)
				MediaPlayer.Volume = sh.Volume;
			MediaPlayer.Play();
		}
	}



	public static string SoundHandleIDToString(SoundHandleID id)
	{
		switch (id)
		{
			case SoundHandleID.Port:
				return ConstantsRes.Port;
			case SoundHandleID.Sortie:
				return ConstantsRes.BGM_Sortie;
			case SoundHandleID.BattleDay:
				return ConstantsRes.BGM_BattleDay;
			case SoundHandleID.BattleNight:
				return ConstantsRes.BGM_BattleNight;
			case SoundHandleID.BattleAir:
				return ConstantsRes.BGM_BattleAir;
			case SoundHandleID.BattleBoss:
				return ConstantsRes.BGM_BattleBoss;
			case SoundHandleID.BattlePracticeDay:
				return "演習昼戦";
			case SoundHandleID.BattlePracticeNight:
				return "演習夜戦";
			case SoundHandleID.ResultWin:
				return "勝利";
			case SoundHandleID.ResultLose:
				return "敗北";
			case SoundHandleID.ResultBossWin:
				return "ボス勝利";
			case SoundHandleID.Record:
				return "戦績";
			case SoundHandleID.Item:
				return "アイテム";
			case SoundHandleID.Quest:
				return "任務";
			case SoundHandleID.Album:
				return "図鑑";
			case SoundHandleID.ImprovementArsenal:
				return "改修工廠";
			default:
				return ConstantsRes.Unknown;
		}
	}
}
