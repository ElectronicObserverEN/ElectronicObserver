using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Serialization.EquipmentUpgrade;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.Helpers;

namespace ElectronicObserver.Window.Tools.DialogAlbumMasterEquipment.EquipmentUpgrade;

public class AlbumMasterEquipmentUpgradeGroupViewModel
{
	public IEquipmentDataMaster Equipment { get; }

	/// <summary>
	/// Equipment upgrade cost, its the first cost found for this equipment so it's accurate for fuel, ammo, ... and devmats/screws for 0 -> 9 upgrades
	/// </summary>
	public EquipmentUpgradeImprovementCost EquipmentUpgradeCost { get; private set; } = new();

	public List<AlbumMasterEquipmentUpgradeLevelViewModel> RequiredItemsPerLevel { get; set; } = [];

	public List<EquipmentUpgradeConversionViewModel> ConversionViewModel { get; private set; } = [];

	public List<EquipmentUpgradeHelpersViewModel> Helpers { get; private set; } = [];

	public AlbumMasterEquipmentUpgradeTranslationViewModel EquipmentUpgradeTranslation { get; }

	public List<EquipmentUpgradeImprovementModel> Models { get; }

	public AlbumMasterEquipmentUpgradeGroupViewModel(List<EquipmentUpgradeImprovementModel> improvements, AlbumMasterEquipmentUpgradeTranslationViewModel translations, IEquipmentDataMaster equipment)
	{
		Models = improvements;
		EquipmentUpgradeTranslation = translations;
		Equipment = equipment;

		LoadUpgradeData();
	}

	private void LoadUpgradeData()
	{
		EquipmentUpgradeImprovementModel? firstImprovement = Models.FirstOrDefault();

		if (firstImprovement is null) return;

		EquipmentUpgradeCost = firstImprovement.Costs;

		Helpers = Models
			.SelectMany(improvement => improvement.Helpers)
			.Select(helperGroup => new EquipmentUpgradeHelpersViewModel(helperGroup))
			.ToList();

		InitializeCostPerLevel();

		ConversionViewModel = Models
			.Where(improvement => improvement.ConversionData is not null)
			.Where(improvement => improvement.Costs.CostMax is not null)
			.Select(improvement => new EquipmentUpgradeConversionViewModel(improvement))
			.ToList();
	}

	private void InitializeCostPerLevel()
	{
		RequiredItemsPerLevel = EquipmentUpgradeCost
			.GetCostPerLevelRange()
			.Where(range => range.StartLevel != UpgradeLevel.Conversion)
			.Select(range => new AlbumMasterEquipmentUpgradeLevelViewModel(range))
			.ToList();
	}

	public void UnsubscribeFromApis()
	{
		ConversionViewModel.ForEach(viewModel => viewModel.UnsubscribeFromApis());
		Helpers.ForEach(viewModel => viewModel.UnsubscribeFromApis());
		RequiredItemsPerLevel.ForEach(viewModel => viewModel.UnsubscribeFromApis());
	}
}
