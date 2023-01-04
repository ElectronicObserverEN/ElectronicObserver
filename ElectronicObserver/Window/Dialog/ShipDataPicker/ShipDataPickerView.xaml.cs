using System.Windows.Controls;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Dialog.ShipDataPicker;
/// <summary>
/// Interaction logic for ShipPickerView.xaml
/// </summary>
public partial class ShipDataPickerView
{
	public ShipDataPickerView(ShipDataPickerViewModel viewModel) : base(viewModel)
	{
		InitializeComponent();
	}

	private void EventSetter_OnHandler(object sender, System.Windows.Input.MouseButtonEventArgs e)
	{
		if (sender is DataGridRow row && row.DataContext is ShipDataViewModel ship)
		{
			PickedShip = ship.Ship;
			DialogResult = true;
		}
	}

	public IShipData? PickedShip { get; private set; }
}
