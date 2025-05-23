using ElectronicObserver.Core.Types;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;

namespace ElectronicObserver.Data.Battle;

public class NormalBattleRankPrediction : BattleRankPrediction
{
	public required IFleetData FriendlyMainFleetBefore { get; init; }
	public required IFleetData FriendlyMainFleetAfter { get; init; }

	public IFleetData? FriendlyEscortFleetBefore { get; init; }
	public IFleetData? FriendlyEscortFleetAfter { get; init; }

	protected override void CalculateFriendlyFleetHp()
	{
		CalculeFriendlyFleetHp(FriendlyMainFleetBefore, FriendlyMainFleetAfter);

		if (FriendlyEscortFleetBefore is not null && FriendlyEscortFleetAfter is not null)
		{
			CalculeFriendlyFleetHp(FriendlyEscortFleetBefore, FriendlyEscortFleetAfter);
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

	protected override BattleRank GetWinRank()
	{
		int rifriend = (int)(FriendHpRate * 100);
		int rienemy = (int)(EnemyHpRate * 100);

		if (FriendlyShipSunk == 0)
		{
			if (EnemyShipSunk == EnemyShipCount)
			{
				return FriendHpRate switch
				{
					<= 0 => BattleRank.SS,
					_ => BattleRank.S,
				};
			}
			
			if (EnemyShipCount > 1 && EnemyShipSunk >= (int)(EnemyShipCount * 0.7))
			{
				return BattleRank.A;
			}
		}

		bool defeatEnemyFlagship = EnemyMainFleetAfter.MembersInstance[0]?.HPCurrent <= 0;

		if (defeatEnemyFlagship && FriendlyShipSunk < EnemyShipSunk)
			return BattleRank.B;

		bool isfriendFlagshipHeavilyDamaged = FriendlyMainFleetAfter.MembersInstance[0]?.HPRate <= 0.25;

		if (FriendlyShipCount == 1 && isfriendFlagshipHeavilyDamaged)
			return BattleRank.D;

		if (rienemy > (2.5 * rifriend))
			return BattleRank.B;

		if (rienemy > (0.9 * rifriend))
			return BattleRank.C;

		return FriendlyShipSunk switch
		{
			> 0 when (FriendlyShipCount - FriendlyShipSunk) == 1 => BattleRank.E,
			_ => BattleRank.D,
		};
	}
}
