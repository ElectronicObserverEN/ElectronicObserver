using System.ComponentModel.DataAnnotations;

namespace ElectronicObserver.Window.Settings.SubWindow.Headquarters;

public enum HeadquartersItem
{
	[Display(ResourceType = typeof(HeadquartersResources), Name = "ItemNameName")]
	Name,

	[Display(ResourceType = typeof(HeadquartersResources), Name = "ItemNameComment")]
	Comment,

	[Display(ResourceType = typeof(HeadquartersResources), Name = "ItemNameHQLevel")]
	HeadquartersLevel,

	[Display(ResourceType = typeof(HeadquartersResources), Name = "ItemNameShipSlots")]
	ShipSlots,

	[Display(ResourceType = typeof(HeadquartersResources), Name = "ItemNameEquipmentSlots")]
	EquipmentSlots,

	[Display(ResourceType = typeof(HeadquartersResources), Name = "ItemNameInstantRepair")]
	InstantRepair,

	[Display(ResourceType = typeof(HeadquartersResources), Name = "ItemNameInstantConstruction")]
	InstantConstruction,

	[Display(ResourceType = typeof(HeadquartersResources), Name = "ItemNameDevelopmentMaterial")]
	DevelopmentMaterial,

	[Display(ResourceType = typeof(HeadquartersResources), Name = "ItemNameImproveMaterial")]
	ImprovementMaterial,

	[Display(ResourceType = typeof(HeadquartersResources), Name = "ItemNameFurnitureCoin")]
	FurnitureCoins,

	[Display(ResourceType = typeof(HeadquartersResources), Name = "ItemNameFuel")]
	Fuel,

	[Display(ResourceType = typeof(HeadquartersResources), Name = "ItemNameAmmo")]
	Ammo,

	[Display(ResourceType = typeof(HeadquartersResources), Name = "ItemNameSteel")]
	Steel,

	[Display(ResourceType = typeof(HeadquartersResources), Name = "ItemNameBauxite")]
	Bauxite,

	[Display(ResourceType = typeof(HeadquartersResources), Name = "ItemNameOtherItem")]
	OtherItem,
}
