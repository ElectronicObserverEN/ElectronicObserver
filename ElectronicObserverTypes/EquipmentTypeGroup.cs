using System.ComponentModel.DataAnnotations;

namespace ElectronicObserverTypes;

public enum EquipmentTypeGroup
{
	[Display(ResourceType = typeof(Properties.EquipmentTypeGroups), Name = "MainGun")]
	MainGun,

	[Display(ResourceType = typeof(Properties.EquipmentTypeGroups), Name = "Secondary")]
	Secondary,

	[Display(ResourceType = typeof(Properties.EquipmentTypeGroups), Name = "Torpedo")]
	Torpedo,

	[Display(ResourceType = typeof(Properties.EquipmentTypeGroups), Name = "Fighter")]
	Fighter,

	[Display(ResourceType = typeof(Properties.EquipmentTypeGroups), Name = "Bomber")]
	Bomber,

	[Display(ResourceType = typeof(Properties.EquipmentTypeGroups), Name = "TorpedoBomber")]
	TorpedoBomber,

	[Display(ResourceType = typeof(Properties.EquipmentTypeGroups), Name = "LandBasedFighters")]
	LandBasedFighters,

	[Display(ResourceType = typeof(Properties.EquipmentTypeGroups), Name = "LandBasedBombers")]
	LandBasedBombers,

	[Display(ResourceType = typeof(Properties.EquipmentTypeGroups), Name = "SeaplanesAndRecons")]
	SeaplaneAndRecons,

	[Display(ResourceType = typeof(Properties.EquipmentTypeGroups), Name = "Radar")]
	Radar,

	[Display(ResourceType = typeof(Properties.EquipmentTypeGroups), Name = "ASWGears")]
	ASW,

	[Display(ResourceType = typeof(Properties.EquipmentTypeGroups), Name = "Others")]
	Other,

	[Display(ResourceType = typeof(Properties.EquipmentTypeGroups), Name = "Transport")]
	Transport,
}
