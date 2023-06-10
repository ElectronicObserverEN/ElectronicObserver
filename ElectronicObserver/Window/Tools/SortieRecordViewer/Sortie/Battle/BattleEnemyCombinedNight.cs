using System;
using System.Collections.Generic;
using ElectronicObserver.Data;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EcMidnightBattle;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

/// <summary>
/// 通常/連合艦隊 vs 連合艦隊 夜戦 <br />
/// api_req_combined_battle/ec_midnight_battle
/// </summary>
public sealed class BattleEnemyCombinedNight : SecondNightBattleData
{
	public override string Title => ConstantsRes.Title_EnemyCombinedNight;

	private static double FuelConsumption => 0.1;
	private static double AmmoConsumption => 0.1;

	public BattleEnemyCombinedNight(PhaseFactory phaseFactory, BattleFleets fleets, ApiReqCombinedBattleEcMidnightBattleResponse battle) 
		: base(phaseFactory, fleets, battle)
	{
		EmulateBattle();

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
