using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Serialization.EquipmentUpgrade;

namespace ElectronicObserver.Utility.Data;
public static class EquipmentUpgradeCostExtensions
{
	public static EquipmentUpgradePlanCostModel CalculateUpgradeCost(this IEquipmentData equipment, List<EquipmentUpgradeDataModel> upgradesData, IShipDataMaster? helper, UpgradeLevel targetedLevel, SliderUpgradeLevel sliderLevel)
	{
		EquipmentUpgradePlanCostModel cost = new();


		EquipmentUpgradeDataModel? upgradeData = upgradesData.FirstOrDefault(data => data.EquipmentId == (int?)equipment.EquipmentId);

		if (upgradeData is null) return cost;

		EquipmentUpgradeImprovementModel? improvementModel = GetImprovementModelDependingOnHelper(upgradeData.Improvement, helper);

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

	public static EquipmentUpgradePlanCostModel CalculateUpgradeLevelCost(this EquipmentUpgradeImprovementModel improvementModel, UpgradeLevel level, bool useSlider)
	{
		EquipmentUpgradeImprovementCostDetail? costDetail = improvementModel.Costs.GetImprovementCostDetailFromLevel(level);

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

			RequiredConsumables = costDetail.ConsumableDetail.Select(cons => new EquipmentUpgradePlanCostItemModel()
			{
				Id = cons.Id,
				Required = cons.Count,
			}).ToList(),

			RequiredEquipments = costDetail.EquipmentDetail.Select(cons => new EquipmentUpgradePlanCostItemModel()
			{
				Id = cons.Id,
				Required = cons.Count,
			}).ToList(),
		};
	}


	public static EquipmentUpgradeImprovementCostDetail? GetImprovementCostDetailFromLevel(this EquipmentUpgradeImprovementCost costDetail, UpgradeLevel level)
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
	private static EquipmentUpgradeImprovementModel? GetImprovementModelDependingOnHelper(List<EquipmentUpgradeImprovementModel> improvements, IShipDataMaster? helper)
	{
		if (helper is null) return improvements.FirstOrDefault();

		return improvements.FirstOrDefault(imp => imp.Helpers.SelectMany(helpers => helpers.ShipIds).ToList().Contains(helper.ShipID));
	}
}
