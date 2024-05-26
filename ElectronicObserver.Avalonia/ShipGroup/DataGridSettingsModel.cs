using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Avalonia.ShipGroup;

public partial class DataGridSettingsModel : ObservableObject
{
	[ObservableProperty] private int _columnHeaderHeight = 32;
	[ObservableProperty] private int _rowHeight = 32;
}
