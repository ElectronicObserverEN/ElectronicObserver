using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Serialization.EquipmentUpgrade;

namespace ElectronicObserver.Window.Tools.DialogAlbumMasterEquipment.EquipmentUpgrade;

public class AlbumMasterEquipmentUpgradeLevelViewModel
{
	public UpgradeLevel StartLevel { get; set; }
	public UpgradeLevel EndLevel { get; set; }

	public string UpgradeLevelsDisplay => StartLevel == EndLevel ? ((int)StartLevel).ToString() : $"{(int)StartLevel - 1} ～ {(int)EndLevel - 1}";

	public EquipmentUpgradeImprovementCostDetail EquipmentUpgradeCost { get; set; } = new();
	
	public EquipmentUpgradeItemCostViewModel? RequiredItems { get; set; }

	public EquipmentUpgradeCostRange Model { get; set; }

	public AlbumMasterEquipmentUpgradeLevelViewModel(EquipmentUpgradeCostRange model)
	{
		Model = model;

		LoadFromModel();
	}

	public void LoadFromModel()
	{
		EquipmentUpgradeCost = new EquipmentUpgradeImprovementCostDetail()
		{
			DevmatCost = Model.DevmatCost,
			SliderDevmatCost = Model.SliderDevmatCost,

			ImproveMatCost = Model.ImproveMatCost,
			SliderImproveMatCost = Model.SliderImproveMatCost,

			ConsumableDetail = Model.ConsumableDetail,
			EquipmentDetail = Model.EquipmentDetail,
		};

		RequiredItems = new(EquipmentUpgradeCost);

		StartLevel = Model.StartLevel;
		EndLevel = Model.EndLevel;
	}

	public void UnsubscribeFromApis()
	{
		RequiredItems?.UnsubscribeFromApis();
	}
}
