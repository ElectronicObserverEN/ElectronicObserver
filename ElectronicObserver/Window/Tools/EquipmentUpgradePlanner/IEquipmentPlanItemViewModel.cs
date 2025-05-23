using System.Collections.Generic;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Data;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.CostCalculation;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;

public interface IEquipmentPlanItemViewModel
{
	public EquipmentId EquipmentMasterDataId { get; }

	public IEquipmentDataMaster? EquipmentMasterData => KCDatabase.Instance.MasterEquipments.ContainsKey((int)EquipmentMasterDataId) switch
	{
		true => KCDatabase.Instance.MasterEquipments[(int)EquipmentMasterDataId],
		_ => null,
	};

	public EquipmentUpgradePlanCostViewModel Cost { get; }

	public void UnsubscribeFromApis();

	public List<IEquipmentPlanItemViewModel> GetPlanChildren();
}
