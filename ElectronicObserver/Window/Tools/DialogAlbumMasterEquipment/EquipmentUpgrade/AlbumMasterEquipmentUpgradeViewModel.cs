using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Serialization.EquipmentUpgrade;
using ElectronicObserver.Data;
using ElectronicObserver.Data.Translation;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.Helpers;

namespace ElectronicObserver.Window.Tools.DialogAlbumMasterEquipment.EquipmentUpgrade;

public class AlbumMasterEquipmentUpgradeViewModel
{
	public EquipmentUpgradeDataModel? UpgradeData { get; private set; }
	public IEquipmentDataMaster Equipment { get; }

	public bool CanBeUpgraded => UpgradeData?.Improvement.FirstOrDefault() is not null;

	/// <summary>
	/// Equipment upgrade cost, its the first cost found for this equipment so it's accurate for fuel, ammo, ... and devmats/screws for 0 -> 9 upgrades
	/// </summary>
	public EquipmentUpgradeImprovementCost EquipmentUpgradeCost { get; private set; } = new();

	public List<AlbumMasterEquipmentUpgradeLevelViewModel> RequiredItemsPerLevel { get; set; } = [];

	public List<EquipmentUpgradeConversionViewModel> ConversionViewModel { get; private set; } = new();

	public List<EquipmentUpgradeHelpersViewModel> Helpers { get; private set; } = new();

	public AlbumMasterEquipmentUpgradeTranslationViewModel EquipmentUpgradeTranslation { get; }

	public AlbumMasterEquipmentUpgradeViewModel(IEquipmentDataMaster equipment)
	{
		EquipmentUpgradeTranslation = Ioc.Default.GetRequiredService<AlbumMasterEquipmentUpgradeTranslationViewModel>();

		Equipment = equipment;
		LoadUpgradeData();
	}

	private void LoadUpgradeData()
	{
		EquipmentUpgradeData upgradeData = KCDatabase.Instance.Translation.EquipmentUpgrade;
		UpgradeData = upgradeData.UpgradeList.FirstOrDefault(upgrade => upgrade.EquipmentId == Equipment.ID);

		if (UpgradeData is null) return;

		EquipmentUpgradeImprovementModel? firstImprovement = UpgradeData.Improvement.FirstOrDefault();

		if (firstImprovement is null) return;

		EquipmentUpgradeCost = firstImprovement.Costs;

		Helpers = UpgradeData.Improvement
			.SelectMany(improvement => improvement.Helpers)
			.Select(helperGroup => new EquipmentUpgradeHelpersViewModel(helperGroup))
			.ToList();

		InitializeCostPerLevel();

		ConversionViewModel = UpgradeData
			.Improvement
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
