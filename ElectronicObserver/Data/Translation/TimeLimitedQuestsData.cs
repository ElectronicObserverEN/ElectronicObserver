using System.Collections.Generic;
using System.IO;
using ElectronicObserver.Core.Types.Serialization.Quests;

namespace ElectronicObserver.Data.Translation;

public class TimeLimitedQuestsData : TranslationBase
{
	private string TimeLimitedQuestsDataPath => Path.Join(DataAndTranslationManager.DataFolder, "TimeLimitedQuests.json");

	public List<TimeLimitedQuestData> TimeLimitedQuests = [];

	public override void Initialize()
	{
		LoadDictionary(TimeLimitedQuestsDataPath);
	}

	public TimeLimitedQuestsData()
	{
		Initialize();
	}

	private void LoadDictionary(string path)
	{
		TimeLimitedQuests.Clear();

		List<TimeLimitedQuestData>? json = Load<List<TimeLimitedQuestData>>(path);
		if (json != null) TimeLimitedQuests = json;
	}
}
