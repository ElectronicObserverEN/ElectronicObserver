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

		ViewModel.SelectShipCommand.Execute(null);

		ViewModel.PropertyChanged += ViewModel_PropertyChanged;

		Closing += ShipPicker_Closing;
	}

	private void ShipPicker_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
	{
		ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
	}

	private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName is not nameof(ViewModel.PickedShip)) return;

		PickedShip = ViewModel.PickedShip;
		DialogResult = true;
	}


	public IShipData? PickedShip { get; private set; }
}
