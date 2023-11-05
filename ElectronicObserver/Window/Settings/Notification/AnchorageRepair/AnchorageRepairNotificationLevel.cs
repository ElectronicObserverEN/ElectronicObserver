using System.ComponentModel.DataAnnotations;

namespace ElectronicObserver.Window.Settings.Notification.AnchorageRepair;

public enum AnchorageRepairNotificationLevel
{
	[Display(ResourceType = typeof(ConfigurationNotifierResources), Name = "AnchorageRepairNotificationLevel_Always")]
	Always,
	[Display(ResourceType = typeof(ConfigurationNotifierResources), Name = "AnchorageRepairNotificationLevel_AkashiFlagship")]
	AkashiFlagship,
	[Display(ResourceType = typeof(ConfigurationNotifierResources), Name = "AnchorageRepairNotificationLevel_ShipNeededRepair")]
	WhenRepairing,
	[Display(ResourceType = typeof(ConfigurationNotifierResources), Name = "AnchorageRepairNotificationLevel_Preset")]
	IncludingPresets,
}
