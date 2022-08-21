using ElectronicObserver.Window.Tools.DropRecordViewer;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Dialog.EquipmentPicker;
/// <summary>
/// Interaction logic for EquipmentPicker.xaml
/// </summary>
public partial class EquipmentPickerView
{
	public EquipmentPickerView(EquipmentPickerViewModel viewModel) : base(viewModel)
	{
		InitializeComponent();

		ViewModel.SelectEquipmentCommand.Execute(null);

		ViewModel.PropertyChanged += ViewModel_PropertyChanged;

		Closing += EquipmentPicker_Closing;
	}

	private void EquipmentPicker_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
	{
		ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
	}

	private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName is not nameof(ViewModel.PickedEquipment)) return;

		PickedEquipment = ViewModel.PickedEquipment;
		DialogResult = true;
	}

	public IEquipmentDataMaster? PickedEquipment { get; private set; }
}
