using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Wpf.Compass.ViewModels;

public class MasterShipSlotViewModel : ObservableObject
{
	public IEquipmentDataMaster? Equipment { get; set; }
	public int Size { get; set; }

	public string SizeString => Size switch
	{
		> 0 => $"{Size}",
		_ => "",
	};

	public EquipmentIconType EquipmentIcon => Equipment?.IconTypeTyped ?? EquipmentIconType.Nothing;
}
