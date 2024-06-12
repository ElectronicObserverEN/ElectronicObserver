using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;
using ElectronicObserver.Window.Tools.SortieRecordViewer;
using ElectronicObserverTypes;

namespace ElectronicObserver.Data.Battle;

public class BattleRankPrediction
{
	public required IFleetData FriendlyMainFleetBefore { get; init; }
	public required IFleetData FriendlyMainFleetAfter { get; init; }

	public IFleetData? FriendlyEscortFleetBefore { get; init; }
	public IFleetData? FriendlyEscortFleetAfter { get; init; }

	public required IFleetData EnemyMainFleetBefore { get; init; }
	public required IFleetData EnemyMainFleetAfter { get; init; }

	public IFleetData? EnemyEscortFleetBefore { get; init; }
	public IFleetData? EnemyEscortFleetAfter { get; init; }
	
	public int FriendlyShipCount { get; private set; }
	public int FriendlyShipSunk { get; private set; }
	public int FriendlyHpBefore { get; private set; }
	public int FriendlyHpAfter { get; private set; }
	public double FriendHpRate => (double)(FriendlyHpBefore - FriendlyHpAfter) / FriendlyHpBefore;

	public int EnemyShipCount { get; private set; }
	public int EnemyShipSunk { get; private set; }
	public int EnemyHpBefore { get; private set; }
	public int EnemyHpAfter { get; private set; }
	public double EnemyHpRate => (double)(EnemyHpBefore - EnemyHpAfter) / EnemyHpBefore;

	public BattleRank PredictRank()
	{
		ResetValues();

		CalculeFriendlyMainFleetHp();
		CalculeFriendlyEscortFleetHp();

		CalculateEnemyMainFleetHp();
		CalculateEnemyEscortFleetHp();

		return GetWinRank();
	}

	public BattleRank PredictRankAirRaid()
	{
		ResetValues();
		CalculeFriendlyMainFleetHp();
		CalculeFriendlyEscortFleetHp();

		double friendrate = (double)(FriendlyHpBefore - FriendlyHpAfter) / FriendlyHpBefore;

		return GetWinRankAirRaid(friendrate);
	}
	
	private void CalculeFriendlyMainFleetHp()
	{
		for (int index = 0; index < FriendlyMainFleetBefore.MembersInstance.Count; index++)
		{
			int? hpBefore = FriendlyMainFleetBefore.MembersInstance[index]?.HPCurrent;
			if (hpBefore is null or < 0) continue;

			int? hpAfter = FriendlyMainFleetAfter.MembersInstance[index]?.HPCurrent;
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

	private void CalculeFriendlyEscortFleetHp()
	{
		if (FriendlyEscortFleetBefore is null) return;
		if (FriendlyEscortFleetAfter is null) return;

		for (int index = 0; index < FriendlyEscortFleetBefore.MembersInstance.Count; index++)
		{
			int? hpBefore = FriendlyEscortFleetBefore.MembersInstance[index]?.HPCurrent;
			if (hpBefore is null or < 0) continue;

			int? hpAfter = FriendlyEscortFleetAfter.MembersInstance[index]?.HPCurrent;
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

	private void CalculateEnemyMainFleetHp()
	{
		for (int index = 0; index < EnemyMainFleetBefore.MembersInstance.Count; index++)
		{
			IShipData? ship = EnemyMainFleetBefore.MembersInstance[index];

			if (ship is null) continue;
			if (!ship.CanBeTargeted) continue;

			int hpBefore = ship.HPCurrent;
			if (hpBefore < 0) continue;

			int? hpAfter = EnemyMainFleetAfter.MembersInstance[index]?.HPCurrent;
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

	private void CalculateEnemyEscortFleetHp()
	{
		if (EnemyEscortFleetBefore is null) return;
		if (EnemyEscortFleetAfter is null) return;

		for (int index = 0; index < EnemyEscortFleetBefore.MembersInstance.Count; index++)
		{
			IShipData? ship = EnemyEscortFleetBefore.MembersInstance[index];

			if (ship is null) continue;
			if (!ship.CanBeTargeted) continue;

			int hpBefore = ship.HPCurrent;
			if (hpBefore < 0) continue;

			int? hpAfter = EnemyEscortFleetAfter.MembersInstance[index]?.HPCurrent;
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
	/// <param name="fleet"></param>
	/// <param name="resultHp"></param>
	/// <param name="battleSide"></param>
	/// <returns></returns>
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
	/// <param name="fleet"></param>
	/// <param name="resultHp"></param>
	/// <returns></returns>
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
	
	private BattleRank GetWinRank()
	{
		int rifriend = (int)(FriendHpRate * 100);
		int rienemy = (int)(EnemyHpRate * 100);

		// 轟沈艦なし
		if (FriendlyShipSunk == 0)
		{
			// 敵艦全撃沈
			if (EnemyShipSunk == EnemyShipCount)
			{
				return FriendHpRate switch
				{
					<= 0 => BattleRank.SS,
					_ => BattleRank.S,
				};
			}
			
			if (EnemyShipCount > 1 && EnemyShipSunk >= (int)(EnemyShipCount * 0.7)) // 敵の 70% 以上を撃沈
			{
				return BattleRank.A;   // A
			}
		}

		bool defeatEnemyFlagship = EnemyMainFleetAfter.MembersInstance[0]?.HPCurrent <= 0;

		// 敵旗艦撃沈 かつ 轟沈艦が敵より少ない
		if (defeatEnemyFlagship && FriendlyShipSunk < EnemyShipSunk)
			return BattleRank.B;   // B

		bool isfriendFlagshipHeavilyDamaged = FriendlyMainFleetAfter.MembersInstance[0]?.HPRate <= 0.25;

		// 自艦隊1隻 かつ 旗艦大破
		if (FriendlyShipCount == 1 && isfriendFlagshipHeavilyDamaged)
			return BattleRank.D;   // D

		// ゲージが 2.5 倍以上
		if (rienemy > (2.5 * rifriend))
			return BattleRank.B;   // B

		// ゲージが 0.9 倍以上
		if (rienemy > (0.9 * rifriend))
			return BattleRank.C;   // C

		return FriendlyShipSunk switch
		{
			// 轟沈艦あり かつ 残った艦が１隻のみ
			> 0 when (FriendlyShipCount - FriendlyShipSunk) == 1 => BattleRank.E,
			_ => BattleRank.D,
		};

		// 残りはD
	}

	/// <summary>
	/// 空襲戦における勝利ランクを計算します。
	/// </summary>
	/// <param name="friendrate">自軍損害率。</param>
	private static BattleRank GetWinRankAirRaid(double friendrate)
	{
		return friendrate switch
		{
			<= 0.0 => BattleRank.SS,
			< 0.1 => BattleRank.A,
			< 0.2 => BattleRank.B,
			< 0.5 => BattleRank.C,
			< 0.8 => BattleRank.D,
			_ => BattleRank.E,
		};
	}
}
