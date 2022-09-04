using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Data;
using ElectronicObserver.ViewModels;
using ElectronicObserver.Window.Dialog.EquipmentPicker;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Mocks;
using ElectronicObserverTypes.Serialization.FitBonus;

namespace ElectronicObserver.Window.Tools.FitBonusViewer;

public partial class FitBonusEquipmentViewModel : ObservableObject
{
	[ObservableProperty]
	private IEquipmentData? selectedEquipment;

	private EquipmentPickerViewModel EquipmentPickerViewModel = new();

	public ObservableCollection<FitBonusResult> FitBonuses { get; set; } = new();

	public FitBonusEquipmentViewModel()
	{

	}

	public FitBonusEquipmentViewModel(IEquipmentData? equipment)
	{
		selectedEquipment = equipment;
	}


	[ICommand]
	private void OpenEquipmentPicker()
	{
		EquipmentPickerView equipmentPicker = new(EquipmentPickerViewModel);

		if (equipmentPicker.ShowDialog(App.Current.MainWindow) is true)
		{
			SelectedEquipment = equipmentPicker.PickedEquipment switch
			{
				null => null,
				_ => new EquipmentDataMock(KCDatabase.Instance.MasterEquipments[equipmentPicker.PickedEquipment.EquipmentID])
			};
		}
	}
}
