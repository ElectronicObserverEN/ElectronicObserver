using ElectronicObserver.Notifier;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Window.Settings.Notification.Damage;

public class ConfigurationNotificationDamageViewModel : Base.ConfigurationNotificationBaseViewModel
{
	protected override NotifierDamage NotifierBase { get; }

	public bool NotifiesBefore { get; set; }

	public bool NotifiesNow { get; set; }

	public bool NotifiesAfter { get; set; }

	public bool ContainsNotLockedShip { get; set; }

	public bool ContainsSafeShip { get; set; }

	public bool ContainsFlagship { get; set; }

	public int LevelBorder { get; set; }

	public bool NotifiesAtEndpoint { get; set; }

	public ConfigurationNotificationDamageViewModel(
		Configuration.ConfigurationData.ConfigNotifierBase config, 
		NotifierDamage notifier) 
		: base(config, notifier)
	{
		NotifierBase = notifier;
	}

	public override void Load()
	{
		base.Load();

		NotifiesBefore = NotifierBase.NotifiesBefore;
		NotifiesNow = NotifierBase.NotifiesNow;
		NotifiesAfter = NotifierBase.NotifiesAfter;
		ContainsNotLockedShip = NotifierBase.ContainsNotLockedShip;
		ContainsSafeShip = NotifierBase.ContainsSafeShip;
		ContainsFlagship = NotifierBase.ContainsFlagship;
		LevelBorder = NotifierBase.LevelBorder;
		NotifiesAtEndpoint = NotifierBase.NotifiesAtEndpoint;
	}

	public override void Save()
	{
		base.Save();

		NotifierBase.NotifiesBefore = NotifiesBefore;
		NotifierBase.NotifiesNow = NotifiesNow;
		NotifierBase.NotifiesAfter = NotifiesAfter;
		NotifierBase.ContainsNotLockedShip = ContainsNotLockedShip;
		NotifierBase.ContainsSafeShip = ContainsSafeShip;
		NotifierBase.ContainsFlagship = ContainsFlagship;
		NotifierBase.LevelBorder = LevelBorder;
		NotifierBase.NotifiesAtEndpoint = NotifiesAtEndpoint;
	}
}
