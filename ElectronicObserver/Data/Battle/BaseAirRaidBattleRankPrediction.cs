using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;
using ElectronicObserverTypes.Mocks;

namespace ElectronicObserver.Data.Battle;

public class BaseAirRaidBattleRankPrediction : BattleRankPrediction
{
	public required IEnumerable<AirBaseBeforeAfter> AirBaseBeforeAfter { get; set; }

	protected override void CalculateFriendlyFleetHp()
	{
		foreach (AirBaseBeforeAfter airBase in AirBaseBeforeAfter)
		{
			if (airBase.Before?.HPCurrent is not int hpBefore) continue;
			if (airBase.After?.HPCurrent is not int hpAfter) continue;
			
			FriendlyHpBefore += hpBefore;
			FriendlyHpAfter += Math.Max(hpAfter, 0);
			FriendlyShipCount++;
		}
	}

	protected override void CalculateEnemyFleetHp()
	{
		CalculateEnemyFleetHp(EnemyMainFleetBefore, EnemyMainFleetAfter);

		if (EnemyEscortFleetBefore is not null && EnemyEscortFleetAfter is not null)
		{
			CalculateEnemyFleetHp(EnemyEscortFleetBefore, EnemyEscortFleetAfter);
		}
	}

	public static IEnumerable<AirBaseBeforeAfter> SimulateBaseAfterBattle(List<int> hpBefore, List<int> hpAfter)
	{
		int index = 0;
		int baseCount = hpBefore.Count(hp => hp != -1);

		return hpBefore.Take(baseCount).Zip(hpAfter.Take(baseCount), (before, after) =>
			new AirBaseBeforeAfter(index++,
				new BaseAirCorpsDataMock()
				{
					HPCurrent = before,
				}, new BaseAirCorpsDataMock()
				{
					HPCurrent = after,
				})
		);
	}

	/// <summary>
	/// No battle rank for air base raid
	/// TODO ? have special ranks for this (light damage/heavy/no damage)
	/// </summary>
	/// <returns></returns>
	protected override BattleRank GetWinRank() => BattleRank.Any;
}
