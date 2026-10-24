namespace ElectronicObserver.Avalonia.QuestTrackerManager2;

public class QuestTrackerProgressRecordV2(int questId, IEnumerable<int> progresses)
{
	public int QuestId { get; init; } = questId;
	public IEnumerable<int> Progresses { get; init; } = progresses;
}
