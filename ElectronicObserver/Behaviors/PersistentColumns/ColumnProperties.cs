using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ElectronicObserver.Behaviors.PersistentColumns;

public class ColumnProperties
{
	public string Header { get; set; } = "";
	public string SortMemberPath { get; set; } = "";
	public DataGridLength Width { get; set; } = DataGridLength.Auto;
	public int DisplayIndex { get; set; }
	public ListSortDirection? SortDirection { get; set; }
	public Visibility Visibility { get; set; }
}
