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
	private List<ShipDataViewModel> AllShips { get; }

	public ObservableCollection<ShipDataViewModel> ShipsFiltered { get; } = new();

	public ShipFilterViewModel Filters { get; } = new();

	public IShipData? PickedShip { get; private set; }
	public ShipDataViewModel? SelectedShip { get; set; }

	public ShipDataPickerTranslationViewModel ShipDataPicker { get; } = new();

	public ShipDataPickerViewModel()
	{
		AllShips = KCDatabase.Instance.Ships
			.Values
			.Cast<IShipData>()
			.Select(ship => new ShipDataViewModel(ship))
			.ToList();

		Filters.PropertyChanged += (_, _) => ReloadShips();

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
