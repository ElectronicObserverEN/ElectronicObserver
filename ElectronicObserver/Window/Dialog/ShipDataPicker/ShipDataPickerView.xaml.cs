namespace ElectronicObserver.Window.Dialog.ShipDataPicker;
/// <summary>
/// Interaction logic for ShipPickerView.xaml
/// </summary>
public partial class ShipDataPickerView
{
	public ShipDataPickerView(ShipDataPickerViewModel viewModel) : base(viewModel)
	{
		InitializeComponent();

		viewModel.PropertyChanged += ViewModel_PropertyChanged;
	}

	private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(ShipDataPickerViewModel.PickedShip)) DialogResult = true;
	}
}
