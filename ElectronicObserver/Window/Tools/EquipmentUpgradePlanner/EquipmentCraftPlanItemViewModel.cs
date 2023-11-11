using ElectronicObserver.Common;
using ElectronicObserver.Data;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.CostCalculation;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Mocks;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;
public class EquipmentCraftPlanItemViewModel : WindowViewModelBase, IEquipmentPlanItemViewModel
{
	public EquipmentId EquipmentMasterDataId { get; set; }

	public IEquipmentData? Equipment => KCDatabase.Instance.MasterEquipments.ContainsKey((int)EquipmentMasterDataId) switch
	{
		true => new EquipmentDataMock(KCDatabase.Instance.MasterEquipments[(int)EquipmentMasterDataId]),
		_ => null,
	};

	public EquipmentUpgradePlanCostViewModel Cost { get; set; } = new(new());

	public EquipmentCraftPlanItemViewModel(EquipmentId equipmentId)
	{
		EquipmentMasterDataId = equipmentId;
	}
}
