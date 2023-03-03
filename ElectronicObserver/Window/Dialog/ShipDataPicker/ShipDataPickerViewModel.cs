using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Common.Datagrid;
using ElectronicObserver.Data;
using ElectronicObserver.Window.Control.ShipFilter;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Dialog.ShipDataPicker;

public partial class ShipDataPickerViewModel : WindowViewModelBase
{
	public ShipFilterViewModel Filters { get; } = new();

	public IShipData? PickedShip { get; private set; }
	public ShipDataViewModel? SelectedShip { get; set; }

	public ShipDataPickerTranslationViewModel ShipDataPicker { get; } = new();

	public DataGridViewModel<ShipDataViewModel> DataGridViewModel { get; set; }

	public ShipDataPickerViewModel()
	{
		List<ShipDataViewModel> allShips = KCDatabase.Instance.Ships.Values.Cast<IShipData>().Select(s => new ShipDataViewModel(s)).ToList();
		DataGridViewModel = new(new(allShips))
		{
			FilterValue = s => Filters.MeetsFilterCondition(s.Ship)
		};

		Filters.PropertyChanged += (_, _) => ReloadShips();

		ReloadShips();
	}

	public void LoadWithShips(IEnumerable<IShipData> ships)
	{
		PickedShip = null;
		SelectedShip = null;

		DataGridViewModel.ItemsSource.Clear();
		DataGridViewModel.AddRange(ships.Select(s => new ShipDataViewModel(s)).ToList());
		ReloadShips();
	}

	private void ReloadShips()
		=> DataGridViewModel.Items.Refresh();

	[RelayCommand]
	public void SelectShip()
	{
		PickedShip = SelectedShip?.Ship;
	}
}
