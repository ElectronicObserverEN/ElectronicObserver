using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserver.Data;
using ElectronicObserver.Data.Translation;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Serialization.EquipmentUpgrade;

namespace ElectronicObserver.Window.Tools.DialogAlbumMasterEquipment.EquipmentUpgrade;

public class AlbumMasterEquipmentUpgradeViewModel
{
	public EquipmentUpgradeDataModel? UpgradeData { get; private set; }
	private IEquipmentDataMaster Equipment { get; }

	public EquipmentUpgradeImprovementCost EquipmentUpgradeCost { get; } = new();

	public AlbumMasterEquipmentUpgradeViewModel(IEquipmentDataMaster equipment)
	{
		Equipment = equipment;
		LoadUpgradeData();
	}

	private void LoadUpgradeData()
	{
		EquipmentUpgradeData upgradeData = KCDatabase.Instance.Translation.EquipmentUpgrade;
		UpgradeData = upgradeData.UpgradeList.FirstOrDefault(upgrade => upgrade.EquipmentId == Equipment.ID);

		EquipmentUpgradeImprovementModel? firstImprovement = UpgradeData?.Improvement.FirstOrDefault();

		if (firstImprovement is null) return;

		EquipmentUpgradeCost.Fuel = firstImprovement.Costs.Fuel;
		EquipmentUpgradeCost.Ammo = firstImprovement.Costs.Ammo;
		EquipmentUpgradeCost.Steel = firstImprovement.Costs.Steel;
		EquipmentUpgradeCost.Bauxite = firstImprovement.Costs.Bauxite;

		// ... 
	}
}
