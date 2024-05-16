using System.Collections;
using System.Collections.ObjectModel;
using Avalonia.Collections;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Avalonia.Behaviors.PersistentColumns;
using static System.Net.Mime.MediaTypeNames;

namespace ElectronicObserver.Avalonia.ShipGroup;

public partial class ShipGroupViewModel : ObservableObject
{
	public ShipGroupTranslationViewModel FormShipGroup { get; } = new();

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

	public required Action<ShipGroupItem> SelectGroupAction { get; init; }
	public required Action AddGroupAction { get; init; }
	public required Action<ShipGroupItem> CopyGroupAction { get; init; }
	public required Action<ShipGroupItem> RenameGroupAction { get; init; }
	public required Action<ShipGroupItem> DeleteGroupAction { get; init; }

	public required Action AddToGroupAction { get; init; }
	public required Action CreateGroupAction { get; init; }
	public required Action ExcludeFromGroupAction { get; init; }
	public required Action FilterGroupAction { get; init; }
	public required Action FilterColumnsAction { get; init; }
	public required Action ExportCsvAction { get; init; }

	[ObservableProperty] private bool _anyShipsSelected;
	public List<ShipGroupItemViewModel> SelectedShips { get; private set; } = [];

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
		ColumnProperties = group.Columns;
		SortDescriptions = group.SortDescriptions;
		SelectGroupAction.Invoke(group);
		SelectionChanged(SelectedShips);
	}

	[RelayCommand]
	private void AddGroup() => AddGroupAction.Invoke();

	[RelayCommand]
	private void CopyGroup(ShipGroupItem group) => CopyGroupAction.Invoke(group);

	[RelayCommand]
	private void RenameGroup(ShipGroupItem group) => RenameGroupAction.Invoke(group);

	[RelayCommand]
	private void DeleteGroup(ShipGroupItem group) => DeleteGroupAction.Invoke(group);

	[RelayCommand]
	private void AddToGroup() => AddToGroupAction.Invoke();

	[RelayCommand]
	private void CreateGroup() => CreateGroupAction.Invoke();

	[RelayCommand]
	private void ExcludeFromGroup() => ExcludeFromGroupAction.Invoke();

	[RelayCommand]
	private void FilterGroup() => FilterGroupAction.Invoke();

	[RelayCommand]
	private void FilterColumns() => FilterColumnsAction.Invoke();

	[RelayCommand]
	private void ExportCsv() => ExportCsvAction.Invoke();
}
