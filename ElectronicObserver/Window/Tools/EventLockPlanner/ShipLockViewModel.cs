using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Window.Tools.EventLockPlanner;

public class ShipLockViewModel : ObservableObject
{
	public IShipData Ship { get; }

	public int PlannedLock { get; set; }
	public int ActualLock => Ship.SallyArea;
	public bool MatchesPhaseLock { get; set; } = true;

	public string Display => ActualLock switch
	{
		< 1 => Ship.NameWithLevel,
		_ => $"[{ActualLock}] {Ship.NameWithLevel}"
	};

	public int NightPowerBase => Ship.FirepowerBase + Ship.TorpedoBase;
	public bool CanUseDaihatsu => Ship.MasterShip.EquippableCategoriesTyped.Contains(EquipmentTypes.LandingCraft);
	public bool CanUseTank => Ship.MasterShip.EquippableCategoriesTyped.Contains(EquipmentTypes.SpecialAmphibiousTank);
	public bool CanUseFcf => Ship.MasterShip.EquippableCategoriesTyped.Contains(EquipmentTypes.CommandFacility);
	public bool IsExpansionSlotAvailable => Ship.IsExpansionSlotAvailable;

	public ShipLockViewModel(IShipData ship)
	{
		Ship = ship;
	}
}
