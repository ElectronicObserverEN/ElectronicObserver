using System.Collections.Generic;

namespace ElectronicObserver.Common.Datagrid;

public class ColumnSelectorViewModel : WindowViewModelBase
{
	public ColumnSelectorTranslationViewModel ColumnSelector { get; } = new();
	public List<ColumnViewModel> Columns { get; }

	public ColumnSelectorViewModel(List<ColumnViewModel> columns)
	{
		Columns = columns;
	}
}
