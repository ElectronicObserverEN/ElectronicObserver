using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Avalonia.ShipGroup;

public partial class ShipGroupViewModel : ObservableObject
{
	public ShipGroupTranslationViewModel FormShipGroup { get; } = new();

	[ObservableProperty]
	private ObservableCollection<ShipGroupItemViewModel> _items = [];
}
