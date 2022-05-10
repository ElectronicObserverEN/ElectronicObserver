using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.EventLockPlanner;

public class ShipLockViewModel : ObservableObject
{
	public IShipData Ship { get; }

	public int PlannedLock { get; set; }
	public int ActualLock => Ship.SallyArea;

	public string Display => ActualLock switch
	{
		< 1 => Ship.Name,
		_ => $"[{ActualLock}] {Ship.Name}"
	};

	public int NightPowerBase => Ship.FirepowerBase + Ship.TorpedoBase;
	public bool CanUseDaihatsu => Ship.MasterShip.EquippableCategoriesTyped.Contains(EquipmentTypes.LandingCraft);
	public bool CanUseTank => Ship.MasterShip.EquippableCategoriesTyped.Contains(EquipmentTypes.SpecialAmphibiousTank);

	public ShipLockViewModel(IShipData ship)
	{
		Ship = ship;
	}
}