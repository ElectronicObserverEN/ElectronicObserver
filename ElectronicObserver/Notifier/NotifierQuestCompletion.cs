using ElectronicObserver.Data;
using ElectronicObserver.Data.Quest;

namespace ElectronicObserver.Notifier;

public class NotifierQuestCompletion : NotifierBase
{
	public NotifierQuestCompletion(Utility.Configuration.ConfigurationData.ConfigNotifierBase config)
		: base(config)
	{
		DialogData.Title = NotifierRes.QuestCompletionTitle;

		QuestCompletionEvaluator.Instance.QuestCompleted += Notify;
		QuestCompletionEvaluator.Instance.RefreshBaseline();
	}

	private void Notify(QuestData quest)
	{
		DialogData.Message = string.Format(NotifierRes.QuestCompletionText, quest.NameWithCode);

		base.Notify();
	}
}
