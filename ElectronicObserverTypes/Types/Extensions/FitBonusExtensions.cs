using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Core.Types.Serialization.FitBonus;

namespace ElectronicObserver.Core.Types.Extensions;

public static class FitBonusExtensions
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

		foreach (IEquipmentDataMaster eq in ship.AllSlotInstanceMaster.OfType<IEquipmentDataMaster>())
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

		if (!ship.MasterShip.ASW.IsDetermined)
		{
			bonus.ASW = 0;
		}

		if (!ship.MasterShip.Evasion.IsDetermined)
		{
			bonus.Evasion = 0;
		}

		if (!ship.MasterShip.LOS.IsDetermined)
		{
			bonus.LOS = 0;
		}

		return bonus;
	}

	/// <summary>
	/// Keep in mind that accuracy bonus is included
	/// </summary>
	/// <param name="ship"></param>
	/// <param name="bonusList"></param>
	/// <returns></returns>
	public static FitBonusValue GetTheoreticalFitBonus(this IShipData ship, IList<FitBonusPerEquipment> bonusList)
		=> ship
			.GetTheoreticalFitBonusList(bonusList)
			.SelectMany(bonus => bonus)
			.Aggregate(new FitBonusValue(), (bonusA, bonusB) => bonusA + bonusB);

	/// <summary>
	/// Keep in mind that accuracy bonus is included
	/// </summary>
	/// <param name="ship"></param>
	/// <param name="bonusList"></param>
	/// <returns></returns>
	private static List<List<FitBonusValue>> GetTheoreticalFitBonusList(this IShipData ship, IList<FitBonusPerEquipment> bonusList)
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

		return GetTheoreticalFitBonusList(ship, fitBonusThatApplies, equipments);
	}

	private static List<List<FitBonusValue>> GetTheoreticalFitBonusList(IShipData ship, List<FitBonusPerEquipment> fitBonusThatApplies, IList<IEquipmentData> equipments)
	{
		List<List<FitBonusValue>> finalBonuses = new();

		foreach (FitBonusPerEquipment fitPerEquip in fitBonusThatApplies)
		{
			foreach (FitBonusData fitData in fitPerEquip.Bonuses)
			{
				List<FitBonusValue> result = GetFitBonusResultFromFitData(ship, equipments, fitPerEquip, fitData);

				finalBonuses.Add(result);
			}
		}

		return finalBonuses;
	}

	private static List<FitBonusValue> GetFitBonusResultFromFitData(IShipData ship, IList<IEquipmentData> equipments, FitBonusPerEquipment fitPerEquip,
		FitBonusData fitData)
	{
		List<FitBonusValue> fitBonusValues = [];

		int bonusMultiplier = NumberOfTimeBonusApplies(fitPerEquip, fitData, ship.MasterShip, equipments);

		if (bonusMultiplier <= 0) return fitBonusValues;

		if (fitData.Bonuses != null)
		{
			fitBonusValues.Add(bonusMultiplier switch
			{
				> 1 => fitData.Bonuses * bonusMultiplier,
				_ => fitData.Bonuses,
			});
		}

		if (fitData.BonusesIfSurfaceRadar != null && ship.HasSurfaceRadar())
		{
			fitBonusValues.Add(fitData.BonusesIfSurfaceRadar);
		}

		if (fitData.BonusesIfAccuracyRadar != null && ship.HasHighAccuracyRadar())
		{
			fitBonusValues.Add(fitData.BonusesIfAccuracyRadar);
		}

		if (fitData.BonusesIfAirRadar != null && ship.HasAirRadar())
		{
			fitBonusValues.Add(fitData.BonusesIfAirRadar);
		}

		return fitBonusValues;
	}

	private static int NumberOfTimeBonusApplies(FitBonusPerEquipment fitPerEquip, FitBonusData fitBonusData, IShipDataMaster shipMaster, IList<IEquipmentData> equipments)
	{
		if (!fitBonusData.AppliesTo(shipMaster)) return 0;

		if (fitBonusData.EquipmentRequired != null)
		{
			int count = GetRequiredEquipmentsFittingRequirements(fitBonusData, equipments).Count;

			if ((fitBonusData.NumberOfEquipmentsRequired ?? 1) > count) return 0;
		}

		if (fitBonusData.EquipmentTypesRequired != null)
		{
			int count = equipments.Count(eq => fitBonusData.EquipmentTypesRequired.Contains(eq.MasterEquipment.CategoryType));
			if ((fitBonusData.NumberOfEquipmentTypesRequired ?? 1) > count) return 0;
		}

		List<IEquipmentData> equipmentsThatMatches = GetEquipmentsFittingRequirements(fitPerEquip, fitBonusData, equipments);

		if (fitBonusData.NumberOfEquipmentsRequiredAfterOtherFilters != null && fitBonusData.NumberOfEquipmentsRequiredAfterOtherFilters > equipmentsThatMatches.Count) return 0;

		if (fitBonusData.NumberOfEquipmentsRequiredAfterOtherFilters != null || fitBonusData.EquipmentRequired != null || fitBonusData.EquipmentTypesRequired != null) return 1;

		return equipmentsThatMatches.Count;
	}

	private static bool AppliesTo(this FitBonusData fitBonusData, IShipDataMaster ship)
	{
		if (fitBonusData.ShipClasses != null && !fitBonusData.ShipClasses.Contains(ship.ShipClassTyped)) return false;
		if (fitBonusData.ShipMasterIds != null && !fitBonusData.ShipMasterIds.Contains(ship.ShipId)) return false;
		if (fitBonusData.ShipIds != null && !fitBonusData.ShipIds.Contains(ship.BaseShip().ShipId)) return false;
		if (fitBonusData.ShipTypes != null && !fitBonusData.ShipTypes.Contains(ship.ShipType)) return false;
		if (fitBonusData.ShipNationalities != null && !fitBonusData.ShipNationalities.Contains(ship.Nationality())) return false;

		return true;
	}

	private static List<IEquipmentData> GetEquipmentsFittingRequirements(FitBonusPerEquipment fitPerEquip, FitBonusData fitBonusData,
		IList<IEquipmentData> equipments)
	{
		List<IEquipmentData> equipmentsThatMatches = new();

		if (fitPerEquip.EquipmentIds != null)
		{
			equipmentsThatMatches.AddRange(equipments.Where(eq => fitPerEquip.EquipmentIds.Contains(eq.EquipmentId)));
		}

		if (fitPerEquip.EquipmentTypes != null)
		{
			equipmentsThatMatches.AddRange(equipments.Where(eq =>
				fitPerEquip.EquipmentTypes.Contains(eq.MasterEquipment.CategoryType)));
		}

		if (fitBonusData.EquipmentLevel != null)
		{
			equipmentsThatMatches = equipmentsThatMatches.Where(eq => eq.Level >= fitBonusData.EquipmentLevel).ToList();
		}

		return equipmentsThatMatches;
	}

	private static List<IEquipmentData> GetRequiredEquipmentsFittingRequirements(FitBonusData fitBonusData, IEnumerable<IEquipmentData> equipments)
	{
		if (fitBonusData.EquipmentRequired is null) return new();

		return equipments
			.Where(eq => fitBonusData.EquipmentRequired.Contains(eq.EquipmentId))
			.Where(eq => fitBonusData.EquipmentRequiresLevel is null || eq.UpgradeLevel >= fitBonusData.EquipmentRequiresLevel)
			.ToList();
	}
}
