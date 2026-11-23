using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Avalonia.UiBlocker;

public interface IGameStateProvider
{
	bool CanShipSink { get; }
	bool CanDoDoubleLineTouch { get; }
	bool CanDoEchelonTouch { get; }
	bool CanDoSecondPatrolFormationTouch { get; }
	bool CanDoFourthPatrolFormationTouch { get; }
	bool IsBattleNode { get; }
	bool IsBoss { get; }
	bool IsEvent { get; }
	bool IsCombinedFleet { get; }
	BattleRank CurrentRank { get; }
	IMapInfoData? Map { get; }
	int? CellId { get; }
}
