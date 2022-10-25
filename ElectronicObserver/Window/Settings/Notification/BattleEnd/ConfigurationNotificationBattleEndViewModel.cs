using ElectronicObserver.Notifier;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Settings.Notification.Base;

namespace ElectronicObserver.Window.Settings.Notification.BattleEnd;

public class ConfigurationNotificationBattleEndViewModel : ConfigurationNotificationBaseViewModel
{
	protected override NotifierBattleEnd NotifierBase { get; }

	public bool IdleTimerEnabled { get; set; }

	public int IdleSeconds { get; set; }

	public ConfigurationNotificationBattleEndViewModel(
		Configuration.ConfigurationData.ConfigNotifierBase config,
		NotifierBattleEnd notifier)
		: base(config, notifier)
	{
		NotifierBase = notifier;
	}

	public override void Load()
	{
		base.Load();

		IdleTimerEnabled = NotifierBase.Config.IdleTimerEnabled;
		IdleSeconds = NotifierBase.Config.IdleSeconds;
	}

	public override void Save()
	{
		base.Save();

		NotifierBase.Config.IdleTimerEnabled = IdleTimerEnabled;
		NotifierBase.Config.IdleSeconds = IdleSeconds;
	}
}
