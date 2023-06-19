using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserver.Data;
using ElectronicObserver.Data.Translation;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.Helpers;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Serialization.EquipmentUpgrade;

namespace ElectronicObserver.Window.Tools.DialogAlbumMasterEquipment.EquipmentUpgrade;

public class AlbumMasterEquipmentUpgradeViewModel
{
	public EquipmentUpgradeDataModel? UpgradeData { get; private set; }
	private IEquipmentDataMaster Equipment { get; }

	public EquipmentUpgradeImprovementCost EquipmentUpgradeCost { get; } = new();

	public List<EquipmentUpgradeHelpersViewModel> Helpers { get; private set; } = new();

	public AlbumMasterEquipmentUpgradeViewModel(IEquipmentDataMaster equipment)
	{
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

		EquipmentUpgradeCost.Fuel = firstImprovement.Costs.Fuel;
		EquipmentUpgradeCost.Ammo = firstImprovement.Costs.Ammo;
		EquipmentUpgradeCost.Steel = firstImprovement.Costs.Steel;
		EquipmentUpgradeCost.Bauxite = firstImprovement.Costs.Bauxite;

		Helpers = UpgradeData.Improvement
			.SelectMany(improvement => improvement.Helpers)
			.Select(helperGroup => new EquipmentUpgradeHelpersViewModel(helperGroup))
			.ToList();

		// ... 
	}
}
