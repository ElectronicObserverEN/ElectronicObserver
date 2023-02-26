using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Behaviors.PersistentColumns;

namespace ElectronicObserver.Common.Datagrid;

public partial class DataGridViewModel<T> : ObservableObject where T : IEnumerable
{
	public List<ColumnProperties> ColumnProperties { get; set; } = new();
	public List<SortDescription> SortDescriptions { get; set; } = new();

	public DataGridTranslationViewModel DataGrid { get; set; } = new();

	public T ItemsSource { get; set; }
	public ICollectionView Items { get; set; }

	public DataGridViewModel(T items)
	{
		ItemsSource = items;
		Items = CollectionViewSource.GetDefaultView(ItemsSource);

		PropertyChanged += DataGridViewModel_PropertyChanged;
	}

	private void DataGridViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName is nameof(ItemsSource))
		{
			Items = CollectionViewSource.GetDefaultView(ItemsSource);
		}
		else if (e.PropertyName is nameof(Items) or nameof(SortDescriptions) && Items is not null)
		{
			Items.SortDescriptions.Clear();

			foreach (SortDescription sortDescription in SortDescriptions)
			{
				Items.SortDescriptions.Add(sortDescription);
			}
		}
	}

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
