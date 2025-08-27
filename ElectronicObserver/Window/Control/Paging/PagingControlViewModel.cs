using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ElectronicObserver.Window.Control.Paging;

public partial class PagingControlViewModel : ObservableObject
{
	public int CurrentPage { get; set; } = 1;

	public List<object> Items { get; set; } = new();

	public ObservableCollection<object> DisplayedItems { get; } = [];

	public int ItemsPerPage { get; set; } = 10;

	public int LastPage => Math.Max(1, (int)Math.Ceiling(Items.Count / (decimal)Math.Max(1, ItemsPerPage)));

	public PagingControlTranslationViewModel PagingControl { get; } = new();

	public PagingControlViewModel()
	{
		PropertyChanged += OnPagerUpdate;
		PropertyChanged += OnPagerUpdate2;
		UpdateCollection();
	}

	public void DisplayPageFromElementKey(int elementKey)
	{
		int page = elementKey / ItemsPerPage;

		if (page > LastPage) return;

		CurrentPage = page + 1;
	}

	private void OnPagerUpdate2(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName is nameof(CurrentPage) or nameof(LastPage)) return;

		NextPageCommand.NotifyCanExecuteChanged();
		PreviousPageCommand.NotifyCanExecuteChanged();
	}

	private void OnPagerUpdate(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName is not nameof(CurrentPage) and not nameof(ItemsPerPage) and not nameof(Items)) return;

		UpdateCollection();
	}

	private void UpdateCollection()
	{
		CurrentPage = Math.Clamp(CurrentPage, 1, LastPage);

		DisplayedItems.Clear();

		if (Items.Count > 0)
		{
			IEnumerable<object> items = Items
				.Skip(ItemsPerPage * (CurrentPage - 1))
				.Take(ItemsPerPage);

			foreach (object item in items)
			{
				DisplayedItems.Add(item);
			}
		}

		OnPropertyChanged(nameof(LastPage));
	}

	private bool NextPageEnabled => CurrentPage < LastPage;
	private bool PreviousPageEnabled => CurrentPage > 1;

	[RelayCommand(CanExecute = nameof(NextPageEnabled))]
	private void NextPage()
	{
		CurrentPage++;
	}

	[RelayCommand(CanExecute = nameof(PreviousPageEnabled))]
	private void PreviousPage()
	{
		CurrentPage--;
	}
}
