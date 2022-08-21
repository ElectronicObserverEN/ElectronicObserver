using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Data;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.ViewModels;
using ElectronicObserver.Window.Dialog.ShipPicker;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Mocks;
using ElectronicObserverTypes.Serialization.FitBonus;

namespace ElectronicObserver.Window.Tools.FitBonusViewer;

public partial class FitBonusShipViewModel
{
	public ObservableCollection<FitBonusResult> FitBonusResults { get; set; } = new();

	public FitBonusValue FitBonusTotal { get; set; } = new();

	private IShipData? ship;
	public IShipData? Ship
	{
		get { return ship; }
		set
		{
			ship = value;

			if (ship != null)
			{
				EquipmentList.Clear();

				foreach (IEquipmentData? equipment in ship.SlotInstance)
				{
					EquipmentList.Add(new FitBonusEquipmentViewModel(equipment));
				}
			}

			RecomputeBonuses();
		}
	}

	private ShipPickerViewModel ShipPickerViewModel { get; }

	public ObservableCollection<FitBonusEquipmentViewModel> EquipmentList { get; set; } = new();

	public FitBonusShipViewModel(IShipData? ship)
	{
		Ship = ship;
		ShipPickerViewModel = Ioc.Default.GetService<ShipPickerViewModel>()!;

		EquipmentList.CollectionChanged += (_, _) => RecomputeBonuses(); 
	}

	private void RecomputeBonuses()
	{
		FitBonusResults.Clear();

		if (ship is null)
		{
			FitBonusTotal = new FitBonusValue();
			return;
		}

		foreach (FitBonusResult result in ship.GetFitBonusList(KCDatabase.Instance.Translation.FitBonus.FitBonusList))
		{
			FitBonusResults.Add(result);
		}

		FitBonusTotal = ship.GetFitBonus(KCDatabase.Instance.Translation.FitBonus.FitBonusList);
	}

	[ICommand]
	private void OpenShipPicker()
	{
		ShipPickerView shipPicker = new(ShipPickerViewModel);

		if (shipPicker.ShowDialog(App.Current.MainWindow) is true)
		{
			Ship = shipPicker.PickedShip switch
			{
				null => null,
				_ => new ShipDataMock(KCDatabase.Instance.MasterShips[shipPicker.PickedShip.ShipID])
			};	
		}
	}
}
