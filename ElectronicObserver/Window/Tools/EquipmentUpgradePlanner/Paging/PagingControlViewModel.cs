using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.Paging;

public partial class PagingControlViewModel : ObservableObject
{
	public int CurrentPage { get; set; } = 0;

	private ObservableCollection<object?> Items { get; set; } = new();

	public List<object?> ItemsPaged { get; set; } = new();

	public int ItemsPerPage { get; set; } = 10;

	public int LastPage => (int)Math.Ceiling(Items.Count / (decimal)ItemsPerPage);

	public bool NextPageEnabled => CurrentPage < LastPage;
	public bool PreviousPageEnabled => CurrentPage > 1;

	public PagingControlViewModel()
	{
		PropertyChanged += OnPagerUpdate;
		UpdateCollection();
	}

	public PagingControlViewModel(ObservableCollection<object?> items) : this()
	{
		Items = items;
		Items.CollectionChanged += Items_CollectionChanged;
	}

	private void Items_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
	{
		UpdateCollection();
	}

	private void OnPagerUpdate(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName is not nameof(CurrentPage) and not nameof(ItemsPerPage)) return;

		OnPropertyChanged(nameof(PreviousPageEnabled));
		OnPropertyChanged(nameof(NextPageEnabled));

		UpdateCollection();
	}

	private void UpdateCollection()
	{
		// dont go out of bound
		CurrentPage = Math.Max(CurrentPage, 0);
		CurrentPage = Math.Min(CurrentPage, LastPage);

		ItemsPaged = Items
			.Skip(ItemsPerPage * (CurrentPage - 1))
			.Take(ItemsPerPage)
			.ToList();
	}

	/// <summary>
	/// Batch update current collection
	/// </summary>
	/// <param name="collection"></param>
	public void UpdateSourceCollection<T>(IList<T> collection)
	{
		// Unsubsribe from event may enhance perf on large collections ?
		Items.CollectionChanged -= Items_CollectionChanged;

		Items.Clear();

		foreach (object? item in collection)
		{
			Items.Add(item);
		}

		Items.CollectionChanged += Items_CollectionChanged;
		UpdateCollection();
	}

	[RelayCommand]
	public void NextPage()
	{
		CurrentPage++;
	}

	[RelayCommand]
	public void PreviousPage()
	{
		CurrentPage--;
	}
}
