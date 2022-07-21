using System.Collections.Generic;
using ElectronicObserverTypes;
using System.Linq;
using ElectronicObserverTypes.Serialization.FitBonus;
using ElectronicObserverTypes.Extensions;

namespace ElectronicObserver.Utility.Data;

public static class EquipmentFitBonus
{

	public static FitBonusValue GetFitBonus(this IShipData ship, IList<FitBonusPerEquipment> bonusList) => 
		GetFitBonuses(ship, bonusList)
		.Aggregate(
			new FitBonusValue(),
			(bonusA, bonusB) => bonusA + bonusB,
			bonus => bonus
		);

	/// <summary>
	/// Keep in mind that accuracy bonus is included
	/// </summary>
	/// <param name="ship"></param>
	/// <returns></returns>
	public static List<FitBonusValue> GetFitBonuses(this IShipData ship, IList<FitBonusPerEquipment> bonusList)
	{ 
		IList<IEquipmentData> equipments = ship.AllSlotInstance.Where(eq => eq != null).ToList()!;

		List<EquipmentId> distinctEquipments = equipments
			.Select(equipment => equipment.MasterEquipment.EquipmentId)
			.Distinct()
			.ToList();

		List<EquipmentTypes> distinctEquipmentTypes = equipments
			.Select(equipment => equipment.MasterEquipment.CategoryType)
			.Distinct()
			.ToList();

		// Keep only the rules that can be applied depending on equip ids or type before cheking them one by one
		List<FitBonusPerEquipment> bonusThatCanBeApplied = bonusList
			.Where(fitCondition => 
				distinctEquipments.Any(equipment => fitCondition.EquipmentIds?.Contains(equipment) == true)
				||
				distinctEquipmentTypes.Any(equipment => fitCondition.EquipmentTypes?.Contains(equipment) == true)
			)
			.ToList();

		List<FitBonusValue> finalBonusList = new List<FitBonusValue>();

		foreach (FitBonusPerEquipment fitPerEquip in bonusThatCanBeApplied)
		{
			if (fitPerEquip.Bonuses != null)
			{
				foreach (FitBonusData fitData in fitPerEquip.Bonuses)
				{
					int bonusMultiplier = NumberOfTimeBonusApplies(fitPerEquip, fitData, ship.MasterShip, equipments);
					if (bonusMultiplier > 0)
					{
						if (fitData.Bonuses != null) finalBonusList.Add(bonusMultiplier > 1 ? (fitData.Bonuses * bonusMultiplier) : fitData.Bonuses);
						if (fitData.BonusesIfLOSRadar != null && ship.HasSurfaceRadar()) finalBonusList.Add(fitData.BonusesIfLOSRadar);
						if (fitData.BonusesIfAirRadar != null && ship.HasAirRadar(1)) finalBonusList.Add(fitData.BonusesIfAirRadar);
					}
				}
			}
		}

		return finalBonusList;
	}

	private static int NumberOfTimeBonusApplies(FitBonusPerEquipment fitPerEquip, FitBonusData fitBonusData, IShipDataMaster shipMaster, IList<IEquipmentData> equipments)
	{
		if (fitBonusData.ShipClasses != null && !fitBonusData.ShipClasses.Contains(shipMaster.ShipClassTyped)) return 0;
		if (fitBonusData.ShipMasterIds != null && !fitBonusData.ShipMasterIds.Contains(shipMaster.ShipId)) return 0;
		if (fitBonusData.ShipIds != null && !fitBonusData.ShipIds.Contains(shipMaster.BaseShip().ShipId)) return 0;
		if (fitBonusData.ShipTypes != null && !fitBonusData.ShipTypes.Contains(shipMaster.ShipType)) return 0;

		if (fitBonusData.EquipmentRequired != null)
		{
			int count = equipments.Count(eq => fitBonusData.EquipmentRequired.Contains(eq.EquipmentId));
			if ((fitBonusData.NumberOfEquipmentsRequired ?? 1) > count) return 0;
		}

		if (fitBonusData.EquipmentTypesRequired != null)
		{
			int count = equipments.Count(eq => fitBonusData.EquipmentTypesRequired.Contains(eq.MasterEquipment.CategoryType));
			if ((fitBonusData.NumberOfEquipmentTypesRequired ?? 1) > count) return 0;
		}

		List<IEquipmentData> equipmentsThatMatches = new List<IEquipmentData>();

		if (fitPerEquip.EquipmentIds != null) equipmentsThatMatches.AddRange(equipments.Where(eq => fitPerEquip.EquipmentIds.Contains(eq.EquipmentId)));
		if (fitPerEquip.EquipmentTypes != null) equipmentsThatMatches.AddRange(equipments.Where(eq => fitPerEquip.EquipmentTypes.Contains(eq.MasterEquipment.CategoryType)));
		if (fitBonusData.EquipmentLevel != null) equipmentsThatMatches = equipmentsThatMatches.Where(eq => eq.Level >= fitBonusData.EquipmentLevel).ToList();

		if (fitBonusData.NumberOfEquipmentsRequiredAfterOtherFilters != null && fitBonusData.NumberOfEquipmentsRequiredAfterOtherFilters > equipmentsThatMatches.Count) return 0;

		if (fitBonusData.NumberOfEquipmentsRequiredAfterOtherFilters != null || fitBonusData.EquipmentRequired != null || fitBonusData.EquipmentTypesRequired != null) return 1;
		return equipmentsThatMatches.Count;
	}
}
