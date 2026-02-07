namespace ElectronicObserver.Core.Types.Quests;

public interface IQuestIdentifier
{
	public int QuestID { get; }
	public QuestResetType QuestResetType { get; }
}
