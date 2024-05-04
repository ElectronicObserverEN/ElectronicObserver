using Avalonia.Controls;
using System.ComponentModel;

namespace ElectronicObserver.Avalonia.Behaviors.PersistentColumns;

public class ColumnModel
{
	/// <summary>
	/// This is only needed to support the old data format.
	/// </summary>
	public required string Name { get; set; }
	public string Header { get; set; } = "";
	public string SortMemberPath { get; set; } = "";
	public DataGridLength Width { get; set; } = DataGridLength.Auto;
	public int DisplayIndex { get; set; }
	public ListSortDirection? SortDirection { get; set; }
	public bool IsVisible { get; set; }
}
