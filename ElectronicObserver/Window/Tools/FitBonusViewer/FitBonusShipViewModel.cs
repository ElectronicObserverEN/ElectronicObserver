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

		List<FitBonusResult> fitBonuses = new (FitBonusResults);

		foreach (FitBonusEquipmentViewModel equipment in EquipmentList)
		{
			equipment.FitBonuses.Clear();

			if (equipment.SelectedEquipment is null) continue;

			List<FitBonusResult> bonusesForThisEquipment = fitBonuses.FindAll(bonus => bonus.EquipmentTypes.Contains(equipment.SelectedEquipment.MasterEquipment.CategoryType) || bonus.EquipmentIds.Contains(equipment.SelectedEquipment.MasterEquipment.EquipmentId));

			foreach (FitBonusResult result in bonusesForThisEquipment)
			{
				fitBonuses.Remove(result);
				equipment.FitBonuses.Add(result);
			}
		}
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

	[ICommand]
	private void AddEquipment()
	{
		FitBonusEquipmentViewModel vm = new FitBonusEquipmentViewModel(null);

		EquipmentList.Add(vm);

		vm.OpenEquipmentPickerCommand.Execute(null);
	}
}
