using System.ComponentModel.DataAnnotations;

namespace ElectronicObserver.Core.Types;

public enum EquipmentTypeGroup
{
	[Display(ResourceType = typeof(Properties.EquipmentTypeGroups), Name = "MainGunSmall")]
	MainGunSmall,

	[Display(ResourceType = typeof(Properties.EquipmentTypeGroups), Name = "MainGunMedium")]
	MainGunMedium,

	[Display(ResourceType = typeof(Properties.EquipmentTypeGroups), Name = "MainGunLarge")]
	MainGunLarge,

	[Display(ResourceType = typeof(Properties.EquipmentTypeGroups), Name = "Secondary")]
	Secondary,

	[Display(ResourceType = typeof(Properties.EquipmentTypeGroups), Name = "AntiAir")]
	AntiAir,

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
