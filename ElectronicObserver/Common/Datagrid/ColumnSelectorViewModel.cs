using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace ElectronicObserver.Common.Datagrid;

public class ColumnSelectorViewModel : WindowViewModelBase
{
	public ColumnSelectorTranslationViewModel ColumnSelector { get; set; } = new();
	public List<ColumnViewModel> Columns { get; set; }

	public ColumnSelectorViewModel(List<ColumnViewModel> columns)
	{
		Columns = columns;
	}
}
