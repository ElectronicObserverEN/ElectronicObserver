using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Serialization.EquipmentUpgrade;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.CostCalculation;

namespace ElectronicObserver.Utility.Data;

public static class EquipmentUpgradeCostExtensions
{
	public static EquipmentUpgradePlanCostModel CalculateUpgradeCost(this IEquipmentData equipment,
		List<EquipmentUpgradeDataModel> upgradesData, IShipDataMaster? helper, UpgradeLevel targetedLevel,
		SliderUpgradeLevel sliderLevel)
		=> equipment.CalculateUpgradeCost(upgradesData, helper, targetedLevel, sliderLevel, null);

	public static EquipmentUpgradePlanCostModel CalculateUpgradeCost(this IEquipmentData equipment,
		List<EquipmentUpgradeDataModel> upgradesData, IShipDataMaster? helper, UpgradeLevel targetedLevel,
		SliderUpgradeLevel sliderLevel, DayOfWeek? day)
	{
		EquipmentUpgradePlanCostModel cost = new();

		if (targetedLevel <= equipment.UpgradeLevel) return cost;

		EquipmentUpgradeDataModel? upgradeData =
			upgradesData.FirstOrDefault(data => data.EquipmentId == (int?)equipment.EquipmentId);

		if (upgradeData is null) return cost;

		EquipmentUpgradeImprovementModel? improvementModel =
			GetImprovementModelDependingOnHelperAndDay(upgradeData.Improvement, helper, day);

		if (improvementModel is null) return cost;

		List<UpgradeLevel> levels = Enum.GetValues<UpgradeLevel>().OrderBy(val => val).ToList();

		foreach (UpgradeLevel level in levels.SkipWhile(l => l <= equipment.UpgradeLevel))
		{
			bool useSlider = (int)level > (int)sliderLevel;
			cost += improvementModel.CalculateUpgradeLevelCost(level, useSlider);

			if (level == targetedLevel) return cost;
		}

		return cost;
	}

	public static EquipmentUpgradePlanCostModel CalculateNextUpgradeCost(this IEquipmentData equipment,
		List<EquipmentUpgradeDataModel> upgradesData, IShipDataMaster? helper, SliderUpgradeLevel sliderLevel)
		=> CalculateUpgradeCost(equipment, upgradesData, helper, equipment.UpgradeLevel.GetNextLevel(), sliderLevel);

	public static EquipmentUpgradePlanCostModel CalculateNextUpgradeCost(this IEquipmentData equipment,
		List<EquipmentUpgradeDataModel> upgradesData, IShipDataMaster? helper, SliderUpgradeLevel sliderLevel,
		DayOfWeek? day)
		=> CalculateUpgradeCost(equipment, upgradesData, helper, equipment.UpgradeLevel.GetNextLevel(), sliderLevel,
			day);

	public static EquipmentUpgradePlanCostModel CalculateUpgradeLevelCost(
		this EquipmentUpgradeImprovementModel improvementModel, UpgradeLevel level, bool useSlider)
	{
		EquipmentUpgradeImprovementCostDetail? costDetail =
			improvementModel.Costs.GetImprovementCostDetailFromLevel(level);

		// Shouldn't happen ...
		if (costDetail is null) return new EquipmentUpgradePlanCostModel();

		return new EquipmentUpgradePlanCostModel()
		{
			Ammo = improvementModel.Costs.Ammo,
			Fuel = improvementModel.Costs.Fuel,
			Steel = improvementModel.Costs.Steel,
			Bauxite = improvementModel.Costs.Bauxite,
			DevelopmentMaterial = useSlider ? costDetail.SliderDevmatCost : costDetail.DevmatCost,
			ImprovementMaterial = useSlider ? costDetail.SliderImproveMatCost : costDetail.ImproveMatCost,
			RequiredConsumables =
				costDetail.ConsumableDetail.Select(cons =>
					new EquipmentUpgradePlanCostItemModel() { Id = cons.Id, Required = cons.Count, }).ToList(),
			RequiredEquipments = costDetail.EquipmentDetail.Select(cons =>
				new EquipmentUpgradePlanCostItemModel() { Id = cons.Id, Required = cons.Count, }).ToList(),
		};
	}


	public static EquipmentUpgradeImprovementCostDetail? GetImprovementCostDetailFromLevel(
		this EquipmentUpgradeImprovementCost costDetail, UpgradeLevel level)
		=> level switch
		{
			UpgradeLevel.Conversion => costDetail.CostMax,
			> UpgradeLevel.Six => costDetail.Cost6To9,
			_ => costDetail.Cost0To5,
		};

	/// <summary>
	/// Return an improvement model depending on the flagship
	/// </summary>
	/// <param name="improvements"></param>
	/// <param name="helper"></param>
	/// <returns></returns>
	private static EquipmentUpgradeImprovementModel? GetImprovementModelDependingOnHelperAndDay(
		List<EquipmentUpgradeImprovementModel> improvements, IShipDataMaster? helper, DayOfWeek? day)
	{
		if (helper is null) return improvements.FirstOrDefault();

		return improvements.FirstOrDefault(imp => imp.Helpers.Any(helpers =>
			helpers.ShipIds.Contains(helper.ShipID) &&
			(day is not { } dayNotNull || helpers.CanHelpOnDays.Contains(dayNotNull))));
	}

	/// <summary>
	/// Return next level of the upgrade level
	/// </summary>
	/// <param name="currentLevel"></param>
	/// <returns></returns>
	public static UpgradeLevel GetNextLevel(this UpgradeLevel currentLevel)
	{
		if (currentLevel is UpgradeLevel.Max or UpgradeLevel.Conversion) return UpgradeLevel.Conversion;

		return ++currentLevel;
	}

	public static List<EquipmentUpgradeCostPerLevel> GetCostPerLevel(this EquipmentUpgradeImprovementCost costs)
	{
		List<UpgradeLevel> levels = Enum.GetValues<UpgradeLevel>()
			.Skip(1)
			.OrderBy(val => val)
			.ToList();

		List<EquipmentUpgradeCostPerLevel> result = [];

		foreach (UpgradeLevel level in levels)
		{
			EquipmentUpgradeImprovementCostDetail? detail = costs.GetImprovementCostDetailFromLevel(level);

			if (detail is { })
			{
				EquipmentUpgradeCostPerLevel levelCost = new(level, detail);

				ApplyExtraCost(costs, level, levelCost);

				result.Add(levelCost);
			}
		}

		return result;
	}

	private static void ApplyExtraCost(EquipmentUpgradeImprovementCost costs, UpgradeLevel level, EquipmentUpgradeCostPerLevel levelCost)
	{
		if (costs.ExtraCost is null) return;

		foreach (EquipmentUpgradeExtraCostModel extraCost in costs.ExtraCost.Where(cost => cost.Levels.Contains(level)))
		{
			foreach (EquipmentUpgradeImprovementCostItemDetail extraConsumable in extraCost.Consumables)
			{
				if (levelCost.ConsumableDetail.Find(consumable => consumable.Id == extraConsumable.Id) is
				    { } foundDetail)
				{
					foundDetail.Count += extraConsumable.Count;
				}
				else
				{
					levelCost.ConsumableDetail.Add(new()
					{
						Id = extraConsumable.Id,
						Count = extraConsumable.Count,
					});
				}
			}
		}
	}

	public static List<EquipmentUpgradeCostRange> GetCostPerLevelRange(this EquipmentUpgradeImprovementCost costs)
	{
		List<EquipmentUpgradeCostRange> result = [];

		if (costs.ExtraCost is null)
		{
			result.Add(new(new EquipmentUpgradeCostPerLevel(UpgradeLevel.One, costs.Cost0To5))
			{
				EndLevel = UpgradeLevel.Six,
			});

			result.Add(new(new EquipmentUpgradeCostPerLevel(UpgradeLevel.Seven, costs.Cost6To9))
			{
				EndLevel = UpgradeLevel.Max,
			});

			if (costs.CostMax is { })
			{
				result.Add(new(new EquipmentUpgradeCostPerLevel(UpgradeLevel.Conversion, costs.CostMax)));
			}

			return result;
		}

		EquipmentUpgradeCostPerLevel? previousCost = null;

		foreach (EquipmentUpgradeCostPerLevel cost in costs.GetCostPerLevel())
		{
			if (!cost.Equals(previousCost) || cost.UpgradeLevel is UpgradeLevel.Conversion)
			{
				result.Add(new(cost));
			}

			previousCost = cost;
		}

		for (int index = 0; index < result.Count; index++)
		{
			if (index < result.Count - 1)
			{
				EquipmentUpgradeCostRange range = result[index];
				EquipmentUpgradeCostRange nextRange = result[index + 1];

				range.EndLevel = nextRange.StartLevel switch
				{
					UpgradeLevel.Conversion => UpgradeLevel.Max,
					_ => nextRange.StartLevel - 1,
				};
			}
		}

		return result;
	}

}
