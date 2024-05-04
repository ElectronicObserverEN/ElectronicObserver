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
	private List<Test> Data { get; } =
	[
		new() { Name = "Test1", Description = "TestDescription1" },
		new() { Name = "Test2", Description = "TestDescription2" },
		new() { Name = "Test3", Description = "TestDescription3" },
		new() { Name = "Test4", Description = "TestDescription4" },
	];

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
	[ObservableProperty] private List<ColumnModel> _columnProperties = [];

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
