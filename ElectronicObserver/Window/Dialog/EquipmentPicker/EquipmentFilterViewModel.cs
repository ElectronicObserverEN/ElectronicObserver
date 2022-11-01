using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;
namespace ElectronicObserver.Window.Dialog.EquipmentPicker;

public partial class EquipmentFilterViewModel : ObservableObject
{
	public List<Filter> TypeFilters { get; }

	public string? NameFilter { get; set; } = "";

	public EquipmentFilterViewModel()
	{
		TypeFilters = Enum.GetValues<EquipmentTypeGroup>()
			.Select(t => new Filter(t)
			{
				IsChecked = true,
			})
			.ToList();

		foreach (Filter filter in TypeFilters)
		{
			filter.PropertyChanged += (_, _) => OnPropertyChanged(string.Empty);
		}
	}

	public bool MeetsFilterCondition(IEquipmentData equipment)
	{
		List<EquipmentTypes> enabledFilters = TypeFilters
			.Where(f => f.IsChecked)
			.SelectMany(f => f.Value.ToTypes())
			.ToList();

		if (!enabledFilters.Contains(equipment.MasterEquipment.CategoryType)) return false;
		if (!string.IsNullOrEmpty(NameFilter) && !equipment.MasterEquipment.NameEN.ToUpper().Contains(NameFilter.ToUpper())) return false;

		return true;
	}

	[ICommand]
	private void ToggleEquipmentTypes()
	{
		if (TypeFilters.All(f => f.IsChecked))
		{
			foreach (Filter typeFilter in TypeFilters)
			{
				typeFilter.IsChecked = false;
			}
		}
		else
		{
			foreach (Filter typeFilter in TypeFilters)
			{
				typeFilter.IsChecked = true;
			}
		}
	}
}
