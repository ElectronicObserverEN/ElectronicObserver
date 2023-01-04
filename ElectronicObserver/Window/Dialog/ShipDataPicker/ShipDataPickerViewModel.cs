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
	private List<ShipDataViewModel> AllShips { get; }

	public ObservableCollection<ShipDataViewModel> ShipsFiltered { get; set; } = new();

	public ShipFilterViewModel Filters { get; } = new();

	public IShipData? PickedShip { get; private set; }

	public ShipDataPickerTranslationViewModel ShipDataPicker { get; set; } = new();

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
}
