using System;
using System.Collections.Generic;
using ElectronicObserver.Notifier;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Settings.Notification.Base;

namespace ElectronicObserver.Window.Settings.Notification.HomePortSupply;

public class ConfigurationNotificationHomePortSupplyViewModel(
	Configuration.ConfigurationData.ConfigNotifierBase config,
	NotifierHomePortSupply notifier) : ConfigurationNotificationBaseViewModel(config, notifier)
{
	protected override NotifierHomePortSupply NotifierBase { get; } = notifier;

	public List<HomePortSupplyNotificationLevel> NotificationLevels { get; } = [.. Enum.GetValues<HomePortSupplyNotificationLevel>()];

	public HomePortSupplyNotificationLevel NotificationLevel { get; set; }
	public bool NotifyForExpeditions { get; set; }
	public int ExpeditionAccelInterval { get; set; }

	public override void Load()
	{
		base.Load();

		NotificationLevel = NotifierBase.NotificationLevel;
		NotifyForExpeditions = NotifierBase.NotifyForExpeditions;
		ExpeditionAccelInterval = NotifierBase.ExpeditionAccelInterval / 1000;
	}

	public override void Save()
	{
		base.Save();

		NotifierBase.NotificationLevel = NotificationLevel;
		NotifierBase.NotifyForExpeditions = NotifyForExpeditions;
		NotifierBase.ExpeditionAccelInterval = ExpeditionAccelInterval * 1000;
	}
}
