using ElectronicObserver.Data;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.CostCalculation;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;

public interface IEquipmentPlanItemViewModel
{
	public EquipmentId EquipmentMasterDataId { get; set; }

	public IEquipmentDataMaster? EquipmentMasterData => KCDatabase.Instance.MasterEquipments.ContainsKey((int)EquipmentMasterDataId) switch
	{
		true => KCDatabase.Instance.MasterEquipments[(int)EquipmentMasterDataId],
		_ => null,
	};

	public EquipmentUpgradePlanCostViewModel Cost { get; }
}
