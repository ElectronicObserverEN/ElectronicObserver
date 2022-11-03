using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;
public class EquipmentUpgradePlanItemViewModel : ObservableObject
{
	public int? Id { get; set; }

	public IEquipmentData Equipment { get; set; }

	public UpgradeLevel DesiredUpgradeLevel { get; set; }

	public bool Finished { get; set; }

	public int Priority { get; set; }

	public EquipmentUpgradePlanItemViewModel(IEquipmentData equipment)
	{
		Equipment = equipment;
	}

}
