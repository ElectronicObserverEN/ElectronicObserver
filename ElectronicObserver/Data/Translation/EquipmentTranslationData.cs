using System;
using System.Collections.Generic;
using System.Text;
using DynaJson;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Data.Translation;

public class EquipmentTranslationData : TranslationBase
{
	private string DefaultFilePath = DataAndTranslationManager.TranslationFolder + @"\equipment.json";

	private Dictionary<string, string> EquipmentList { get; set; } = new();
	private Dictionary<string, string> TypeList { get; set; } = new();

	private bool IsTranslated(string rawData) => Configuration.Config.UI.JapaneseEquipmentName == false &&
												 EquipmentList.ContainsKey(rawData);
	private bool IsTypeTranslated(string rawData) => Configuration.Config.UI.JapaneseEquipmentType == false
													 && TypeList.ContainsKey(rawData);
	public string Name(string rawData) => IsTranslated(rawData) ? EquipmentList[rawData] : rawData;
	public string TypeName(string rawData) => IsTypeTranslated(rawData) ? TypeList[rawData] : rawData;

	public EquipmentTranslationData()
	{
		Initialize();
	}
	public override void Initialize()
	{
		EquipmentList = new Dictionary<string, string>();
		TypeList = new Dictionary<string, string>();
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
				if (category.Key == "equipment")
				{
					EquipmentList.Add(entry.Key, entries[entry.Key]);
				}
				if (category.Key == "equiptype")
				{
					TypeList.Add(entry.Key, entries[entry.Key]);
				}
			}
		}
	}
}
