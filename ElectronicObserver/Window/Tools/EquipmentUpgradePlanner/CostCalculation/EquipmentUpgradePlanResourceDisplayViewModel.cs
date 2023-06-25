using ElectronicObserver.Common;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.CostCalculation;

public abstract class EquipmentUpgradePlanResourceDisplayViewModel : CanBeUpdatedByApiViewModel
{
	public int Required { get; set; }

	public int Owned { get; set; }

	public bool EnoughOwned => Owned >= Required;

	protected EquipmentUpgradePlanResourceDisplayViewModel(bool shouldUpdate) : base(shouldUpdate)
	{
	}
}
