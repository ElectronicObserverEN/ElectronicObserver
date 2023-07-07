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
		UseItemId.InstantRepair => Properties.Window.FormHeadQuarters.ItemNameInstantRepair,
		UseItemId.InstantConstruction => Properties.Window.FormHeadQuarters.ItemNameInstantConstruction,
		UseItemId.DevelopmentMaterial => Properties.Window.FormHeadQuarters.ItemNameDevelopmentMaterial,
		UseItemId.ImproveMaterial => Properties.Window.FormHeadQuarters.ItemNameImproveMaterial,
		UseItemId.FurnitureBoxSmall => Properties.Window.FormHeadQuarters.ItemNameFurnitureBoxSmall,
		UseItemId.FurnitureBoxMedium => Properties.Window.FormHeadQuarters.ItemNameFurnitureBoxMedium,
		UseItemId.FurnitureBoxLarge => Properties.Window.FormHeadQuarters.ItemNameFurnitureBoxLarge,
		UseItemId.Fuel => Properties.Window.FormHeadQuarters.ItemNameFuel,
		UseItemId.Ammo => Properties.Window.FormHeadQuarters.ItemNameAmmo,
		UseItemId.Steel => Properties.Window.FormHeadQuarters.ItemNameSteel,
		UseItemId.Bauxite => Properties.Window.FormHeadQuarters.ItemNameBauxite,
		UseItemId.FurnitureCoin => Properties.Window.FormHeadQuarters.ItemNameFurnitureCoin,
		UseItemId.DockKey => Properties.Window.FormHeadQuarters.ItemNameDockKey,
		UseItemId.RepairTeam => Properties.Window.FormHeadQuarters.ItemNameRepairTeam,
		UseItemId.RepairGoddess => Properties.Window.FormHeadQuarters.ItemNameRepairGoddess,
		UseItemId.FurnitureFairy => Properties.Window.FormHeadQuarters.ItemNameFurnitureFairy,
		UseItemId.PortExpensionSet => Properties.Window.FormHeadQuarters.ItemNamePortExpensionSet,
		UseItemId.MoraleFoodMamiya => Properties.Window.FormHeadQuarters.ItemNameMoraleFoodMamiya,
		UseItemId.MarriageRingAndPapers => Properties.Window.FormHeadQuarters.ItemNameMarriageRingAndPapers,
		UseItemId.ValentineChocolate => Properties.Window.FormHeadQuarters.ItemNameValentineChocolate,
		UseItemId.Medals => Properties.Window.FormHeadQuarters.ItemNameMedals,
		UseItemId.RemodelBlueprints => Properties.Window.FormHeadQuarters.ItemNameRemodelBlueprints,
		UseItemId.MoraleFoodIrako => Properties.Window.FormHeadQuarters.ItemNameMoraleFoodIrako,
		UseItemId.PresentBox => Properties.Window.FormHeadQuarters.ItemNamePresentBox,
		UseItemId.FirstClassMedal => Properties.Window.FormHeadQuarters.ItemNameFirstClassMedal,
		UseItemId.Hishimochi => Properties.Window.FormHeadQuarters.ItemNameHishimochi,
		UseItemId.HeadquartersPersonnel => Properties.Window.FormHeadQuarters.ItemNameHeadquartersPersonnel,
		UseItemId.ReinforcementExpansion => Properties.Window.FormHeadQuarters.ItemNameReinforcementExpansion,
		UseItemId.PrototypeFlightDeckCatapult => Properties.Window.FormHeadQuarters.ItemNamePrototypeFlightDeckCatapult,
		UseItemId.CombatRation => Properties.Window.FormHeadQuarters.ItemNameCombatRation,
		UseItemId.UnderwayReplenishment => Properties.Window.FormHeadQuarters.ItemNameUnderwayReplenishment,
		UseItemId.Saury => Properties.Window.FormHeadQuarters.ItemNameSaury,
		UseItemId.CannedSaury => Properties.Window.FormHeadQuarters.ItemNameCannedSaury,
		UseItemId.SkilledCrewMember => Properties.Window.FormHeadQuarters.ItemNameSkilledCrewMember,
		UseItemId.NeTypeEngine => Properties.Window.FormHeadQuarters.ItemNameNeTypeEngine,
		UseItemId.DecorationMaterial => Properties.Window.FormHeadQuarters.ItemNameDecorationMaterial,
		UseItemId.ConstructionBattalion => Properties.Window.FormHeadQuarters.ItemNameConstructionBattalion,
		UseItemId.NewModelAircraftBlueprint => Properties.Window.FormHeadQuarters.ItemNameNewModelAircraftBlueprint,
		UseItemId.NewModelArtilleryArmamentMaterials => Properties.Window.FormHeadQuarters.ItemNameNewModelArtilleryArmamentMaterials,
		UseItemId.CombatRationSpecialOnigiri => Properties.Window.FormHeadQuarters.ItemNameCombatRationSpecialOnigiri,
		UseItemId.NewModelAviationArmamentMaterials => Properties.Window.FormHeadQuarters.ItemNameNewModelAviationArmamentMaterials,
		UseItemId.ActionReport => Properties.Window.FormHeadQuarters.ItemNameActionReport,
		UseItemId.StraitMedal => Properties.Window.FormHeadQuarters.ItemNameStraitMedal,
		UseItemId.XmasSelectGiftBox => Properties.Window.FormHeadQuarters.ItemNameXmasSelectGiftBox,
		UseItemId.ShoGoMedalTei => Properties.Window.FormHeadQuarters.ItemNameShoGoMedal,
		UseItemId.ShoGoMedalHei => Properties.Window.FormHeadQuarters.ItemNameShoGoMedal,
		UseItemId.ShoGoMedalOtsu => Properties.Window.FormHeadQuarters.ItemNameShoGoMedal,
		UseItemId.ShoGoMedalKou => Properties.Window.FormHeadQuarters.ItemNameShoGoMedal,
		UseItemId.Rice => Properties.Window.FormHeadQuarters.Rice,
		UseItemId.Umeboshi => Properties.Window.FormHeadQuarters.Umeboshi,
		UseItemId.Nori => Properties.Window.FormHeadQuarters.Nori,
		UseItemId.Tea => Properties.Window.FormHeadQuarters.Tea,
		UseItemId.HoushouDinnerTicket => Properties.Window.FormHeadQuarters.ItemNameHoushouDinnerTicket,
		UseItemId.SetsubunBeans => Properties.Window.FormHeadQuarters.ItemNameSetsubunBeans,
		UseItemId.EmergencyRepairMaterial => Properties.Window.FormHeadQuarters.ItemNameEmergencyRepairMaterial,
		UseItemId.NewModelRocketDevelopmentMaterials => Properties.Window.FormHeadQuarters.ItemNameNewModelRocketDevelopmentMaterials,
		UseItemId.Sardine => Properties.Window.FormHeadQuarters.ItemNameSardine,
		UseItemId.NewModelArmamentMaterials => Properties.Window.FormHeadQuarters.ItemNameNewModelArmamentMaterials,
		UseItemId.SubmarineSupplyMaterials => Properties.Window.FormHeadQuarters.ItemNameSubmarineSupplyMaterials,
		UseItemId.Pumpkin => Properties.Window.FormHeadQuarters.Pumpkin,
		UseItemId.TeruTeruBozu => Properties.Window.FormHeadQuarters.TeruTeruBozu,
		UseItemId.SeaColoredRibon => Properties.Window.FormHeadQuarters.SeaColoredRibon,
		UseItemId.WhiteSash => Properties.Window.FormHeadQuarters.WhiteSash,
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
