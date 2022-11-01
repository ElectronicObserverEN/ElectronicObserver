using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ElectronicObserver.Common;
using ElectronicObserver.Data;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Dialog.EquipmentPicker;

public partial class EquipmentPickerViewModel : WindowViewModelBase
{
	public EquipmentPickerTranslationViewModel Translations { get; set; } = new();

	public EquipmentFilterViewModel Filters { get; set; } = new();

	private List<IEquipmentData> AllEquipments => KCDatabase.Instance.Equipments
		.Values
		.Cast<IEquipmentData>()
		.ToList();

	public ObservableCollection<IEquipmentData> EquipmentsFiltered { get; set; } = new();

	public IEquipmentData? SelectedEquipment { get; set; }

	public EquipmentPickerViewModel()
	{
		RefreshList();
		Filters.PropertyChanged += Filters_PropertyChanged;
	}

	private void RefreshList() => 	
		EquipmentsFiltered = new(AllEquipments
			.Where(s => Filters.MeetsFilterCondition(s))
			.OrderBy(s => s.MasterEquipment.CategoryType)
			.ThenBy(s => s.MasterID));

	private void Filters_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		RefreshList();
	}
}
