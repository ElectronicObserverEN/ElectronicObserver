using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Data;
using ElectronicObserver.Window.Control.ShipFilter;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Dialog.ShipDataPicker;

public partial class ShipDataPickerViewModel : WindowViewModelBase
{
	private List<ShipDataViewModel> AllShips { get; set; }

	public ObservableCollection<ShipDataViewModel> ShipsFiltered { get; } = new();

	public ShipFilterViewModel Filters { get; } = new();

	public IShipData? PickedShip { get; private set; }
	public ShipDataViewModel? SelectedShip { get; set; }

	public ShipDataPickerTranslationViewModel ShipDataPicker { get; } = new();


	public ShipDataPickerViewModel()
	{
		AllShips = KCDatabase.Instance.Ships.Values.Cast<IShipData>().Select(s => new ShipDataViewModel(s)).ToList();

		Filters.PropertyChanged += (_, _) => ReloadShips();

		ReloadShips();
	}

	public void LoadWithShips(IEnumerable<IShipData> ships)
	{
		PickedShip = null;
		SelectedShip = null;

		AllShips = ships.Select(s => new ShipDataViewModel(s)).ToList();
		ReloadShips();
	}

	private void ReloadShips()
	{
		ShipsFiltered.Clear();

		List<ShipDataViewModel> filteredShips = AllShips
			.Where(s => Filters.MeetsFilterCondition(s.Ship))
			.ToList();

		foreach (ShipDataViewModel ship in filteredShips)
		{
			ShipsFiltered.Add(ship);
		}
	}

	[RelayCommand]
	public void SelectShip()
	{
		PickedShip = SelectedShip?.Ship;
	}
}
