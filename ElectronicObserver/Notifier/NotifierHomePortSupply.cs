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

	private DateTime LastNotificationTime { get; set; } = DateTime.Now;

	public NotifierHomePortSupply(Configuration.ConfigurationData.ConfigNotifierHomePortSupply config)
		: base(config)
	{
		DialogData.Title = NotifierRes.HomePortSupply;

		NotificationLevel = (HomePortSupplyNotificationLevel)config.NotificationLevel;
	}

	protected override void UpdateTimerTick()
	{
		FleetManager fleets = KCDatabase.Instance.Fleet;
		HomePortSupplyService homePortSupplyService = KCDatabase.Instance.HomePortSupplyService;

		if (LastNotificationTime > homePortSupplyService.HomePortSupplyTimer) return;

		if ((DateTime.Now - homePortSupplyService.HomePortSupplyTimer).TotalMilliseconds + AccelInterval >= 15 * 60 * 1000)
		{
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
	}

	public override void Notify()
	{
		DialogData.Message = NotifierRes.HomePortSupplyFinished;

		base.Notify();
	}

	public override void ApplyToConfiguration(Configuration.ConfigurationData.ConfigNotifierBase config)
	{
		base.ApplyToConfiguration(config);

		if (config is not Configuration.ConfigurationData.ConfigNotifierHomePortSupply c) return;

		c.NotificationLevel = (int)NotificationLevel;
	}
}
