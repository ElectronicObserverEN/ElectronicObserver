using System.Text;
using System.Text.Json;
using ElectronicObserver.Core.Services;

namespace ElectronicObserver.Avalonia.Translation.Quest;

public sealed class QuestDataService : DataServiceBase
{
	protected override string FileName => "quest.json";
	protected override DataType DataType => DataType.Translation;

	private Dictionary<int, QuestRecord> QuestList { get; set; } = [];

	public QuestDataService(
		IConfigurationUi configurationUi,
		ISoftwareUpdaterService softwareUpdaterService,
		IEoLogger logger) : base(configurationUi, softwareUpdaterService, logger)
	{
		_ = Initialize();
	}

	/// <inheritdoc />
	protected override async Task Initialize()
	{
		QuestList = [];
		await LoadDictionary(FilePath);
	}

	private async Task LoadDictionary(string path)
	{
		Dictionary<string, JsonElement>? raw = await Load<Dictionary<string, JsonElement>>(path);

		if (raw is null) return;

		foreach ((string key, JsonElement data) in raw)
		{
			if (key is "version") continue;
			if (data.Deserialize<QuestRecord>() is not QuestRecord quest) continue;
			if (!int.TryParse(key, out int questId)) continue;

			QuestList.TryAdd(questId, quest);
		}
	}

	private QuestRecord? TryGetQuest(int id)
	{
		if (ConfigurationUi.DisableOtherTranslations) return null;

		QuestList.TryGetValue(id, out QuestRecord? quest);
		
		return quest;
	}
	
	public string Name(int id, string rawData) => TryGetQuest(id)?.Name ?? rawData;

	public string Code(int id) => TryGetQuest(id)?.Code ?? "";

	public string Description(int id, string rawData)
	{
		if (TryGetQuest(id) is not QuestRecord quest)
		{
			return rawData.Replace("<br>", "\r\n");
		}

		string[] words = quest.Description.Split(' ');
		StringBuilder sb = new();
		StringBuilder line = new();

		foreach (string word in words)
		{
			if (line.Length + word.Length > 80)
			{
				sb.AppendLine(line.ToString().TrimEnd());
				line.Clear();
			}
			line.Append(word).Append(' ');
		}

		if (line.Length > 0)
		{
			sb.AppendLine(line.ToString().TrimEnd());
		}

		return sb.ToString();
	}
}
