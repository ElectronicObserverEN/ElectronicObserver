using System.Collections.Generic;
using System.Linq;

namespace ElectronicObserver.Common.Datagrid;

public abstract class DataGridViewModelBase
{
	public Dictionary<string, ColumnViewModel> Columns { get; set; } = new();

	public ColumnViewModel? GetColumn(string name)
	{
		Columns.TryGetValue(name, out ColumnViewModel? col);
		return col;
	}

	protected void OpenColumnSelectorBase()
	{
		List<ColumnViewModel> columnList = Columns.Values.ToList();
		List<(ColumnViewModel, ColumnViewModel)> columns = columnList.Select(column => (column, new ColumnViewModel()
		{
			DisplayIndex = column.DisplayIndex,
			Header = column.Header,
			SortDirection = column.SortDirection,
			Visibility = column.Visibility,
			Width = column.Width
		})).ToList();

		List<ColumnViewModel> columnsForSelector = columns.Select(cols => cols.Item2).ToList();

		ColumnSelectorView columnSelectionView = new(new(columnsForSelector));

		if (columnSelectionView.ShowDialog() == true)
		{
			// Apply changes
			foreach ((ColumnViewModel column, ColumnViewModel columnAfterChanges) in columns)
			{
				column.Width = columnAfterChanges.Width;
				column.Visibility = columnAfterChanges.Visibility;
				column.DisplayIndex = columnAfterChanges.DisplayIndex;
				column.SortDirection = columnAfterChanges.SortDirection;
			}
		}
	}
}
