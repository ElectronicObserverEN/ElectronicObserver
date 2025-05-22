using System.Windows.Controls;
using ElectronicObserver.Core.Types;

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
		if (sender is DataGridRow row && row.DataContext is IEquipmentData equipment)
		{
			PickedEquipment = equipment.MasterEquipment;
			DialogResult = true;
		}
	}
}
