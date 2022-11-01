using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Services;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;
namespace ElectronicObserver.Window.Dialog.EquipmentPicker;

public partial class EquipmentFilterViewModel : ObservableObject
{
	public List<Filter> TypeFilters { get; }

	private TransliterationService TransliterationService { get; }

	public string? NameFilter { get; set; } = "";

	public EquipmentFilterViewModel()
	{
		TransliterationService = Ioc.Default.GetService<TransliterationService>()!;

		TypeFilters = Enum.GetValues<EquipmentTypeGroup>()
			.Where(e => e != EquipmentTypeGroup.Unknown)
			.Select(t => new Filter(t)
			{
				IsChecked = false,
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
		if (!string.IsNullOrEmpty(NameFilter) && !TransliterationService.Matches(equipment.MasterEquipment, NameFilter)) return false;

		return true;
	}
}
