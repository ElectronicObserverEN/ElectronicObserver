using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Behaviors.PersistentColumns;

namespace ElectronicObserver.Common.Datagrid;

public partial class DataGridViewModel : ObservableObject
{
	public List<ColumnProperties> ColumnProperties { get; set; } = new();
	public List<SortDescription> SortDescriptions { get; set; } = new();

	/// <summary>
	/// List<SortDescription>.Find returns a non null SortDescription so i put my own .Find logic in this method
	/// </summary>
	/// <returns></returns>
	private SortDescription? FindSortDescription(string sortMemberPath)
	{
		if (string.IsNullOrEmpty(sortMemberPath)) return null;
		if (!SortDescriptions.Any(sort => sort.PropertyName == sortMemberPath)) return null;
		return SortDescriptions.Find(sort => sort.PropertyName == sortMemberPath);
	}

	[RelayCommand]
	private void OpenColumnSelector()
	{
		List<ColumnViewModel> columns = ColumnProperties
			.Select(column => new ColumnViewModel(column, FindSortDescription(column.SortMemberPath)))
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

			SortDescriptions = new(columns
				.Select(col => col.SortDescription)
				.Where(col => col is not null)
				.Cast<SortDescription>());
		}
	}

	[RelayCommand]
	private void HideColumn(string columnName)
	{
		ColumnProperties? column = ColumnProperties.FirstOrDefault(col => columnName == col.Header);

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
