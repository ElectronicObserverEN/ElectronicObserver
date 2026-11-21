using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks;

namespace ElectronicObserver.Avalonia.UiBlocker;

public sealed class ConditionModel
{
	public ConditionType Type { get; set; }
	public bool Invert { get; set; }

	public BattleRank? Rank { get; set; }
	public MapInfoModel? Map { get; set; }
	public List<int> CellIds { get; set; } = [];

	public ConditionModel Clone()
	{
		ConditionModel copy = (ConditionModel)MemberwiseClone();
		copy.CellIds = [.. CellIds];
		return copy;
	}
}
