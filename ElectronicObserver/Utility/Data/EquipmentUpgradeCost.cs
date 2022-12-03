using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Serialization.EquipmentUpgrade;

namespace ElectronicObserver.Utility.Data;
public static class EquipmentUpgradeCost
{
	public static EquipmentUpgradePlanCostModel CalculateUpgradeCost(this IEquipmentData equipment, List<EquipmentUpgradeDataModel> upgradesData, IShipDataMaster? helper, UpgradeLevel targetedLevel, SliderUpgradeLevel sliderLevel)
	{
		EquipmentUpgradePlanCostModel cost = new EquipmentUpgradePlanCostModel();


		EquipmentUpgradeDataModel? upgradeData = upgradesData.FirstOrDefault(data => data.EquipmentId == (int?)equipment.EquipmentId);

		if (upgradeData is null) return cost;

		if (helper is null) return cost;
		EquipmentUpgradeImprovmentModel? improvmentModel = GetImprovmentModelDependingOnHelper(upgradeData.Improvement, helper);

		if (improvmentModel is null) return cost;

		UpgradeLevel level = equipment.UpgradeLevel;

		// 0 -> 10
		while ((level < UpgradeLevel.Max && targetedLevel == UpgradeLevel.Conversion) || (level < targetedLevel && targetedLevel != UpgradeLevel.Conversion))
		{
			level++;

			bool useSlider = sliderLevel != SliderUpgradeLevel.Conversion && sliderLevel != SliderUpgradeLevel.Never && (int)level >= (int)sliderLevel;

			cost += improvmentModel.CalculateUpgradeLevelCost(level, useSlider);
		}

		// Conversion
		if (targetedLevel == UpgradeLevel.Conversion)
		{
			cost += improvmentModel.CalculateUpgradeLevelCost(UpgradeLevel.Conversion, sliderLevel != SliderUpgradeLevel.Never);
		}

		return cost;
	}

	public static EquipmentUpgradePlanCostModel CalculateUpgradeLevelCost(this EquipmentUpgradeImprovmentModel improvmentModel, UpgradeLevel level, bool useSlider)
	{
		EquipmentUpgradeImprovmentCostDetail? costDetail = improvmentModel.Costs.GetImprovmentCostDetailFromLevel(level);

		// Shouldn't happen ...
		if (costDetail is null) return new EquipmentUpgradePlanCostModel();

		return new EquipmentUpgradePlanCostModel()
		{
			Ammo = improvmentModel.Costs.Ammo,
			Fuel = improvmentModel.Costs.Fuel,
			Steel = improvmentModel.Costs.Steel,
			Bauxite = improvmentModel.Costs.Bauxite,

			DevelopmentMaterial = useSlider ? costDetail.SliderDevmatCost : costDetail.DevmatCost,
			ImprovmentMaterial = useSlider ? costDetail.SliderImproveMatCost : costDetail.ImproveMatCost,

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


	public static EquipmentUpgradeImprovmentCostDetail? GetImprovmentCostDetailFromLevel(this EquipmentUpgradeImprovmentCost costDetail, UpgradeLevel level)
		=> level switch
		{
			UpgradeLevel.Conversion => costDetail.CostMax,
			> UpgradeLevel.Six => costDetail.Cost6To9,
			_ => costDetail.Cost0To5,
		};

	/// <summary>
	/// Return an improvment model depending on the flagship
	/// </summary>
	/// <param name="improvments"></param>
	/// <param name="helper"></param>
	/// <returns></returns>
	private static EquipmentUpgradeImprovmentModel? GetImprovmentModelDependingOnHelper(List<EquipmentUpgradeImprovmentModel> improvments, IShipDataMaster? helper)
	{
		if (helper is null) return improvments.FirstOrDefault();

		return improvments.FirstOrDefault(imp => imp.Helpers.SelectMany(helpers => helpers.ShipIds).ToList().Contains(helper.ShipID));
	}
}
