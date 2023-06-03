using System;
using System.Collections.Generic;
using ElectronicObserver.Data;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EcMidnightBattle;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Data;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

public sealed class BattleEnemyCombinedNight : BattleNight
{
	public override string Title => ConstantsRes.Title_EnemyCombinedNight;

	private static double FuelConsumption => 0.1;
	private static double AmmoConsumption => 0.1;

	public BattleEnemyCombinedNight(IKCDatabase kcDatabase, BattleFleets fleets, ApiReqCombinedBattleEcMidnightBattleResponse battle) 
		: base(kcDatabase, fleets, battle)
	{
		foreach (PhaseBase phase in Phases)
		{
			FleetsAfterBattle = phase.EmulateBattle(FleetsAfterBattle);
		}

		foreach (IShipData? ship in FleetsAfterBattle.Fleet.MembersWithoutEscaped!)
		{
			if (ship is null) continue;

			ship.Fuel = Math.Max(0, ship.Fuel - Math.Max(1, (int)Math.Floor(ship.FuelMax * FuelConsumption)));
			ship.Ammo = Math.Max(0, ship.Ammo - Math.Max(1, (int)Math.Floor(ship.AmmoMax * AmmoConsumption)));
		}
	}

	protected override IEnumerable<PhaseBase?> AllPhases()
	{
		yield return Initial;
		yield return NightInitial;
		yield return FriendlySupportInfo;
		yield return FriendlyShelling;
		yield return NightBattle;
	}
}
