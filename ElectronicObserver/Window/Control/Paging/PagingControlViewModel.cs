using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ElectronicObserver.Window.Control.Paging;

public partial class PagingControlViewModel : ObservableObject
{
	public int CurrentPage { get; set; } = 1;

	public List<object> Items { get; set; } = new();

	public List<object> DisplayedItems { get; private set; } = new();

	public int ItemsPerPage { get; set; } = 10;

	public int LastPage => Math.Max(1, (int)Math.Ceiling(Items.Count / (decimal)Math.Max(1, ItemsPerPage)));

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
		CurrentPage = Math.Clamp(CurrentPage, 1, LastPage);

		DisplayedItems = Items switch
		{
			{ Count: > 0 } => Items
				.Skip(ItemsPerPage * (CurrentPage - 1))
				.Take(ItemsPerPage)
				.ToList(),

			_ => new(),
		};

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
