﻿using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;
using ElectronicObserver.Window.Tools.SortieRecordViewer.SortieDetail;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Data;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

public abstract class BattleData
{
	public BattleFleets FleetsBeforeBattle => Initial.FleetsAfterPhase!;
	public BattleFleets FleetsAfterBattle { get; protected set; }

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

	protected PhaseInitial Initial { get; }
	protected PhaseSearching Searching { get; }
	protected PhaseFriendlySupportInfo FriendlySupportInfo { get; }
	protected PhaseSupport Support { get; }

	public abstract IEnumerable<PhaseBase> Phases { get; }

	protected BattleData(IKCDatabase kcDatabase, BattleFleets fleets, IBattleApiResponse battle)
	{
		Initial = new(kcDatabase, fleets, battle);
		Searching = battle switch
		{
			IDayBattleApiResponse d => new(d),
			_ => new(battle),
		};
		FriendlySupportInfo = new();
		Support = new();
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