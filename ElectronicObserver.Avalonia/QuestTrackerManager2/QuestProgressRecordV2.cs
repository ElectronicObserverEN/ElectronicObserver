namespace ElectronicObserver.Avalonia.QuestTrackerManager2;

/// <summary>
/// Progress is persisted separately from <see cref="TrackerModelV2"/> so tracker definitions
/// (map/rank/count/conditions) stay shareable without per-user progress mixed in.
/// </summary>
public class QuestProgressRecordV2(DateTime lastQuestListUpdate, IEnumerable<QuestTrackerProgressRecordV2> trackerProgresses)
{
	public DateTime LastQuestListUpdate { get; init; } = lastQuestListUpdate;
	public IEnumerable<QuestTrackerProgressRecordV2> TrackerProgresses { get; init; } = trackerProgresses;
}
