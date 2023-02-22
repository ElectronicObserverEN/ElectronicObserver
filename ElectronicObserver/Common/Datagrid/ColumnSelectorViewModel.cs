using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace ElectronicObserver.Common.Datagrid;

public class ColumnSelectorViewModel : WindowViewModelBase
{
	public List<ColumnViewModel> Columns { get; set; }

	public ColumnSelectorViewModel(List<ColumnViewModel> columns)
	{
		Columns = columns;
	}
}
