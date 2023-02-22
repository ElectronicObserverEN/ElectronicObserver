using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ElectronicObserver.Common;
using ElectronicObserver.Common.Datagrid;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Dialog.EquipmentPicker;

public abstract partial class EquipmentPickerViewModel : WindowViewModelBase
{
	public DataGridViewModel DataGridViewModel { get; set; } = new();

	public EquipmentPickerTranslationViewModel Translations { get; set; } = new();

	public EquipmentFilterViewModel Filters { get; set; } = new();

	protected abstract List<IEquipmentData> AllEquipments { get; }

	public ObservableCollection<IEquipmentData> EquipmentsFiltered { get; set; } = new();

	public IEquipmentData? SelectedEquipment { get; set; }

	protected EquipmentPickerViewModel()
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
