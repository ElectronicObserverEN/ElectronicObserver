using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Avalonia.Behaviors.PersistentColumns;
using ElectronicObserver.Avalonia.ShipGroup;

namespace ElectronicObserver.Avalonia.Samples.ViewModels;

public class Test
{
	public required string Name { get; set; }
	public required string Description { get; set; }
}

public partial class MainViewModel : ViewModelBase
{
	private List<Test> Data { get; } = [];

	public ObservableCollection<ShipGroupItem> Groups { get; set; } =
	[
		new()
		{
			Name = "Group1",
			Columns =
			[
				new() { Name = "", IsVisible = true, DisplayIndex = 0, },
				new() { Name = "", IsVisible = true, DisplayIndex = 1, },
			],
			SortDescriptions = [],
		},
		new()
		{
			Name = "Group2",
			Columns =
			[
				new() { Name = "", IsVisible = false },
				new() { Name = "", IsVisible = false },
			],
			SortDescriptions = [],
		},
	];

	[ObservableProperty] private ShipGroupItem? _selectedGroup;
	[ObservableProperty] private DataGridCollectionView _collectionView = new(new List<Test>());
	[ObservableProperty] private ObservableCollection<ColumnModel> _columnProperties = [];

	public MainViewModel()
	{
		for (int i = 0; i < 100; i++)
		{
			Data.Add(new()
			{
				Name = $"Test{i + 1}",
				Description = $"TestDescription{i + 1}",
			});
		}
	}

	[RelayCommand]
	private void SelectGroup(ShipGroupItem group)
	{
		SelectedGroup = group;
	}

	partial void OnSelectedGroupChanging(ShipGroupItem? oldValue, ShipGroupItem? newValue)
	{
		if (oldValue is null) return;

		oldValue.SortDescriptions = CollectionView.SortDescriptions;
	}

	partial void OnSelectedGroupChanged(ShipGroupItem? value)
	{
		if (value is null) return;

		ColumnProperties = value.Columns;
		CollectionView = new(Data);

		CollectionView.SortDescriptions.Clear();
		CollectionView.SortDescriptions.AddRange(value.SortDescriptions);
	}
}
