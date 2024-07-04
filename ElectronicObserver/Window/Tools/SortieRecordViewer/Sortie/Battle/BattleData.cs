using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

public abstract class BattleData
{
	public abstract string Title { get; }
	public DateTime? TimeStamp { get; set; }

	protected PhaseFactory PhaseFactory { get; }

	public BattleFleets FleetsBeforeBattle => Initial.FleetsAfterPhase!;
	public BattleFleets FleetsAfterBattle { get; private set; }

	public bool IsPractice => this is BattlePracticeDay or BattlePracticeNight;
	public bool IsFriendCombined => FleetsBeforeBattle.EscortFleet is not null;
	public bool IsEnemyCombined => FleetsBeforeBattle.EnemyEscortFleet is not null;
	public bool IsBaseAirRaid => this is BattleBaseAirRaid;

	public IEnumerable<int> ResultHPs => FleetsAfterBattle
		.SortieShips()
		.Select(s => s?.HPCurrent ?? 0);

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

	public PhaseInitial Initial { get; }

	public IEnumerable<PhaseBase> Phases => AllPhases().OfType<PhaseBase>();

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
}
