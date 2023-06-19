using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserver.Data;
using ElectronicObserver.Data.Translation;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.CostCalculation;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.Helpers;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Serialization.EquipmentUpgrade;

namespace ElectronicObserver.Window.Tools.DialogAlbumMasterEquipment.EquipmentUpgrade;

public class AlbumMasterEquipmentUpgradeViewModel
{
	public EquipmentUpgradeDataModel? UpgradeData { get; private set; }
	private IEquipmentDataMaster Equipment { get; }

	/// <summary>
	/// Equipment upgrade cost, its the first cost found for this equipment so it's accurate for fuel, ammo, ... and devmats/screws for 0 -> 9 upgrades
	/// </summary>
	public EquipmentUpgradeImprovementCost EquipmentUpgradeCost { get; private set; } = new();
	
	public EquipmentUpgradeItemCostViewModel? RequiredItems0To5 { get; set; }
	public EquipmentUpgradeItemCostViewModel? RequiredItems6To9 { get; set; }

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

		EquipmentUpgradeCost = firstImprovement.Costs;

		Helpers = UpgradeData.Improvement
			.SelectMany(improvement => improvement.Helpers)
			.Select(helperGroup => new EquipmentUpgradeHelpersViewModel(helperGroup))
			.ToList();

		RequiredItems0To5 = new(EquipmentUpgradeCost.Cost0To5);
		RequiredItems6To9 = new(EquipmentUpgradeCost.Cost6To9);

		// ... 
	}
}
