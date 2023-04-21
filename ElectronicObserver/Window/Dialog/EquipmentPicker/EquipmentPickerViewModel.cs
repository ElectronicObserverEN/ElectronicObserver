using System.Collections.Generic;
using ElectronicObserver.Common;
using ElectronicObserver.Common.Datagrid;
using ElectronicObserver.Window.Control.EquipmentFilter;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Dialog.EquipmentPicker;

public abstract partial class EquipmentPickerViewModel : WindowViewModelBase
{
	public DataGridViewModel<IEquipmentData> DataGridViewModel { get; set; }

	public EquipmentPickerTranslationViewModel Translations { get; set; } = new();

	public EquipmentFilterViewModel Filters { get; set; } = new();

	protected abstract List<IEquipmentData> AllEquipments { get; }

	public IEquipmentData? SelectedEquipment { get; set; }

	protected EquipmentPickerViewModel()
	{
		DataGridViewModel = new(new(AllEquipments));
		DataGridViewModel.FilterValue = Filters.MeetsFilterCondition;

		Filters.PropertyChanged += Filters_PropertyChanged;
	}

	private void RefreshList() =>
		DataGridViewModel.Items.Refresh();

	private void Filters_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		RefreshList();
	}
}
