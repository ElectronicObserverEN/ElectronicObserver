using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Core.Types;
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
	public bool IsCombined => IsFriendCombined || IsEnemyCombined;
	public bool IsBaseAirRaid => this is BattleBaseAirRaid;
	public bool IsRadar => this is BattleNormalRadar or BattleCombinedRadar;
	public bool IsAirRaid => this is BattleAirRaid or BattleCombinedAirRaid;

	public IEnumerable<int> InitialHPs => GetHpList(FleetsBeforeBattle.Fleet)
		.Concat(GetHpList(FleetsBeforeBattle.EscortFleet))
		.Take(12)
		.Concat(GetHpList(FleetsBeforeBattle.EnemyFleet))
		.Concat(GetHpList(FleetsBeforeBattle.EnemyEscortFleet));

	public IEnumerable<int> ResultHPs => GetHpList(FleetsAfterBattle.Fleet)
		.Concat(GetHpList(FleetsAfterBattle.EscortFleet))
		.Take(12)
		.Concat(GetHpList(FleetsAfterBattle.EnemyFleet))
		.Concat(GetHpList(FleetsAfterBattle.EnemyEscortFleet));

	private static IEnumerable<int> GetHpList(IFleetData? fleet) =>
		fleet?.MembersInstance
			.Select(s => s?.HPCurrent ?? 0)
			.Concat(Enumerable.Repeat(0, 6))
			.Take(Math.Max(fleet.MembersInstance.Count, 6))
		?? Enumerable.Repeat(0, 6);

	public IEnumerable<int> MvpShipIndexes()
	{
		if (FleetsBeforeBattle.Fleet.Members is null) return [0];

		List<(int? Index, int Damage)> damages = Phases
			.OfType<AttackPhaseBase>()
			.SelectMany(p => p.AttackDisplays)
			.Where(a => a.AttackerIndex?.FleetFlag is FleetFlag.Player)
			.Where(a => a.AttackerIndex?.Index < FleetsBeforeBattle.Fleet.Members.Count)
			.GroupBy(a => a.AttackerIndex?.Index)
			.Select(g => (g.Key, (int)g.Sum(a => a.Damage)))
			.ToList();

		int max = damages.Select(d => d.Damage).Max();

		if (max is 0) return [0];

		return damages
			.Where(d => d.Damage == max)
			.Select(d => d.Index)
			.OfType<int>();
	}

	public IEnumerable<int> MvpShipCombinedIndexes()
	{
		if (FleetsBeforeBattle.EscortFleet?.Members is null) return [0];

		List<(int? Index, int Damage)> damages = Phases
			.OfType<AttackPhaseBase>()
			.SelectMany(p => p.AttackDisplays)
			.Where(a => a.AttackerIndex?.FleetFlag is FleetFlag.Player)
			.Where(a => a.AttackerIndex?.Index > 5)
			.GroupBy(a => a.AttackerIndex?.Index)
			.Select(g => (g.Key, (int)g.Sum(a => a.Damage)))
			.ToList();

		int max = damages.Select(d => d.Damage).Max();

		if (max is 0) return [0];

		return damages
			.Where(d => d.Damage == max)
			.Select(d => d.Index - 6)
			.OfType<int>();
	}

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
	public List<int> AttackDamages { get; } = [.. Enumerable.Repeat(0, 24)];

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
			FleetsAfterBattle = phase.EmulateBattle(FleetsAfterBattle, AttackDamages);
		}
	}

	public string GetBattleDetail(int index) => string.Join("\r\n", Phases
		.OfType<AttackPhaseBase>()
		.Select(p => p.GetBattleDetail(index) switch
		{
			string detail => $"""
				== {p.Title} ==
				{detail}
				""",

			_ => null,
		})
		.Where(d => !string.IsNullOrEmpty(d)));
}
