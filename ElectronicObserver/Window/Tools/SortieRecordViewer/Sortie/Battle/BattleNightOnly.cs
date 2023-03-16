using System;
using System.Collections.Generic;
using ElectronicObserver.KancolleApi.Types.ApiReqBattleMidnight.SpMidnight;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;
using ElectronicObserverTypes.Data;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

public class BattleNightOnly : BattleNight
{
	private static double FuelConsumption => 0.1;
	private static double AmmoConsumption => 0.1;

	public BattleNightOnly(IKCDatabase kcDatabase, BattleFleets fleets, ApiReqBattleMidnightSpMidnightResponse battle) : base(kcDatabase, fleets, battle)
	{
		foreach (var phase in GetPhases())
		{
			FleetsAfterBattle = phase.EmulateBattle(FleetsAfterBattle);
		}

		foreach (var ship in FleetsAfterBattle.Fleet.MembersWithoutEscaped!)
		{
			if (ship is null) continue;

			ship.Fuel = Math.Max(0, ship.Fuel - Math.Max(1, (int)Math.Floor(ship.FuelMax * FuelConsumption)));
			ship.Ammo = Math.Max(0, ship.Ammo - Math.Max(1, (int)Math.Floor(ship.AmmoMax * AmmoConsumption)));
		}
	}

	public override IEnumerable<PhaseBase> Phases => GetPhases();

	private IEnumerable<PhaseBase> GetPhases()
	{
		yield return Initial;
		yield return Searching;
		yield return NightInitial;
		yield return FriendlySupportInfo;
		yield return FriendlyShelling;
		yield return Support;
		yield return NightBattle;
	}
}
