using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;
using ElectronicObserver.Window.Tools.SortieRecordViewer.SortieDetail;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

public abstract class BattleData
{
	public abstract string Title { get; }

	protected PhaseFactory PhaseFactory { get; }

	public BattleFleets FleetsBeforeBattle => Initial.FleetsAfterPhase!;
	public BattleFleets FleetsAfterBattle { get; protected set; }

	public IEnumerable<AirBaseBeforeAfter> AirBaseBeforeAfter => FleetsBeforeBattle.AirBases
		.Zip(FleetsAfterBattle.AirBases, (before, after) => (Before: before, After: after))
		.Select((t, i) => new AirBaseBeforeAfter(i, t.Before, t.After));

	public IEnumerable<ShipBeforeAfter> MainFleetBeforeAfter => FleetsBeforeBattle.Fleet.MembersInstance
		.Zip(FleetsAfterBattle.Fleet.MembersInstance, (before, after) => (Before: before, After: after))
		.Select((t, i) => new ShipBeforeAfter(i, t.Before, t.After));

	public IEnumerable<ShipBeforeAfter>? EscortFleetBeforeAfter => FleetsBeforeBattle.EscortFleet?.MembersInstance
		.Zip(FleetsAfterBattle.EscortFleet!.MembersInstance, (before, after) => (Before: before, After: after))
		.Select((t, i) => new ShipBeforeAfter(i, t.Before, t.After));

	public IEnumerable<ShipBeforeAfter>? EnemyMainFleetBeforeAfter => FleetsBeforeBattle.EnemyFleet?.MembersInstance
		.Zip(FleetsAfterBattle.EnemyFleet!.MembersInstance, (before, after) => (Before: before, After: after))
		.Select((t, i) => new ShipBeforeAfter(i, t.Before, t.After));

	public IEnumerable<ShipBeforeAfter>? EnemyEscortFleetBeforeAfter => FleetsBeforeBattle.EnemyEscortFleet?.MembersInstance
		.Zip(FleetsAfterBattle.EnemyEscortFleet!.MembersInstance, (before, after) => (Before: before, After: after))
		.Select((t, i) => new ShipBeforeAfter(i, t.Before, t.After));

	public IEnumerable<SortieCost> MainFleetRepairCosts => MainFleetBeforeAfter
		.Select(ship => RepairCost(ship.Before, ship.After));

	public SortieCost TotalRepairCost => MainFleetRepairCosts
		.Aggregate(new SortieCost(), (a, b) => a + b);

	public IEnumerable<SortieCost> MainFleetSupplyCosts => MainFleetBeforeAfter
		.Select(ship => SupplyCost(ship.Before, ship.After));

	public SortieCost TotalSupplyCost => MainFleetSupplyCosts
		.Aggregate(new SortieCost(), (a, b) => a + b);

	public PhaseInitial Initial { get; }

	public IEnumerable<PhaseBase> Phases => AllPhases().Where(p => p is not null)!;

	protected BattleData(PhaseFactory phaseFactory, BattleFleets fleets, IBattleApiResponse battle)
	{
		PhaseFactory = phaseFactory;

		Initial = PhaseFactory.Initial(fleets, battle);
	}

	protected abstract IEnumerable<PhaseBase?> AllPhases();

	protected void EmulateBattle()
	{
		foreach (PhaseBase phase in Phases)
		{
			FleetsAfterBattle = phase.EmulateBattle(FleetsAfterBattle);
		}
	}

	private static SortieCost RepairCost(IShipData? before, IShipData? after) => (before, after) switch
	{
		({ }, { }) => RepairCost(before, before.HPCurrent - after.HPCurrent),

		_ => new(),
	};

	private static SortieCost RepairCost(IShipData ship, int damage) => new()
	{
		Fuel = (int)(ship.MasterShip.Fuel * 0.032 * damage),
		Steel = (int)(ship.MasterShip.Fuel * 0.06 * damage),
	};

	private static SortieCost SupplyCost(IShipData? before, IShipData? after) => (before, after) switch
	{
		({ }, { }) => new()
		{
			Fuel = before.Fuel - after.Fuel,
			Ammo = before.Ammo - after.Ammo,
		},

		_ => new(),
	};
}
