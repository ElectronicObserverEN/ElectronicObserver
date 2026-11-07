using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2;

public class TrackerModelV2(
	int questId,
	List<ITaskModel?>? tasks = null,
	FleetConditionSet? conditions = null) : ObservableObject
{
	public int QuestId { get; set; } = questId;
	public List<ITaskModel?> Tasks { get; set; } = tasks ?? [];
	public FleetConditionSet Conditions { get; set; } = conditions ?? new();
}
