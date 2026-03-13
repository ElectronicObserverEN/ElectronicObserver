using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Serialization.EquipmentUpgrade;
using ElectronicObserver.Data;
using ElectronicObserver.Data.Translation;

namespace ElectronicObserver.Window.Tools.DialogAlbumMasterEquipment.EquipmentUpgrade;

public class AlbumMasterEquipmentUpgradeViewModel
{
	public int Fuel { get; set; }
	public int Ammo { get; set; }
	public int Steel { get; set; }
	public int Bauxite { get; set; }

	public List<AlbumMasterEquipmentUpgradeGroupViewModel> UpgradeViewModels { get; private set; } = [];
	private IEquipmentDataMaster Equipment { get; }

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
		EquipmentUpgradeDataModel? data = upgradeData.UpgradeList.FirstOrDefault(upgrade => upgrade.EquipmentId == Equipment.ID);

		if (data is null) return;

		EquipmentUpgradeImprovementModel? firstImprovement = data.Improvement.FirstOrDefault();

		if (firstImprovement is null) return;

		Fuel = firstImprovement.Costs.Fuel;
		Ammo = firstImprovement.Costs.Ammo;
		Steel = firstImprovement.Costs.Steel;
		Bauxite = firstImprovement.Costs.Bauxite;

		UpgradeViewModels = data.Improvement
			.GroupBy(improvement => improvement, new UpgradeCostDataEqualityComparer())
			.Select(group => new AlbumMasterEquipmentUpgradeGroupViewModel(group.ToList(), EquipmentUpgradeTranslation, Equipment))
			.ToList();
	}

	public void UnsubscribeFromApis()
	{
		UpgradeViewModels.ForEach(vm => vm.UnsubscribeFromApis());
	}
}
