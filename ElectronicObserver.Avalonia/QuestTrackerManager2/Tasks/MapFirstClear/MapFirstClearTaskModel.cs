namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.MapFirstClear;

public class MapFirstClearTaskModel : ITaskModel
{
	public MapInfoModel Map { get; set; } = new();
	public int Count { get; set; } = 1;
}
