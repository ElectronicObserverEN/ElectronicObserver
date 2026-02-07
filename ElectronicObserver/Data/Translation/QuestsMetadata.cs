using System.Collections.Generic;
using System.IO;
using ElectronicObserver.Core.Types.Serialization.Quests;

namespace ElectronicObserver.Data.Translation;

public class QuestsMetadata : TranslationBase
{
	private string QuestMetadataPath => Path.Join(DataAndTranslationManager.DataFolder, "QuestsMetadata.json");

	public List<QuestMetadata> QuestsMetadataList { get; private set; } = [];

	public sealed override void Initialize()
	{
		LoadDictionary(QuestMetadataPath);
	}

	public QuestsMetadata()
	{
		Initialize();
	}

	private void LoadDictionary(string path)
	{
		QuestsMetadataList.Clear();

		List<QuestMetadata>? json = Load<List<QuestMetadata>>(path);
		if (json != null) QuestsMetadataList = json;
	}
}
