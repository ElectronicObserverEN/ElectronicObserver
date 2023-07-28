using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;

namespace ElectronicObserver.Window.Control.EquipmentFilter;

public class Filter : ObservableObject
{
	public EquipmentTypeGroup Value { get; set; }
	public string Display => Value.Display();
	public bool IsChecked { get; set; }

	public Filter(EquipmentTypeGroup value)
	{
		Value = value;
	}
}
