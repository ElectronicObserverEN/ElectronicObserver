using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace ElectronicObserver.Common.Datagrid;

public class ColumnSelectorViewModel : WindowViewModelBase
{
	public List<ColumnViewModel> Columns { get; set; }

	public List<ListSortDirection?> SortDirections { get; } = new()
	{
		ListSortDirection.Ascending,
		ListSortDirection.Descending,
	};

	public ColumnSelectorViewModel(List<ColumnViewModel> columns)
	{
		Columns = columns;
	}
}
