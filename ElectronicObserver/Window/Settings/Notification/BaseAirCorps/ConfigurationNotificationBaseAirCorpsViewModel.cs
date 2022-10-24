using ElectronicObserver.Notifier;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Settings.Notification.Base;

namespace ElectronicObserver.Window.Settings.Notification.BaseAirCorps;

public class ConfigurationNotificationBaseAirCorpsViewModel : ConfigurationNotificationBaseViewModel
{
	protected override NotifierBaseAirCorps NotifierBase { get; }

	public bool NotifiesNotSupplied { get; set; }

	public bool NotifiesTired { get; set; }

	public bool NotifiesNotOrganized { get; set; }

	public bool NotifiesStandby { get; set; }

	public bool NotifiesRetreat { get; set; }

	public bool NotifiesRest { get; set; }

	public bool NotifiesNormalMap { get; set; }

	public bool NotifiesEventMap { get; set; }

	public bool NotifiesSquadronRelocation { get; set; }

	public bool NotifiesEquipmentRelocation { get; set; }

	public ConfigurationNotificationBaseAirCorpsViewModel(
		Configuration.ConfigurationData.ConfigNotifierBase config,
		NotifierBaseAirCorps notifier)
		: base(config, notifier)
	{
		NotifierBase = notifier;
	}

	public override void Load()
	{
		base.Load();

		NotifiesNotSupplied = NotifierBase.NotifiesNotSupplied;
		NotifiesTired = NotifierBase.NotifiesTired;
		NotifiesNotOrganized = NotifierBase.NotifiesNotOrganized;

		NotifiesStandby = NotifierBase.NotifiesStandby;
		NotifiesRetreat = NotifierBase.NotifiesRetreat;
		NotifiesRest = NotifierBase.NotifiesRest;

		NotifiesNormalMap = NotifierBase.NotifiesNormalMap;
		NotifiesEventMap = NotifierBase.NotifiesEventMap;

		NotifiesSquadronRelocation = NotifierBase.NotifiesSquadronRelocation;
		NotifiesEquipmentRelocation = NotifierBase.NotifiesEquipmentRelocation;
	}

	public override void Save()
	{
		base.Save();

		NotifierBase.NotifiesNotSupplied = NotifiesNotSupplied;
		NotifierBase.NotifiesTired = NotifiesTired;
		NotifierBase.NotifiesNotOrganized = NotifiesNotOrganized;

		NotifierBase.NotifiesStandby = NotifiesStandby;
		NotifierBase.NotifiesRetreat = NotifiesRetreat;
		NotifierBase.NotifiesRest = NotifiesRest;

		NotifierBase.NotifiesNormalMap = NotifiesNormalMap;
		NotifierBase.NotifiesEventMap = NotifiesEventMap;

		NotifierBase.NotifiesSquadronRelocation = NotifiesSquadronRelocation;
		NotifierBase.NotifiesEquipmentRelocation = NotifiesEquipmentRelocation;
	}
}
