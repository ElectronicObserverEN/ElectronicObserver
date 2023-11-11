using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Common.ContentDialogs.ExportFilter;

// todo: all required
public class DestinationItemViewModel : ObservableObject
{
	public string Display { get; init; } = null!;
	public List<int> CellIds { get; init; } = null!;
	public bool IsChecked { get; set; } = true;
}
