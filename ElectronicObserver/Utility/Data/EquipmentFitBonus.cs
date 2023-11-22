using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;
using ElectronicObserverTypes.Serialization.FitBonus;

namespace ElectronicObserver.Utility.Data;

public static class EquipmentFitBonus
{
	public static FitBonusValue GetFitBonus(this IShipData ship)
	{
		FitBonusValue bonus = new()
		{
			Firepower = ship.FirepowerTotal - ship.FirepowerBase,
			Torpedo = ship.TorpedoTotal - ship.TorpedoBase,
			AntiAir = ship.AATotal - ship.AABase,
			Armor = ship.ArmorTotal - ship.ArmorBase,
			ASW = ship.ASWTotal - (ship.MasterShip.ASW.GetEstParameterMin(ship.Level) + ship.ASWModernized),
			Evasion = ship.EvasionTotal - ship.MasterShip.Evasion.GetEstParameterMin(ship.Level),
			LOS = ship.LOSTotal - ship.MasterShip.LOS.GetEstParameterMin(ship.Level),
			Range = ship.MasterShip.Range,
		};

		foreach (IEquipmentDataMaster eq in ship.AllSlotInstanceMaster.Where(eq => eq != null))
		{
			bonus.Firepower -= eq.Firepower;
			bonus.Torpedo -= eq.Torpedo;
			bonus.AntiAir -= eq.AA;
			bonus.Armor -= eq.Armor;
			bonus.ASW -= eq.ASW;
			bonus.Evasion -= eq.Evasion;
			bonus.LOS -= eq.LOS;
			bonus.Range = Math.Max(bonus.Range, eq.Range);
		}

		bonus.Range = ship.Range - bonus.Range;

		if (!ship.MasterShip.ASW.IsDetermined) bonus.ASW = 0;
		if (!ship.MasterShip.Evasion.IsDetermined) bonus.Evasion = 0;
		if (!ship.MasterShip.LOS.IsDetermined) bonus.LOS = 0;

		return bonus;
	}

	public static FitBonusValue GetTheoricalFitBonus(this IShipData ship, IList<FitBonusPerEquipment> bonusList) =>
		ship.GetTheoricalFitBonuses(bonusList)
		.Aggregate(
			new FitBonusValue(),
			(bonusA, bonusB) => bonusA + bonusB,
			bonus => bonus
		);


	/// <summary>
	/// Keep in mind that accuracy bonus is included
	/// </summary>
	/// <param name="ship"></param>
	/// <param name="bonusList"></param>
	/// <returns></returns>
	public static List<FitBonusValue> GetTheoricalFitBonuses(this IShipData ship, IList<FitBonusPerEquipment> bonusList)
		=> ship.GetTheoricalFitBonusList(bonusList).Select(bonus => bonus.FinalBonus).ToList();

	/// <summary>
	/// Keep in mind that accuracy bonus is included
	/// </summary>
	/// <param name="ship"></param>
	/// <param name="bonusList"></param>
	/// <returns></returns>
	public static List<FitBonusResult> GetTheoricalFitBonusList(this IShipData ship, IList<FitBonusPerEquipment> bonusList)
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
		List<FitBonusPerEquipment> fitBonusThatApplies = bonusList
			.Where(fitCondition =>
				distinctEquipments.Any(equipment => fitCondition.EquipmentIds?.Contains(equipment) == true)
				||
				distinctEquipmentTypes.Any(equipment => fitCondition.EquipmentTypes?.Contains(equipment) == true)
			)
			.ToList();

		return GetTheoricalFitBonusList(ship, fitBonusThatApplies, equipments);
	}

	private static List<FitBonusResult> GetTheoricalFitBonusList(IShipData ship, List<FitBonusPerEquipment> fitBonusThatApplies, IList<IEquipmentData> equipments)
	{
		List<FitBonusResult> finalBonuses = new();

		foreach (FitBonusPerEquipment fitPerEquip in fitBonusThatApplies)
		{
			foreach (FitBonusData fitData in fitPerEquip.Bonuses)
			{
				FitBonusResult? result = GetFitBonusResultFromFitData(ship, equipments, fitPerEquip, fitData);
				if (result is not null) finalBonuses.Add(result);
			}
		}

		return finalBonuses;
	}

	private static FitBonusResult? GetFitBonusResultFromFitData(IShipData ship, IList<IEquipmentData> equipments, FitBonusPerEquipment fitPerEquip,
		FitBonusData fitData)
	{
		FitBonusResult result = new();
		if (fitPerEquip.EquipmentTypes != null) result.EquipmentTypes.AddRange(fitPerEquip.EquipmentTypes);
		if (fitPerEquip.EquipmentIds != null) result.EquipmentIds.AddRange(fitPerEquip.EquipmentIds);

		int bonusMultiplier = NumberOfTimeBonusApplies(fitPerEquip, fitData, ship.MasterShip, equipments);

		if (bonusMultiplier <= 0) return null;

		result.FitBonusData = fitData;
		if (fitData.Bonuses != null)
			result.FitBonusValues.Add(bonusMultiplier > 1
				? fitData.Bonuses * bonusMultiplier
				: fitData.Bonuses);
		if (fitData.BonusesIfSurfaceRadar != null && ship.HasSurfaceRadar())
			result.FitBonusValues.Add(fitData.BonusesIfSurfaceRadar);
		if (fitData.BonusesIfAirRadar != null && ship.HasAirRadar(1))
			result.FitBonusValues.Add(fitData.BonusesIfAirRadar);

		return result;
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

		List<IEquipmentData> equipmentsThatMatches = new();

		if (fitPerEquip.EquipmentIds != null) equipmentsThatMatches.AddRange(equipments.Where(eq => fitPerEquip.EquipmentIds.Contains(eq.EquipmentId)));
		if (fitPerEquip.EquipmentTypes != null) equipmentsThatMatches.AddRange(equipments.Where(eq => fitPerEquip.EquipmentTypes.Contains(eq.MasterEquipment.CategoryType)));
		if (fitBonusData.EquipmentLevel != null) equipmentsThatMatches = equipmentsThatMatches.Where(eq => eq.Level >= fitBonusData.EquipmentLevel).ToList();

		if (fitBonusData.NumberOfEquipmentsRequiredAfterOtherFilters != null && fitBonusData.NumberOfEquipmentsRequiredAfterOtherFilters > equipmentsThatMatches.Count) return 0;

		if (fitBonusData.NumberOfEquipmentsRequiredAfterOtherFilters != null || fitBonusData.EquipmentRequired != null || fitBonusData.EquipmentTypesRequired != null) return 1;
		return equipmentsThatMatches.Count;
	}
}
