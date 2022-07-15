using System.ComponentModel;
using System.Windows.Controls;

namespace ElectronicObserver.Behaviors.PersistentColumns;

public class ColumnProperties
{
	public DataGridLength Width { get; set; } = DataGridLength.Auto;
	public int DisplayIndex { get; set; }
	public ListSortDirection? SortDirection { get; set; }
}
