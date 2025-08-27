using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Avalonia.ShipGroup;

public partial class DataGridSettingsModel : ObservableObject
{
	[ObservableProperty] public partial int ColumnHeaderHeight { get; set; } = 32;
	[ObservableProperty] public partial int RowHeight { get; set; } = 32;
}
