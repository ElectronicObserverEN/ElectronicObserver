using System.ComponentModel.DataAnnotations;
using ElectronicObserver.Properties.Window;

namespace ElectronicObserver.Window.Settings.SubWindow.Headquarters;

public enum HeadquartersItem
{
	[Display(ResourceType = typeof(FormHeadQuarters), Name = "ItemNameName")]
	Name,

	[Display(ResourceType = typeof(FormHeadQuarters), Name = "ItemNameComment")]
	Comment,

	[Display(ResourceType = typeof(FormHeadQuarters), Name = "ItemNameHQLevel")]
	HeadquartersLevel,

	[Display(ResourceType = typeof(FormHeadQuarters), Name = "ItemNameShipSlots")]
	ShipSlots,

	[Display(ResourceType = typeof(FormHeadQuarters), Name = "ItemNameEquipmentSlots")]
	EquipmentSlots,

	[Display(ResourceType = typeof(FormHeadQuarters), Name = "ItemNameInstantRepair")]
	InstantRepair,

	[Display(ResourceType = typeof(FormHeadQuarters), Name = "ItemNameInstantConstruction")]
	InstantConstruction,

	[Display(ResourceType = typeof(FormHeadQuarters), Name = "ItemNameDevelopmentMaterial")]
	DevelopmentMaterial,

	[Display(ResourceType = typeof(FormHeadQuarters), Name = "ItemNameImproveMaterial")]
	ImprovementMaterial,

	[Display(ResourceType = typeof(FormHeadQuarters), Name = "ItemNameFurnitureCoin")]
	FurnitureCoins,

	[Display(ResourceType = typeof(FormHeadQuarters), Name = "ItemNameFuel")]
	Fuel,

	[Display(ResourceType = typeof(FormHeadQuarters), Name = "ItemNameAmmo")]
	Ammo,

	[Display(ResourceType = typeof(FormHeadQuarters), Name = "ItemNameSteel")]
	Steel,

	[Display(ResourceType = typeof(FormHeadQuarters), Name = "ItemNameBauxite")]
	Bauxite,

	[Display(ResourceType = typeof(FormHeadQuarters), Name = "ItemNameOtherItem")]
	OtherItem,
}
