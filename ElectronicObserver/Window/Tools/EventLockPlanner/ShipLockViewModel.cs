using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Data;

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

	public string TooltipDisplay => string.Format(
			EventLockPlannerResources.ShipTooltip,
			Ship.SallyArea > 0 ? $"[{Ship.SallyArea}] " : "",
			Ship.MasterShip.ShipTypeName, Ship.NameWithLevel,
			Ship.FirepowerBase, Ship.FirepowerTotal,
			Ship.TorpedoBase, Ship.TorpedoTotal,
			Ship.AABase, Ship.AATotal,
			Ship.ArmorBase, Ship.ArmorTotal,
			Ship.ASWBase, Ship.ASWTotal,
			Ship.EvasionBase, Ship.EvasionTotal,
			Ship.LOSBase, Ship.LOSTotal,
			Ship.LuckTotal,
			Ship.ID,
			Constants.GetRange(Ship.Range),
			Constants.GetSpeed(Ship.Speed)
		);

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
