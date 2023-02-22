using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls.Primitives;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Behaviors.PersistentColumns;

namespace ElectronicObserver.Common.Datagrid;

public partial class DataGridViewModel : ObservableObject
{
	public List<ColumnProperties> ColumnProperties { get; set; } = new();
	public List<SortDescription> SortDescriptions { get; set; } = new();

	public DataGridTranslationViewModel DataGrid { get; set; } = new();

	[RelayCommand]
	private void OpenColumnSelector()
	{
		List<ColumnViewModel> columns = ColumnProperties
			.Select(column => new ColumnViewModel(column))
			.ToList();

		ColumnSelectorView columnSelectionView = new(new(columns));

		if (columnSelectionView.ShowDialog() == true)
		{
			// Apply changes
			foreach (ColumnViewModel column in columns)
			{
				column.SaveChanges();
			}

			// Trigger PropertyChanged "manually"
			ColumnProperties = new(columns.Select(col => col.ColumnProperties));
		}
	}

	[RelayCommand]
	private void HideColumn(object columnHeader)
	{
		string headerText = columnHeader switch
		{
			string stringHeader => stringHeader,
			_ => "",
		};

		ColumnProperties? column = ColumnProperties.FirstOrDefault(col => headerText == col.Header);

		if (column is not null)
		{
			column.Visibility = System.Windows.Visibility.Collapsed;

			// Trigger PropertyChanged "manually"
			ColumnProperties = new(ColumnProperties);
		}
	}

	[RelayCommand]
	private void ClearSorting()
	{
		SortDescriptions = new();
	}
}
