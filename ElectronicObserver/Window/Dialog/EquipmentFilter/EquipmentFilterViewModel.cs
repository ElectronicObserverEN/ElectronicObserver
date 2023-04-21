﻿using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Services;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;

namespace ElectronicObserver.Window.Dialog.EquipmentFilter;

public partial class EquipmentFilterViewModel : ObservableObject
{
	public List<Filter> TypeFilters { get; }

	private TransliterationService TransliterationService { get; }

	public string? NameFilter { get; set; } = "";

	public EquipmentFilterViewModel() : this (false)
	{

	}

	public EquipmentFilterViewModel(bool typesCheckedByDefault)
	{
		TransliterationService = Ioc.Default.GetService<TransliterationService>()!;

		TypeFilters = Enum.GetValues<EquipmentTypeGroup>()
			.Where(e => e != EquipmentTypeGroup.Unknown)
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
		List<EquipmentTypes> enabledFilters = TypeFilters
			.Where(f => f.IsChecked)
			.SelectMany(f => f.Value.ToTypes())
			.ToList();

		if (!enabledFilters.Contains(equipment.CategoryType)) return false;
		if (!string.IsNullOrEmpty(NameFilter) && !TransliterationService.Matches(equipment, NameFilter)) return false;

		return true;
	}
}
