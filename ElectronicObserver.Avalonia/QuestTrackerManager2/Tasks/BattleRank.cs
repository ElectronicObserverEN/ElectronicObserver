using System.ComponentModel.DataAnnotations;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks;

public enum BattleRank
{
	[Display(ResourceType = typeof(QuestTrackerManagerResources), Name = "BattleRank_Any")]
	Any = 0,
	E = 1,
	D = 2,
	C = 3,
	B = 4,
	A = 5,
	S = 6,
	SS = 7,
}
