using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Services;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;

namespace ElectronicObserver.Window.Control.EquipmentFilter;

public partial class EquipmentFilterViewModel : ObservableObject
{
	public List<Filter> TypeFilters { get; }

	private TransliterationService TransliterationService { get; }

	public string? NameFilter { get; set; } = "";

	public EquipmentFilterTranslationViewModel EquipmentFilter { get; } = new();

	public EquipmentFilterViewModel() : this(false)
	{

	}

	public EquipmentFilterViewModel(bool typesCheckedByDefault)
	{
		TransliterationService = Ioc.Default.GetService<TransliterationService>()!;

		TypeFilters = Enum.GetValues<EquipmentTypeGroup>()
			.Select(t => new Filter(t)
			{
				IsChecked = typesCheckedByDefault,
			})
			.ToList();

		foreach (Filter filter in TypeFilters)
		{
			filter.PropertyChanged += (_, _) => OnPropertyChanged(string.Empty);
		}
	}

	public bool MeetsFilterCondition(IEquipmentData equipment)
		=> MeetsFilterCondition(equipment.MasterEquipment);

	public bool MeetsFilterCondition(IEquipmentDataMaster equipment)
	{
		List<EquipmentTypeGroup> enabledGroups = TypeFilters
			.Where(f => f.IsChecked)
			.Select(f => f.Value)
			.ToList();

		if (!enabledGroups.Contains(equipment.CategoryType.ToGroup())) return false;
		if (!string.IsNullOrEmpty(NameFilter) && !TransliterationService.Matches(equipment, NameFilter)) return false;

		return true;
	}

	[RelayCommand]
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
