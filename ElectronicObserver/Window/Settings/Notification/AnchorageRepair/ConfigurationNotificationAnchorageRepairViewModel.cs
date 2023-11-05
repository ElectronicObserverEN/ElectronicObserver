using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Notifier;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Settings.Notification.Base;

namespace ElectronicObserver.Window.Settings.Notification.AnchorageRepair;

public class ConfigurationNotificationAnchorageRepairViewModel : ConfigurationNotificationBaseViewModel
{
	protected override NotifierAnchorageRepair NotifierBase { get; }

	public List<AnchorageRepairNotificationLevel> NotificationLevels { get; }

	public AnchorageRepairNotificationLevel NotificationLevel { get; set; }

	public ConfigurationNotificationAnchorageRepairViewModel(
		Configuration.ConfigurationData.ConfigNotifierBase config,
		NotifierAnchorageRepair notifier)
		: base(config, notifier)
	{
		NotificationLevels = Enum.GetValues<AnchorageRepairNotificationLevel>().ToList();

		NotifierBase = notifier;
	}

	public override void Load()
	{
		base.Load();

		NotificationLevel = (AnchorageRepairNotificationLevel)NotifierBase.NotificationLevel;
	}


	public override void Save()
	{
		base.Save();

		NotifierBase.NotificationLevel = (int)NotificationLevel;
	}
}
