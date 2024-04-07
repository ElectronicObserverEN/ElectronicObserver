using System.Collections;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ElectronicObserver.Avalonia.ShipGroup;

public partial class ShipGroupViewModel : ObservableObject
{
	public ShipGroupTranslationViewModel FormShipGroup { get; } = new();

	[ObservableProperty] private bool _autoUpdate = true;
	[ObservableProperty] private bool _showStatusBar = true;

	[ObservableProperty] private ObservableCollection<ShipGroupItem> _groups = [];

	[ObservableProperty] private ObservableCollection<ShipGroupItemViewModel> _items = [];
	[ObservableProperty] private string _shipCountText = "";
	[ObservableProperty] private string _levelTotalText = "";
	[ObservableProperty] private string _levelAverageText = "";

	public required Action<ShipGroupItem> SelectGroupAction { get; init; }
	public required Action AddGroupAction { get; init; }
	public required Action<ShipGroupItem> CopyGroupAction { get; init; }
	public required Action<ShipGroupItem> RenameGroupAction { get; init; }
	public required Action<ShipGroupItem> DeleteGroupAction { get; init; }

	[RelayCommand]
	private void SelectionChanged(IList selectedItems)
	{
		List<ShipGroupItemViewModel> selectedShips = selectedItems
			.OfType<ShipGroupItemViewModel>()
			.ToList();

		int selectedShipCount = selectedShips.Count;

		if (selectedShipCount >= 2)
		{
			int membersCount = selectedShips.Count;
			int levelSum = selectedShips.Sum(s => s.Level);
			double levelAverage = levelSum / Math.Max(membersCount, 1.0);
			long expSum = selectedShips.Sum(s => (long)s.ExpTotal);
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
	private void SelectGroup(ShipGroupItem group) => SelectGroupAction.Invoke(group);

	[RelayCommand]
	private void AddGroup() => AddGroupAction.Invoke();

	[RelayCommand]
	private void CopyGroup(ShipGroupItem group) => CopyGroupAction.Invoke(group);

	[RelayCommand]
	private void RenameGroup(ShipGroupItem group) => RenameGroupAction.Invoke(group);

	[RelayCommand]
	private void DeleteGroup(ShipGroupItem group) => DeleteGroupAction.Invoke(group);
}
