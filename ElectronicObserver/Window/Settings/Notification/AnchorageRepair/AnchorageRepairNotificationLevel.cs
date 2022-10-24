using System.ComponentModel.DataAnnotations;
using ElectronicObserver.Properties.Window.Dialog;


namespace ElectronicObserver.Window.Settings.Notification.AnchorageRepair;

public enum AnchorageRepairNotificationLevel
{
	[Display(ResourceType = typeof(DialogConfigurationNotifier), Name = "AnchorageRepairNotificationLevel_Always")]
	Always,
	[Display(ResourceType = typeof(DialogConfigurationNotifier), Name = "AnchorageRepairNotificationLevel_AkashiFlagship")]
	AkashiFlagship,
	[Display(ResourceType = typeof(DialogConfigurationNotifier), Name = "AnchorageRepairNotificationLevel_ShipNeededRepair")]
	WhenRepairing,
	[Display(ResourceType = typeof(DialogConfigurationNotifier), Name = "AnchorageRepairNotificationLevel_Preset")]
	IncludingPresets,
}
