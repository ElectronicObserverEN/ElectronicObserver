using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Window.Dialog.ShipDataPicker;

public class ShipDataViewModel : ObservableObject
{
	public IShipData Ship { get; }

	public int NightPowerBase => Ship.FirepowerBase + Ship.TorpedoBase;
	public bool CanUseDaihatsu => Ship.MasterShip.EquippableCategoriesTyped.Contains(EquipmentTypes.LandingCraft);
	public bool CanUseTank => Ship.MasterShip.EquippableCategoriesTyped.Contains(EquipmentTypes.SpecialAmphibiousTank);
	public bool CanUseFcf => Ship.MasterShip.EquippableCategoriesTyped.Contains(EquipmentTypes.CommandFacility);
	public bool IsExpansionSlotAvailable => Ship.IsExpansionSlotAvailable;

	public ShipDataViewModel(IShipData ship)
	{
		Ship = ship;
	}
}
