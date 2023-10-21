using System.ComponentModel.DataAnnotations;

namespace ElectronicObserverTypes;

public enum ShipTypeGroup
{
	[Display(ResourceType = typeof(Properties.ShipTypeGroups), Name = "Destroyers")]
	Destroyers,
	[Display(ResourceType = typeof(Properties.ShipTypeGroups), Name = "Escorts")]
	Escorts,
	[Display(ResourceType = typeof(Properties.ShipTypeGroups), Name = "LightCruisers")]
	LightCruisers,
	[Display(ResourceType = typeof(Properties.ShipTypeGroups), Name = "HeavyCruisers")]
	HeavyCruisers,
	[Display(ResourceType = typeof(Properties.ShipTypeGroups), Name = "Battleships")]
	Battleships,
	[Display(ResourceType = typeof(Properties.ShipTypeGroups), Name = "Carriers")]
	Carriers,
	[Display(ResourceType = typeof(Properties.ShipTypeGroups), Name = "Submarines")]
	Submarines,
	[Display(ResourceType = typeof(Properties.ShipTypeGroups), Name = "Auxiliaries")]
	Auxiliaries,
}
