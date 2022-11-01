using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Dialog.EquipmentPicker;
/// <summary>
/// Interaction logic for EquipmentPicker.xaml
/// </summary>
public partial class MasterEquipmentPickerView
{
	public MasterEquipmentPickerView(MasterEquipmentPickerViewModel viewModel) : base(viewModel)
	{
		InitializeComponent();
	}

	public IEquipmentDataMaster? PickedEquipment { get; private set; }

	private void EventSetter_OnHandler(object sender, System.Windows.Input.MouseButtonEventArgs e)
	{
		PickedEquipment = ViewModel.SelectedEquipment?.MasterEquipment;
		DialogResult = true;
	}
}
