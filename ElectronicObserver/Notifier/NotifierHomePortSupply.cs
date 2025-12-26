using System;
using System.Linq;
using ElectronicObserver.Core.Services;
using ElectronicObserver.Data;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Settings.Notification.HomePortSupply;

namespace ElectronicObserver.Notifier;

public class NotifierHomePortSupply : NotifierBase
{
	public HomePortSupplyNotificationLevel NotificationLevel { get; set; }
	public bool NotifyForExpeditions { get; set; }
	public int ExpeditionAccelInterval { get; set; }

	private DateTime LastNotificationTime { get; set; } = DateTime.Now;

	/// <summary>
	/// Last notification time for fleet 2.
	/// </summary>
	private DateTime LastExpeditionNotificationTime1 { get; set; } = DateTime.Now;

	/// <summary>
	/// Last notification time for fleet 3.
	/// </summary>
	private DateTime LastExpeditionNotificationTime2 { get; set; } = DateTime.Now;

	/// <summary>
	/// Last notification time for fleet 4.
	/// </summary>
	private DateTime LastExpeditionNotificationTime3 { get; set; } = DateTime.Now;

	public NotifierHomePortSupply(Configuration.ConfigurationData.ConfigNotifierHomePortSupply config)
		: base(config)
	{
		DialogData.Title = NotifierRes.HomePortSupply;

		NotificationLevel = (HomePortSupplyNotificationLevel)config.NotificationLevel;
		NotifyForExpeditions = config.NotifyForExpeditions;
		ExpeditionAccelInterval = config.ExpeditionAccelInterval;
	}

	protected override void UpdateTimerTick()
	{
		RegularNotification();
		ExpeditionNotification();
	}

	private void RegularNotification()
	{
		FleetManager fleets = KCDatabase.Instance.Fleet;
		HomePortSupplyService homePortSupplyService = KCDatabase.Instance.HomePortSupplyService;

		if (LastNotificationTime > homePortSupplyService.HomePortSupplyTimer) return;

		if ((DateTime.Now - homePortSupplyService.HomePortSupplyTimer).TotalMilliseconds + AccelInterval < 15 * 60 * 1000)
		{
			return;
		}

		bool shouldNotify = NotificationLevel switch
		{
			HomePortSupplyNotificationLevel.Always => true,

			HomePortSupplyNotificationLevel.HomePortSupplyFleet
				=> fleets.Fleets.Values.Any(HomePortSupplyService.IsHomePortSupplyFleet),

			HomePortSupplyNotificationLevel.HomePortSupplyAvailable
				=> fleets.Fleets.Values.Any(HomePortSupplyService.CanHomePortSupply),

			HomePortSupplyNotificationLevel.IncludingPresets
				=> fleets.Fleets.Values.Any(HomePortSupplyService.CanHomePortSupply)
				|| KCDatabase.Instance.FleetPreset.Presets.Values.Any(p =>
					FleetData.CanHomePortSupplyWithMember(p.MembersInstance)),

			_ => true,
		};

		if (!shouldNotify) return;

		Notify();
		LastNotificationTime = DateTime.Now;
	}

	private void ExpeditionNotification()
	{
		if (!NotifyForExpeditions) return;

		FleetManager fleetManager = KCDatabase.Instance.Fleet;

		LastExpeditionNotificationTime1 = ExpeditionNotification(LastExpeditionNotificationTime1, fleetManager.Fleets[2]);
		LastExpeditionNotificationTime2 = ExpeditionNotification(LastExpeditionNotificationTime2, fleetManager.Fleets[3]);
		LastExpeditionNotificationTime3 = ExpeditionNotification(LastExpeditionNotificationTime3, fleetManager.Fleets[4]);
	}

	private DateTime ExpeditionNotification(DateTime lastNotificationTime, FleetData? fleet)
	{
		if (fleet is null) return lastNotificationTime;

		int thresholdMilliseconds = 15 * 60 * 1000 + ExpeditionAccelInterval;
		DateTime notifyTime = fleet.ExpeditionTime.AddMilliseconds(-thresholdMilliseconds);

		if (lastNotificationTime >= notifyTime) return lastNotificationTime;
		if (DateTime.Now < notifyTime) return lastNotificationTime;
		
		if (!HomePortSupplyService.IsHomePortSupplyFleet(fleet)) return lastNotificationTime;

		NotifyForExpedition(fleet.Name, TimeSpan.FromMilliseconds(thresholdMilliseconds));

		return DateTime.Now;
	}

	public override void Notify()
	{
		DialogData.Message = NotifierRes.HomePortSupplyFinished;

		base.Notify();
	}

	private void NotifyForExpedition(string fleetName, TimeSpan remainingTime)
	{
		DialogData.Message = string.Format(NotifierRes.HomePortSupplyExpeditionFinished, fleetName, remainingTime);

		base.Notify();
	}

	public override void ApplyToConfiguration(Configuration.ConfigurationData.ConfigNotifierBase config)
	{
		base.ApplyToConfiguration(config);

		if (config is not Configuration.ConfigurationData.ConfigNotifierHomePortSupply c) return;

		c.NotificationLevel = (int)NotificationLevel;
		c.NotifyForExpeditions = NotifyForExpeditions;
		c.ExpeditionAccelInterval = ExpeditionAccelInterval;
	}
}
