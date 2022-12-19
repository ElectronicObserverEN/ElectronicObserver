using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.ResourceDisplay;

public abstract class EquipmentUpgradePlanResourceDisplayViewModel : ObservableObject
{
	public int Required { get; set; }

	public int Owned { get; set; }

	public bool EnoughOwned => Owned >= Required;
}
