using System.Timers;
using ElectronicObserver.Observer;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Notifier;

public class NotifierBattleEnd : NotifierBase
{
	public Configuration.ConfigurationData.ConfigNotifierBattleEnd Config { get; }
	private Timer? Timer { get; set; }

	public NotifierBattleEnd(Configuration.ConfigurationData.ConfigNotifierBattleEnd config)
		: base(config)
	{
		Initialize();

		Config = config;
	}

	private void Initialize()
	{
		DialogData.Title = "Battle notification";

		APIObserver o = APIObserver.Instance;

		o.ApiPort_Port.ResponseReceived += CloseAll;

		o.ApiReqSortie_BattleResult.ResponseReceived += BattleFinished;
		o.ApiReqCombinedBattle_BattleResult.ResponseReceived += BattleFinished;

		o.ApiReqSortie_Battle.ResponseReceived += BattleStarted;
		o.ApiReqBattleMidnight_Battle.ResponseReceived += BattleStarted;
		o.ApiReqBattleMidnight_SpMidnight.ResponseReceived += BattleStarted;
		o.ApiReqSortie_AirBattle.ResponseReceived += BattleStarted;
		o.ApiReqSortie_LdAirBattle.ResponseReceived += BattleStarted;
		o.ApiReqSortie_NightToDay.ResponseReceived += BattleStarted;
		o.ApiReqSortie_LdShooting.ResponseReceived += BattleStarted;
		o.ApiReqCombinedBattle_Battle.ResponseReceived += BattleStarted;
		o.ApiReqCombinedBattle_BattleWater.ResponseReceived += BattleStarted;
		o.ApiReqCombinedBattle_AirBattle.ResponseReceived += BattleStarted;
		o.ApiReqCombinedBattle_MidnightBattle.ResponseReceived += BattleStarted;
		o.ApiReqCombinedBattle_SpMidnight.ResponseReceived += BattleStarted;
		o.ApiReqCombinedBattle_LdAirBattle.ResponseReceived += BattleStarted;
		o.ApiReqCombinedBattle_EcBattle.ResponseReceived += BattleStarted;
		o.ApiReqCombinedBattle_EcMidnightBattle.ResponseReceived += BattleStarted;
		o.ApiReqCombinedBattle_EcNightToDay.ResponseReceived += BattleStarted;
		o.ApiReqCombinedBattle_EachBattle.ResponseReceived += BattleStarted;
		o.ApiReqCombinedBattle_EachBattleWater.ResponseReceived += BattleStarted;
		o.ApiReqCombinedBattle_LdShooting.ResponseReceived += BattleStarted;
	}

	private void BattleStarted(string apiname, dynamic data)
	{
		Timer = new Timer
		{
			Enabled = Config.IdleTimerEnabled,
			Interval = Config.IdleSeconds * 1000,
			AutoReset = false,
		};

		Timer.Elapsed += (s, e) =>
		{
			Notify("Idle warning.");
		};
	}

	private void BattleFinished(string apiname, dynamic data)
	{
		Timer?.Stop();
		Notify("Battle ended.");
	}

	void CloseAll(string apiname, dynamic data)
	{
		Timer?.Stop();
		DialogData.OnCloseAll();
	}

	public void Notify(string message)
	{
		DialogData.Message = message;
		base.Notify();
	}


	public override void ApplyToConfiguration(Utility.Configuration.ConfigurationData.ConfigNotifierBase config)
	{
		base.ApplyToConfiguration(config);

		if (config is Configuration.ConfigurationData.ConfigNotifierBattleEnd c)
		{
			c.IdleTimerEnabled = Config.IdleTimerEnabled;
			c.IdleSeconds = Config.IdleSeconds;
		}
	}
}
