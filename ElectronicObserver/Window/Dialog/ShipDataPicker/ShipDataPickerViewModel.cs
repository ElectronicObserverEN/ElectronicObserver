using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Data;
using ElectronicObserver.Window.Tools.EventLockPlanner;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Dialog.ShipDataPicker;

public partial class ShipDataPickerViewModel : WindowViewModelBase
{
	private List<IShipData> AllShips { get; }

	public ObservableCollection<IShipData> ShipsFiltered { get; set; } = new();

	public ShipFilterViewModel Filters { get; } = new();

	public IShipData? PickedShip { get; private set; }

	public ShipDataPickerTranslationViewModel ShipDataPicker { get; set; } = new();

	public ShipDataPickerViewModel()
	{
		AllShips = KCDatabase.Instance.Ships
			.Values
			.Cast<IShipData>()
			.ToList();

		Filters.PropertyChanged += ReloadShips;
	}

	[RelayCommand]
	private void SelectShip(IShipData? ship)
	{
		PickedShip = ship;
	}

	private void ReloadShips(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		ShipsFiltered.Clear();

		List<IShipData> filteredShips = AllShips.Where(Filters.MeetsFilterCondition).ToList();

		foreach (IShipData ship in filteredShips)
		{
			ShipsFiltered.Add(ship);
		}
	}
}
