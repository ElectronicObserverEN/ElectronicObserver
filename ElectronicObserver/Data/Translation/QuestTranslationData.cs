using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DynaJson;
using ElectronicObserver.Utility;
using Newtonsoft.Json;

namespace ElectronicObserver.Data.Translation;

public class QuestTranslationData : TranslationBase
{
	private string DefaultFilePath = DataAndTranslationManager.TranslationFolder + @"\quest.json";

	private Dictionary<int, Quests> QuestList { get; set; }

	private bool IsTranslated(int id) => Configuration.Config.UI.DisableOtherTranslations == false && QuestList != null && QuestList.ContainsKey(id);
	public string Name(int id, string rawData) => IsTranslated(id) ? QuestList[id].Name : rawData;
	public string Description(int id, string rawData)
	{
		if (IsTranslated(id))
		{
			// Fit tooltip text to 80 characters
			var sentences = QuestList[id].Description.Split(" ");
			var sb = new StringBuilder();

			var line = "";
			foreach (var s in sentences)
			{
				if ((line + s).Length > 80)
				{
					sb.AppendLine(line);
					line = "";
				}
				line += $"{s} ";
			}
			if (line.Length > 0)
				sb.AppendLine(line);
			var desc = sb.ToString();

			return desc;
		}
		else
		{
			return rawData.Replace("<br>", "\r\n");
		}
	}

	public override void Initialize()
	{
		QuestList = LoadDictionary(DefaultFilePath);
	}

	public QuestTranslationData()
	{
		Initialize();
	}

	public class QuestRecord
	{
		[JsonProperty("code")]
		public string Code { get; set; } = "";
		[JsonProperty("name_jp")]
		public string NameJp { get; set; } = "";
		[JsonProperty("name")]
		public string Name { get; set; } = "";
		[JsonProperty("desc_jp")]
		public string DescJp { get; set; } = "";
		[JsonProperty("desc")]
		public string Desc { get; set; } = "";

		// hack to parse the version entry
		public static implicit operator QuestRecord?(string s) => null;
	}


	private Dictionary<int, Quests> LoadDictionary(string path)
	{
		var dict = new Dictionary<int, Quests>();
		Dictionary<string, QuestRecord>? questRecords = null;

		try
		{
			questRecords = JsonConvert.DeserializeObject<Dictionary<string, QuestRecord>>(File.ReadAllText(path));
		}
		catch (FileNotFoundException)
		{
			Utility.Logger.Add(3, GetType().Name + ": File does not exists.");
		}
		catch (DirectoryNotFoundException)
		{
			Utility.Logger.Add(3, GetType().Name + ": File does not exists.");
		}
		catch (Exception ex)
		{
			Utility.ErrorReporter.SendErrorReport(ex, "Failed to load " + GetType().Name);
		}

		if (questRecords == null) return dict;

		foreach ((string i, QuestRecord quest) in questRecords)
		{
			// the quest dictionary includes version
			// ignore that entry
			if (!int.TryParse(i, out int questId)) continue;

			if (quest.Code == "")
			{
				int pos = quest.Name.IndexOf(":");
				if (pos > 0 && pos < 10)
				{
					var ch = char.Parse(quest.Name.Substring(pos - 2, 1));
					quest.Code = pos > 3 && char.IsLetter(ch) ? quest.Name.Substring(0, pos - 2) : quest.Name.Substring(0, pos);
					quest.Name = quest.Name.Substring(pos + 2);
				}
			}
			dict.TryAdd(questId, new Quests(quest.Code, quest.Name, quest.NameJp, quest.Desc, quest.DescJp));
		}

		return dict;
	}

	public Quests? this[int index] => QuestList.ContainsKey(index) switch
	{
		true => QuestList[index],
		false => null
	};
}

public class Quests
{
	public string Code { get; set; }
	public string Name { get; set; }
	public string NameJP { get; set; }
	public string Description { get; set; }
	public string DescriptionJP { get; set; }
	public Quests(string code, string name, string nameJP, string description, string descriptionJP)
	{
		Code = code;
		Name = name;
		NameJP = nameJP;
		DescriptionJP = descriptionJP;
		Description = description;
	}
}
