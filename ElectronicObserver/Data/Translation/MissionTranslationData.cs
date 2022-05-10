using System;
using System.Collections.Generic;
using System.Text;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Data.Translation;

public class MissionTranslationData : TranslationBase
{
	private string DefaultFilePath = DataAndTranslationManager.TranslationFolder + @"\expedition.json";

	private Dictionary<string, string> NameDictionary;
	private bool isLoaded => Configuration.Config.UI.DisableOtherTranslations == false && NameDictionary != null;

	public bool IsTranslated(string rawData) => isLoaded && NameDictionary.ContainsKey(rawData);
	public string Name(string rawData) => IsTranslated(rawData) ? NameDictionary[rawData] : rawData;

	public void LoadDictionary(string path)
	{
		var json = Load(path);
		if (json == null) return;

		for (int i = 1; i < 1000; i++)
		{
			if (json.IsDefined(i.ToString()))
			{
				var name = json[i.ToString()]["name"];
				var nameJP = json[i.ToString()]["name_jp"];
				NameDictionary.TryAdd(nameJP, name);
			}
		}
	}

	public override void Initialize()
	{
		NameDictionary = new Dictionary<string, string>();
		LoadDictionary(DefaultFilePath);
	}

	public MissionTranslationData()
	{
		Initialize();
	}
}
