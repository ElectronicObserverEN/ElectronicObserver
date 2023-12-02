using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Common.ContentDialogs.ExportFilter;

public class DestinationItemViewModel : ObservableObject
{
	public required string Display { get; init; }
	public required List<int> CellIds { get; init; }
	public bool IsChecked { get; set; } = true;
}
