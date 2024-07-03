using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;

namespace ElectronicObserver.Data.Battle;

public class AirRaidBattleRankPrediction : NormalBattleRankPrediction
{
	protected override void CalculateEnemyFleetHp()
	{
		// No need to calculate enemy HP for air raid
	}

	protected override BattleRank GetWinRank() => FriendHpRate switch
	{
		<= 0.0 => BattleRank.SS,
		< 0.1 => BattleRank.A,
		< 0.2 => BattleRank.B,
		< 0.5 => BattleRank.C,
		< 0.8 => BattleRank.D,
		_ => BattleRank.E,
	};
}
