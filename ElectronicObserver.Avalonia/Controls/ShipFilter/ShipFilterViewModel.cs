using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Core.Services;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Extensions;

namespace ElectronicObserver.Avalonia.Controls.ShipFilter;

public sealed partial class ShipFilterViewModel : ObservableObject
{
	private TransliterationService TransliterationService { get; }

	public List<ShipTypeGroupFilterViewModel> TypeFilters { get; } = Enum
		.GetValues<ShipTypeGroup>()
		.Select(g => new ShipTypeGroupFilterViewModel(g))
		.ToList();
	
	[ObservableProperty] public partial int AswMin { get; set; } = 0;
	[ObservableProperty] public partial int AswMax { get; set; } = 200;

	[ObservableProperty] public partial int LuckMin { get; set; } = 0;
	[ObservableProperty] public partial int LuckMax { get; set; } = 200;

	[ObservableProperty] public partial string? NameFilter { get; set; }
	[ObservableProperty] public partial bool? CanEquipDaihatsu { get; set; }
	[ObservableProperty] public partial bool? CanEquipTank { get; set; }
	[ObservableProperty] public partial bool? CanEquipFcf { get; set; }
	[ObservableProperty] public partial bool? CanEquipBulge { get; set; }
	[ObservableProperty] public partial bool? CanEquipSeaplaneFighter { get; set; }
	[ObservableProperty] public partial bool? IsExpansionSlotAvailable { get; set; }
	[ObservableProperty] public partial bool FinalRemodel { get; set; } = true;
	
	/// <inheritdoc/>
	public ShipFilterViewModel(TransliterationService transliterationService)
	{
		TransliterationService = transliterationService;

		foreach (ShipTypeGroupFilterViewModel typeFilter in TypeFilters)
		{
			typeFilter.PropertyChanged += TypeFilterOnPropertyChanged;
		}
	}

	private void TypeFilterOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		OnPropertyChanged(nameof(TypeFilters));
	}

	public bool MeetsFilterCondition(IShipData ship)
	{
		List<ShipTypes> activeTypeFilters = TypeFilters
			.Where(f => f.IsChecked)
			.SelectMany(f => f.Value.ToTypes())
			.ToList();

		if (!activeTypeFilters.Contains(ship.MasterShip.ShipType)) return false;
		if (ship.ASWBase < AswMin) return false;
		if (ship.ASWBase > AswMax) return false;
		if (ship.LuckBase < LuckMin) return false;
		if (ship.LuckBase > LuckMax) return false;
		if (CanEquipDaihatsu != null && CanEquipDaihatsu != ship.CanEquipDaihatsu()) return false;
		if (CanEquipTank != null && CanEquipTank != ship.CanEquipTank()) return false;
		if (CanEquipFcf != null && CanEquipFcf != ship.CanEquipFcf()) return false;
		if (CanEquipBulge != null && CanEquipBulge != ship.CanEquipBulge()) return false;
		if (CanEquipSeaplaneFighter != null && CanEquipSeaplaneFighter != ship.CanEquipSeaplaneFighter()) return false;
		if (IsExpansionSlotAvailable != null && IsExpansionSlotAvailable != ship.IsExpansionSlotAvailable) return false;
		if (FinalRemodel && !ship.IsFinalRemodel()) return false;
		if (!string.IsNullOrEmpty(NameFilter) && !TransliterationService.Matches(ship.MasterShip, NameFilter)) return false;
		// other filters

		return true;
	}
}
