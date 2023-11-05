using ElectronicObserverTypes;
using ElectronicObserverTypes.Data;

namespace ElectronicObserver.Data;

/// <summary>
/// 消費アイテムのマスターデータを保持します。
/// </summary>
public class UseItemMaster : ResponseWrapper, IIdentifiable
{

	/// <summary>
	/// アイテムID
	/// </summary>
	public UseItemId ItemID => (UseItemId)(int)RawData.api_id;

	/// <summary>
	/// 使用形態
	/// 1=高速修復材, 2=高速建造材, 3=開発資材, 4=資源還元, その他
	/// </summary>
	public int UseType => (int)RawData.api_usetype;

	/// <summary>
	/// カテゴリ
	/// </summary>
	public int Category => (int)RawData.api_category;

	/// <summary>
	/// アイテム名
	/// </summary>
	public string NameTranslated => ItemID switch
	{
		UseItemId.InstantRepair => HeadquartersResources.ItemNameInstantRepair,
		UseItemId.InstantConstruction => HeadquartersResources.ItemNameInstantConstruction,
		UseItemId.DevelopmentMaterial => HeadquartersResources.ItemNameDevelopmentMaterial,
		UseItemId.ImproveMaterial => HeadquartersResources.ItemNameImproveMaterial,
		UseItemId.FurnitureBoxSmall => HeadquartersResources.ItemNameFurnitureBoxSmall,
		UseItemId.FurnitureBoxMedium => HeadquartersResources.ItemNameFurnitureBoxMedium,
		UseItemId.FurnitureBoxLarge => HeadquartersResources.ItemNameFurnitureBoxLarge,
		UseItemId.Fuel => HeadquartersResources.ItemNameFuel,
		UseItemId.Ammo => HeadquartersResources.ItemNameAmmo,
		UseItemId.Steel => HeadquartersResources.ItemNameSteel,
		UseItemId.Bauxite => HeadquartersResources.ItemNameBauxite,
		UseItemId.FurnitureCoin => HeadquartersResources.ItemNameFurnitureCoin,
		UseItemId.DockKey => HeadquartersResources.ItemNameDockKey,
		UseItemId.RepairTeam => HeadquartersResources.ItemNameRepairTeam,
		UseItemId.RepairGoddess => HeadquartersResources.ItemNameRepairGoddess,
		UseItemId.FurnitureFairy => HeadquartersResources.ItemNameFurnitureFairy,
		UseItemId.PortExpensionSet => HeadquartersResources.ItemNamePortExpensionSet,
		UseItemId.MoraleFoodMamiya => HeadquartersResources.ItemNameMoraleFoodMamiya,
		UseItemId.MarriageRingAndPapers => HeadquartersResources.ItemNameMarriageRingAndPapers,
		UseItemId.ValentineChocolate => HeadquartersResources.ItemNameValentineChocolate,
		UseItemId.Medals => HeadquartersResources.ItemNameMedals,
		UseItemId.RemodelBlueprints => HeadquartersResources.ItemNameRemodelBlueprints,
		UseItemId.MoraleFoodIrako => HeadquartersResources.ItemNameMoraleFoodIrako,
		UseItemId.PresentBox => HeadquartersResources.ItemNamePresentBox,
		UseItemId.FirstClassMedal => HeadquartersResources.ItemNameFirstClassMedal,
		UseItemId.Hishimochi => HeadquartersResources.ItemNameHishimochi,
		UseItemId.HeadquartersPersonnel => HeadquartersResources.ItemNameHeadquartersPersonnel,
		UseItemId.ReinforcementExpansion => HeadquartersResources.ItemNameReinforcementExpansion,
		UseItemId.PrototypeFlightDeckCatapult => HeadquartersResources.ItemNamePrototypeFlightDeckCatapult,
		UseItemId.CombatRation => HeadquartersResources.ItemNameCombatRation,
		UseItemId.UnderwayReplenishment => HeadquartersResources.ItemNameUnderwayReplenishment,
		UseItemId.Saury => HeadquartersResources.ItemNameSaury,
		UseItemId.CannedSaury => HeadquartersResources.ItemNameCannedSaury,
		UseItemId.SkilledCrewMember => HeadquartersResources.ItemNameSkilledCrewMember,
		UseItemId.NeTypeEngine => HeadquartersResources.ItemNameNeTypeEngine,
		UseItemId.DecorationMaterial => HeadquartersResources.ItemNameDecorationMaterial,
		UseItemId.ConstructionBattalion => HeadquartersResources.ItemNameConstructionBattalion,
		UseItemId.NewModelAircraftBlueprint => HeadquartersResources.ItemNameNewModelAircraftBlueprint,
		UseItemId.NewModelArtilleryArmamentMaterials => HeadquartersResources.ItemNameNewModelArtilleryArmamentMaterials,
		UseItemId.CombatRationSpecialOnigiri => HeadquartersResources.ItemNameCombatRationSpecialOnigiri,
		UseItemId.NewModelAviationArmamentMaterials => HeadquartersResources.ItemNameNewModelAviationArmamentMaterials,
		UseItemId.ActionReport => HeadquartersResources.ItemNameActionReport,
		UseItemId.StraitMedal => HeadquartersResources.ItemNameStraitMedal,
		UseItemId.XmasSelectGiftBox => HeadquartersResources.ItemNameXmasSelectGiftBox,
		UseItemId.ShoGoMedalTei => HeadquartersResources.ItemNameShoGoMedal,
		UseItemId.ShoGoMedalHei => HeadquartersResources.ItemNameShoGoMedal,
		UseItemId.ShoGoMedalOtsu => HeadquartersResources.ItemNameShoGoMedal,
		UseItemId.ShoGoMedalKou => HeadquartersResources.ItemNameShoGoMedal,
		UseItemId.Rice => HeadquartersResources.Rice,
		UseItemId.Umeboshi => HeadquartersResources.Umeboshi,
		UseItemId.Nori => HeadquartersResources.Nori,
		UseItemId.Tea => HeadquartersResources.Tea,
		UseItemId.HoushouDinnerTicket => HeadquartersResources.ItemNameHoushouDinnerTicket,
		UseItemId.SetsubunBeans => HeadquartersResources.ItemNameSetsubunBeans,
		UseItemId.EmergencyRepairMaterial => HeadquartersResources.ItemNameEmergencyRepairMaterial,
		UseItemId.NewModelRocketDevelopmentMaterials => HeadquartersResources.ItemNameNewModelRocketDevelopmentMaterials,
		UseItemId.Sardine => HeadquartersResources.ItemNameSardine,
		UseItemId.NewModelArmamentMaterials => HeadquartersResources.ItemNameNewModelArmamentMaterials,
		UseItemId.SubmarineSupplyMaterials => HeadquartersResources.ItemNameSubmarineSupplyMaterials,
		UseItemId.Pumpkin => HeadquartersResources.Pumpkin,
		UseItemId.TeruTeruBozu => HeadquartersResources.TeruTeruBozu,
		UseItemId.SeaColoredRibbon => HeadquartersResources.SeaColoredRibbon,
		UseItemId.WhiteSash => HeadquartersResources.WhiteSash,
		_ => Name,
	};


	/// <summary>
	/// アイテム名
	/// </summary>
	public string Name => RawData.api_name;

	/// <summary>
	/// 説明
	/// </summary>
	public string Description => RawData.api_description[0];

	//description[1]=家具コインの内容量　省略します


	public int ID => (int)ItemID;

	public override string ToString() => $"[{ItemID}] {NameTranslated}";
}
