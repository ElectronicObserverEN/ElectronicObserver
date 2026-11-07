namespace ElectronicObserver.Avalonia.QuestTrackerManager2;

public interface IQuestTrackerManager
{
	DateTime LastQuestListUpdate { get; set; }
	DateTime LastTrackerSave { get; set; }
	void Load();
	void Save();
	IEnumerable<ITrackerViewModel> Trackers { get; }
}
