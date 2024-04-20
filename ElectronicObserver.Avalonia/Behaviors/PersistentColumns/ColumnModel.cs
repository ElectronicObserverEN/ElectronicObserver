using Avalonia.Controls;
using System.ComponentModel;

namespace ElectronicObserver.Avalonia.Behaviors.PersistentColumns;

public class ColumnModel
{
	public string Header { get; set; } = "";
	public string SortMemberPath { get; set; } = "";
	public DataGridLength Width { get; set; } = DataGridLength.Auto;
	public int DisplayIndex { get; set; }
	public ListSortDirection? SortDirection { get; set; }
	public bool IsVisible { get; set; }
}
