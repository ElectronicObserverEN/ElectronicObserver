using System.Collections.Generic;

namespace ElectronicObserver.Core.Types.Serialization.EquipmentUpgrade;

public class EquipmentUpgradeCostRange(EquipmentUpgradeCostPerLevel baseDetail)
{
	public UpgradeLevel StartLevel { get; set; } = baseDetail.UpgradeLevel;
	public UpgradeLevel EndLevel { get; set; } = baseDetail.UpgradeLevel;

	public int DevmatCost => baseDetail.DevmatCost;

	public int SliderDevmatCost => baseDetail.SliderDevmatCost;

	public int ImproveMatCost => baseDetail.ImproveMatCost;

	public int SliderImproveMatCost => baseDetail.SliderImproveMatCost;

	public List<EquipmentUpgradeImprovementCostItemDetail> EquipmentDetail => baseDetail.EquipmentDetail;

	public List<EquipmentUpgradeImprovementCostItemDetail> ConsumableDetail => baseDetail.ConsumableDetail;
}
