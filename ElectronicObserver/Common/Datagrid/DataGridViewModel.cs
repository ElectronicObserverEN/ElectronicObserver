using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Behaviors.PersistentColumns;

namespace ElectronicObserver.Common.Datagrid;

public partial class DataGridViewModel<T> : ObservableObject
{
	public List<ColumnProperties> ColumnProperties { get; set; } = new();
	public List<SortDescription> SortDescriptions { get; set; } = new();

	public DataGridTranslationViewModel DataGrid { get; set; } = new();

	public ICollectionView Items { get; private set; }
	public ObservableCollection<T> ItemsSource { get; set; }

	public DataGridViewModel(ObservableCollection<T> items) : this()
	{
		ItemsSource = items;
	}

	public DataGridViewModel()
	{
		ItemsSource = new();
		Items = CollectionViewSource.GetDefaultView(ItemsSource);

		PropertyChanged += DataGridViewModel_PropertyChanged;
	}

	private void DataGridViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName is nameof(Items) or nameof(SortDescriptions) or nameof(FilterValue))
		{
			Items.Filter = CollectionFilter;

			Items.SortDescriptions.Clear();

			foreach (SortDescription sortDescription in SortDescriptions)
			{
				Items.SortDescriptions.Add(sortDescription);
			}
		}
		if (e.PropertyName is nameof(ItemsSource))
		{
			Items = CollectionViewSource.GetDefaultView(ItemsSource);
		}
	}

	#region Filtering
	public Func<T, bool>? FilterValue { get; set; }

	private bool CollectionFilter(object item)
	{
		if (FilterValue is null) return true;

		return (item is T typedItem && FilterValue(typedItem));
	}
	#endregion

	#region Data manipulation
	public void AddRange(IEnumerable<T> items)
	{
		List<T> newCollection = ItemsSource.ToList();
		newCollection.AddRange(items);

		ItemsSource = new(newCollection);
	}
	#endregion

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
			ColumnProperties = columns
				.Select(col => col.ColumnProperties)
				.OfType<ColumnProperties>()
				.ToList();
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
