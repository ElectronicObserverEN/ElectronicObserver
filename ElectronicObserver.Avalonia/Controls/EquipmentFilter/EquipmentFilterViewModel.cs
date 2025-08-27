using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Core.Services;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Extensions;

namespace ElectronicObserver.Avalonia.Controls.EquipmentFilter;

public partial class EquipmentFilterViewModel : ObservableObject
{
	public List<Filter> TypeFilters { get; }

	private TransliterationService TransliterationService { get; }

	[ObservableProperty] public partial string? NameFilter { get; set; } = "";

	public EquipmentFilterViewModel(TransliterationService transliterationService,
		List<IEquipmentData>? equipment = null)
	{
		TransliterationService = transliterationService;
		
		// todo: it's not ideal to do it this way because we want to reuse the selector viewmodel
		IEnumerable<EquipmentTypeGroup> typeGroups = equipment
			?.Select(e => e.MasterEquipment.CategoryType)
			.Distinct()
			.Select(t => t.ToGroup())
			.Distinct()
			?? Enum.GetValues<EquipmentTypeGroup>();

		TypeFilters = typeGroups
			.OrderBy(g => g)
			.Select(t => new Filter(t))
			.ToList();

		foreach (Filter filter in TypeFilters)
		{
			filter.PropertyChanged += (_, _) => OnPropertyChanged(string.Empty);
		}
	}

	public bool MeetsFilterCondition(IEquipmentData equipment)
		=> MeetsFilterCondition(equipment.MasterEquipment);

	private bool MeetsFilterCondition(IEquipmentDataMaster equipment)
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
		bool allChecked = TypeFilters.All(f => f.IsChecked);

		foreach (Filter typeFilter in TypeFilters)
		{
			typeFilter.IsChecked = !allChecked;
		}
	}
}
