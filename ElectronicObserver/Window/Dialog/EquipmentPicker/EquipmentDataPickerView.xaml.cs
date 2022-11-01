using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Dialog.EquipmentPicker;
/// <summary>
/// Interaction logic for EquipmentPicker.xaml
/// </summary>
public partial class EquipmentDataPickerView
{
	public EquipmentDataPickerView(EquipmentDataPickerViewModel viewModel) : base(viewModel)
	{
		InitializeComponent();
	}

	public IEquipmentData? PickedEquipment { get; private set; }

	private void EventSetter_OnHandler(object sender, System.Windows.Input.MouseButtonEventArgs e)
	{
		PickedEquipment = ViewModel.SelectedEquipment;
		DialogResult = true;
	}
}
