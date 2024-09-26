using System.Collections;
using System.Collections.ObjectModel;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Avalonia.Behaviors.PersistentColumns;

namespace ElectronicObserver.Avalonia.ShipGroup;

public partial class ShipGroupViewModel : ObservableObject
{
	public ShipGroupTranslationViewModel FormShipGroup { get; } = new();

	public DataGridSettingsModel DataGridSettings { get; } = new();

	[ObservableProperty] private int _conditionBorder;
	[ObservableProperty] private bool _autoUpdate = true;
	[ObservableProperty] private bool _showStatusBar = true;

	[ObservableProperty] private ObservableCollection<ShipGroupItem> _groups = [];

	[ObservableProperty] private GridLength _groupHeight;

	[ObservableProperty] private ObservableCollection<ShipGroupItemViewModel> _items = [];
	[ObservableProperty] private DataGridCollectionView _collectionView = new(new List<ShipGroupItemViewModel>());
	[ObservableProperty] private string _shipCountText = "";
	[ObservableProperty] private string _levelTotalText = "";
	[ObservableProperty] private string _levelAverageText = "";

	[ObservableProperty] private ObservableCollection<ColumnModel> _columnProperties = [];
	[ObservableProperty] private DataGridSortDescriptionCollection _sortDescriptions = [];
	[ObservableProperty] private int _frozenColumns;

	public required Action<ShipGroupItem> SelectGroupAction { get; init; }
	public required Func<Task> AddGroupAction { get; init; }
	public required Func<ShipGroupItem, Task> CopyGroupAction { get; init; }
	public required Func<ShipGroupItem, Task> RenameGroupAction { get; init; }
	public required Action<ShipGroupItem> DeleteGroupAction { get; init; }

	public required Action AddToGroupAction { get; init; }
	public required Func<Task> CreateGroupAction { get; init; }
	public required Action ExcludeFromGroupAction { get; init; }
	public required Action FilterGroupAction { get; init; }
	public required Action FilterColumnsAction { get; init; }
	public required Action ExportCsvAction { get; init; }
	public required Func<DataGridSettingsModel, Task> OpenDataGridSettingsAction { get; init; }

	[ObservableProperty] private bool _anyShipsSelected;
	public List<ShipGroupItemViewModel> SelectedShips { get; private set; } = [];

	// hacky way cause the logic is in the EO project
	public static FuncValueConverter<int, string>? SpeedToDisplayConverter { get; set; }
	public static FuncValueConverter<int, string>? RangeToDisplayConverter { get; set; }

	partial void OnItemsChanging(ObservableCollection<ShipGroupItemViewModel>? value)
	{
		if (value is null) return;

		CollectionView = new(value);

		CollectionView.SortDescriptions.Clear();
		CollectionView.SortDescriptions.AddRange(SortDescriptions);
	}

	[RelayCommand]
	private void SelectionChanged(IList selectedItems)
	{
		SelectedShips = selectedItems
			.OfType<ShipGroupItemViewModel>()
			.ToList();

		int selectedShipCount = SelectedShips.Count;

		AnyShipsSelected = selectedShipCount > 0;

		if (selectedShipCount >= 2)
		{
			int membersCount = SelectedShips.Count;
			int levelSum = SelectedShips.Sum(s => s.Level);
			double levelAverage = levelSum / Math.Max(membersCount, 1.0);
			long expSum = SelectedShips.Sum(s => (long)s.ExpTotal);
			double expAverage = expSum / Math.Max(membersCount, 1.0);

			ShipCountText = string.Format(ShipGroupResources.SelectedShips, selectedShipCount, Items.Count);
			LevelTotalText = string.Format(ShipGroupResources.TotalAndAverageLevel, levelSum, levelAverage);
			LevelAverageText = string.Format(ShipGroupResources.TotalAndAverageExp, expSum, expAverage);
		}
		else
		{
			int membersCount = Items.Count;
			int levelSum = Items.Sum(s => s.Level);
			double levelAverage = levelSum / Math.Max(membersCount, 1.0);
			long expSum = Items.Sum(s => (long)s.ExpTotal);
			double expAverage = expSum / Math.Max(membersCount, 1.0);

			ShipCountText = string.Format(ShipGroupResources.ShipCount, Items.Count);
			LevelTotalText = string.Format(ShipGroupResources.TotalAndAverageLevel, levelSum, levelAverage);
			LevelAverageText = string.Format(ShipGroupResources.TotalAndAverageExp, expSum, expAverage);
		}
	}

	[RelayCommand]
	private void SelectGroup(ShipGroupItem group)
	{
		SelectGroupAction.Invoke(group);
		SelectionChanged(SelectedShips);
	}

	[RelayCommand]
	private async Task AddGroup() => await AddGroupAction.Invoke();

	[RelayCommand]
	private async Task CopyGroup(ShipGroupItem group) => await CopyGroupAction.Invoke(group);

	[RelayCommand]
	private async Task RenameGroup(ShipGroupItem group) => await RenameGroupAction.Invoke(group);

	[RelayCommand]
	private void DeleteGroup(ShipGroupItem group) => DeleteGroupAction.Invoke(group);

	[RelayCommand]
	private void AddToGroup() => AddToGroupAction.Invoke();

	[RelayCommand]
	private async Task CreateGroup() =>  await CreateGroupAction.Invoke();

	[RelayCommand]
	private void ExcludeFromGroup() => ExcludeFromGroupAction.Invoke();

	[RelayCommand]
	private void FilterGroup() => FilterGroupAction.Invoke();

	[RelayCommand]
	private void FilterColumns() => FilterColumnsAction.Invoke();

	[RelayCommand]
	private void ExportCsv() => ExportCsvAction.Invoke();

	[RelayCommand]
	private async Task OpenDataGridSettings() => await OpenDataGridSettingsAction.Invoke(DataGridSettings);
}
