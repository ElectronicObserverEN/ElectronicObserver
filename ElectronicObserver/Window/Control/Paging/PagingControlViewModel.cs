using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ElectronicObserver.Window.Control.Paging;

public partial class PagingControlViewModel<T> : ObservableObject
{
	public int CurrentPage { get; set; }

	public List<T> Items { get; set; } = new();

	public List<T> DisplayedItems { get; private set; } = new();

	public int ItemsPerPage { get; set; } = 10;

	public int LastPage => (int)Math.Ceiling(Items.Count / (decimal)ItemsPerPage);

	public PagingControlTranslationViewModel PagingControl { get; } = new();

	public PagingControlViewModel()
	{
		PropertyChanged += OnPagerUpdate;
		PropertyChanged += OnPagerUpdate2;
		UpdateCollection();
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
		// don't go out of bound
		CurrentPage = Math.Max(CurrentPage, Items.Any() ? 1 : 0);
		CurrentPage = Math.Min(CurrentPage, LastPage);

		DisplayedItems = Items
			.Skip(ItemsPerPage * (CurrentPage - 1))
			.Take(ItemsPerPage)
			.ToList();

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
