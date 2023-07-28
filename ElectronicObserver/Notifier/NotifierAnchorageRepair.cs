using System;
using System.Linq;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Notifier;

public class NotifierAnchorageRepair : NotifierBase
{

	/// <summary>
	/// 通知レベル
	/// 0 = いつでも、1 = 明石旗艦の時、2 = 修理艦もいる時、3 = 2 + プリセット編成時
	/// </summary>
	public int NotificationLevel { get; set; }



	// いったん母港に行くまでは通知しない
	private bool ProcessedFlag { get; set; } = true;

	public NotifierAnchorageRepair(Utility.Configuration.ConfigurationData.ConfigNotifierAnchorageRepair config)
		: base(config)
	{
		DialogData.Title = NotifierRes.AnchorageRepair;

		SubscribeToApis();

		NotificationLevel = config.NotificationLevel;
	}

	private void SubscribeToApis()
	{
		APIObserver o = APIObserver.Instance;

		o.ApiPort_Port.ResponseReceived += ClearFlag;
	}

	private void ClearFlag(string apiname, dynamic data)
	{
		ProcessedFlag = false;
	}


	protected override void UpdateTimerTick()
	{

		var fleets = KCDatabase.Instance.Fleet;

		if (!ProcessedFlag)
		{

			if ((DateTime.Now - fleets.AnchorageRepairingTimer).TotalMilliseconds + AccelInterval >= 20 * 60 * 1000)
			{
				bool clear = NotificationLevel switch
				{
					//いつでも
					0 => true,

					//明石旗艦の時
					1 => fleets.Fleets.Values.Any(f => f.IsFlagshipRepairShip),

					//修理艦もいる時
					2 => fleets.Fleets.Values.Any(f => f.CanAnchorageRepair),

					// プリセット込み
					3 => fleets.Fleets.Values.Any(f => f.CanAnchorageRepair) ||
						KCDatabase.Instance.FleetPreset.Presets.Values.Any(p =>
						FleetData.CanAnchorageRepairWithMember(p.MembersInstance)),

					_ => true,
				};

				if (clear)
				{

					Notify();
					ProcessedFlag = true;
				}
			}
		}

	}


	public override void Notify()
	{

		DialogData.Message = NotifierRes.AnchorageRepairFinished;

		base.Notify();
	}


	public override void ApplyToConfiguration(Utility.Configuration.ConfigurationData.ConfigNotifierBase config)
	{
		base.ApplyToConfiguration(config);

		if (config is Configuration.ConfigurationData.ConfigNotifierAnchorageRepair c)
		{
			c.NotificationLevel = NotificationLevel;
		}
	}

}
