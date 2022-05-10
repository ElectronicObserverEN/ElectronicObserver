using System;
using System.Collections.Generic;
using System.Text;
using DynaJson;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Data.Translation;

public class OperationData : TranslationBase
{
	public string DefaultFilePath = DataAndTranslationManager.TranslationFolder + @"\operation.json";

	private Dictionary<string, string> MapList;
	private Dictionary<string, string> FleetList;
	private bool isLoaded => Configuration.Config.UI.DisableOtherTranslations == false && MapList != null && FleetList != null;

	public bool IsMapTranslated(string rawData) => isLoaded && MapList.ContainsKey(rawData);
	public bool IsFleetTranslated(string rawData) => isLoaded && FleetList.ContainsKey(rawData);
	public string MapName(string rawData) => IsMapTranslated(rawData) ? MapList[rawData] : rawData;
	public string FleetName(string rawData) => IsFleetTranslated(rawData) ? FleetList[rawData] : rawData;
	public OperationData()
	{
		Initialize();
	}
	public override void Initialize()
	{
		MapList = new Dictionary<string, string>();
		FleetList = new Dictionary<string, string>();
		LoadDictionary(DefaultFilePath);
	}

	public void LoadDictionary(string path)
	{
		var json = Load(path);
		if (json == null) return;

		foreach (KeyValuePair<string, object> category in json)
		{
			if (category.Key == "version") continue;

			var entries = JsonObject.Parse(category.Value.ToString());
			foreach (KeyValuePair<string, dynamic> entry in entries)
			{
				if (category.Key == "map")
				{
					MapList.Add(entry.Key, entries[entry.Key]);
				}
				if (category.Key == "fleet")
				{
					FleetList.Add(entry.Key, entries[entry.Key]);
				}
			}
		}
	}
}
