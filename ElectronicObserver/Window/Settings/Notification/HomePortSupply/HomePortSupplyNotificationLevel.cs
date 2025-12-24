using System.ComponentModel.DataAnnotations;

namespace ElectronicObserver.Window.Settings.Notification.HomePortSupply;

public enum HomePortSupplyNotificationLevel
{
	[Display(ResourceType = typeof(ConfigurationNotifierResources), Name = "HomePortSupplyNotificationLevel_Always")]
	Always,

	[Display(ResourceType = typeof(ConfigurationNotifierResources), Name = "HomePortSupplyNotificationLevel_HomePortSupplyFleet")]
	HomePortSupplyFleet,

	[Display(ResourceType = typeof(ConfigurationNotifierResources), Name = "HomePortSupplyNotificationLevel_HomePortSupplyAvailable")]
	HomePortSupplyAvailable,

	[Display(ResourceType = typeof(ConfigurationNotifierResources), Name = "HomePortSupplyNotificationLevel_IncludingPresets")]
	IncludingPresets,
}
