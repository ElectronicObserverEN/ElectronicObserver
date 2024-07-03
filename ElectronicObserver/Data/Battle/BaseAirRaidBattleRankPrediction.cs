﻿using System;
using System.Collections.Generic;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

namespace ElectronicObserver.Data.Battle;

public class BaseAirRaidBattleRankPrediction : BattleRankPrediction
{
	public required IEnumerable<AirBaseBeforeAfter> AirBaseBeforeAfter { get; set; }

	protected override void CalculateFriendlyFleetHp()
	{
		foreach (AirBaseBeforeAfter airBase in AirBaseBeforeAfter)
		{
			int? hpBefore = airBase.Before?.HPCurrent;
			if (hpBefore is null) continue;

			int? hpAfter = airBase.After?.HPCurrent;
			if (hpAfter is null) continue;

			FriendlyHpBefore += (int)hpBefore;
			FriendlyHpAfter += Math.Max((int)hpAfter, 0);
			FriendlyShipCount++;
		}
	}

	/// <summary>
	/// No battle rank for air base raid (TODO ? have special ranks for this (light damage/heavy/no damage))
	/// </summary>
	/// <returns></returns>
	protected override BattleRank GetWinRank() => BattleRank.Any;
}
