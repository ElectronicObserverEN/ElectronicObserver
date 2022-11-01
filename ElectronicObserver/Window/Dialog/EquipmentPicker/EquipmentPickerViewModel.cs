using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Data;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;

namespace ElectronicObserver.Window.Dialog.EquipmentPicker;

public partial class EquipmentPickerViewModel : WindowViewModelBase
{
	public List<Filter> TypeFilters { get; }

	private List<EquipmentData> AllEquipments => KCDatabase.Instance.Equipments
		.Values
		.Cast<EquipmentData>()
		.OrderBy(s => s.MasterID)
		.ToList();

	public ObservableCollection<EquipmentGroup> EquipmentGroups { get; set; } = new();

	public EquipmentData? PickedEquipment { get; private set; }

	public EquipmentPickerViewModel()
	{
		TypeFilters = Enum.GetValues<EquipmentTypeGroup>().Select(t => new Filter(t)).ToList();

		foreach (Filter filter in TypeFilters)
		{
			filter.PropertyChanged += Filter_PropertyChanged;
		}
	}

	[ICommand]
	private void SelectEquipment(EquipmentData? equipment)
	{
		PickedEquipment = equipment;
	}

	private void Filter_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		List<EquipmentTypes> enabledFilters = TypeFilters
			.Where(f => f.IsChecked)
			.SelectMany(f => f.Value.ToTypes())
			.ToList();

		EquipmentGroups = new(AllEquipments
			.Where(s => enabledFilters.Contains(s.MasterEquipment.CategoryType))
			.GroupBy(s => s.MasterEquipment.CategoryType)
			.Select(g => new EquipmentGroup
			{
				Id = g.Key,
				Name = KCDatabase.Instance.EquipmentTypes[(int)g.Key].NameEN,
				Equipments = g.ToList(),
			}).ToList());
	}
}
