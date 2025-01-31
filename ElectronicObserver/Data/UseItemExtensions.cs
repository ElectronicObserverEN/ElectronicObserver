﻿using ElectronicObserverTypes;

namespace ElectronicObserver.Data;

public static class UseItemExtensions
{
    public static string NameTranslated(this UseItemId itemId) => itemId switch
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
	    UseItemId.NewModelArtilleryArmamentMaterials =>
		    HeadquartersResources.ItemNameNewModelArtilleryArmamentMaterials,
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
	    UseItemId.NewModelRocketDevelopmentMaterials =>
		    HeadquartersResources.ItemNameNewModelRocketDevelopmentMaterials,
	    UseItemId.Sardine => HeadquartersResources.ItemNameSardine,
	    UseItemId.NewModelArmamentMaterials => HeadquartersResources.ItemNameNewModelArmamentMaterials,
	    UseItemId.SubmarineSupplyMaterials => HeadquartersResources.ItemNameSubmarineSupplyMaterials,
	    UseItemId.Pumpkin => HeadquartersResources.Pumpkin,
	    UseItemId.TeruTeruBozu => HeadquartersResources.TeruTeruBozu,
	    UseItemId.SeaColoredRibbon => HeadquartersResources.SeaColoredRibbon,
	    UseItemId.WhiteSash => HeadquartersResources.WhiteSash,
	    UseItemId.LatestOverseasWarshipTechnology => HeadquartersResources.LatestOverseasWarshipTechnology,
	    UseItemId.NightSkilledCrewMember => HeadquartersResources.NightSkilledCrewMember,
	    UseItemId.SpecialAviationRation => HeadquartersResources.SpecialAviationRation,
	    _ => ConstantsRes.Unknown,
    };

}
