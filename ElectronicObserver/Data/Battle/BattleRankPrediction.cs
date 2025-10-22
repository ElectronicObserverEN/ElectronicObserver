using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;
using ElectronicObserver.Window.Tools.SortieRecordViewer;

namespace ElectronicObserver.Data.Battle;

public abstract class BattleRankPrediction
{
	public required IFleetData EnemyMainFleetBefore { get; init; }
	public required IFleetData EnemyMainFleetAfter { get; init; }

	public IFleetData? EnemyEscortFleetBefore { get; init; }
	public IFleetData? EnemyEscortFleetAfter { get; init; }
	
	public int FriendlyShipCount { get; protected set; }
	public int FriendlyShipSunk { get; protected set; }
	public int FriendlyHpBefore { get; protected set; }
	public int FriendlyHpAfter { get; protected set; }
	public double FriendHpRate => (double)(FriendlyHpBefore - FriendlyHpAfter) / FriendlyHpBefore;

	public int EnemyShipCount { get; private set; }
	public int EnemyShipSunk { get; private set; }
	public int EnemyHpBefore { get; private set; }
	public int EnemyHpAfter { get; private set; }
	public double EnemyHpRate => (double)(EnemyHpBefore - EnemyHpAfter) / EnemyHpBefore;

	public BattleRank PredictRank()
	{
		ResetValues();

		CalculateFriendlyFleetHp();

		CalculateEnemyFleetHp();

		return GetWinRank();
	}

	protected abstract void CalculateFriendlyFleetHp();
	protected abstract void CalculateEnemyFleetHp();
	protected abstract BattleRank GetWinRank();

	protected void CalculeFriendlyFleetHp(IFleetData fleetBefore, IFleetData fleetAfter)
	{
		if (fleetBefore.MembersWithoutEscaped is null) return;
		if (fleetAfter.MembersWithoutEscaped is null) return;

		for (int index = 0; index < fleetBefore.MembersWithoutEscaped.Count; index++)
		{
			int? hpBefore = fleetBefore.MembersWithoutEscaped[index]?.HPCurrent;
			if (hpBefore is null or < 0) continue;

			int? hpAfter = fleetAfter.MembersWithoutEscaped[index]?.HPCurrent;
			if (hpAfter is null) continue;

			FriendlyHpBefore += (int)hpBefore;
			FriendlyHpAfter += Math.Max((int)hpAfter, 0);
			FriendlyShipCount++;

			if (hpAfter <= 0)
			{
				FriendlyShipSunk++;
			}
		}
	}

	protected void CalculateEnemyFleetHp(IFleetData fleetBefore, IFleetData fleetAfter)
	{
		for (int index = 0; index < fleetBefore.MembersInstance.Count; index++)
		{
			IShipData? ship = fleetBefore.MembersInstance[index];

			if (ship is null) continue;
			if (!ship.CanBeTargeted) continue;

			int hpBefore = ship.HPCurrent;
			if (hpBefore < 0) continue;

			int? hpAfter = fleetAfter.MembersInstance[index]?.HPCurrent;
			if (hpAfter is null) continue;

			EnemyHpBefore += hpBefore;
			EnemyHpAfter += Math.Max((int)hpAfter, 0);
			EnemyShipCount++;

			if (hpAfter <= 0)
			{
				EnemyShipSunk++;
			}
		}
	}

	private void ResetValues()
	{
		FriendlyShipCount = 0;
		FriendlyShipSunk = 0;
		FriendlyHpBefore = 0;
		FriendlyHpAfter = 0;

		EnemyShipCount = 0;
		EnemyShipSunk = 0;
		EnemyHpBefore = 0;
		EnemyHpAfter = 0;
	}

	/// <summary>
	/// Create a copy of the fleet with their HP after the battle
	/// </summary>
	public static IFleetData? SimulateFleetAfterBattle(IFleetData? fleet, List<int> resultHp, BattleSides battleSide)
	{
		if (fleet is null) return null;

		int offset = BattleIndex.Get(battleSide, 0);
		List<int> resultHpWithOffset = resultHp.Skip(offset).Take(fleet.MembersInstance.Count).ToList();

		return SimulateFleetAfterBattle(fleet, resultHpWithOffset);
	}

	/// <summary>
	/// Create a copy of the fleet with their HP after the battle
	/// </summary>
	public static IFleetData SimulateFleetAfterBattle(IFleetData fleet, List<int> resultHp)
	{
		IFleetData fleetClone = fleet.DeepClone();

		if (fleetClone.Members is null) return fleetClone;
		if (resultHp.Count < fleetClone.Members.Count) return fleetClone;

		for (int index = 0; index < fleetClone.Members.Count; index++)
		{
			if (fleetClone.MembersInstance[index] is {} ship)
			{
				ship.HPCurrent = resultHp[index];
			}
		}

		return fleetClone;
	}
}
