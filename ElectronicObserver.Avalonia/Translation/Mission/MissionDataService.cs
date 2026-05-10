using System.Text.Json;
using ElectronicObserver.Core.Services;

namespace ElectronicObserver.Avalonia.Translation.Mission;

public sealed class MissionDataService : DataServiceBase
{
	protected override string FileName => "expedition.json";
	protected override DataType DataType => DataType.Translation;

	private Dictionary<string, string> NameDictionary { get; set; } = [];

	public MissionDataService(
		IConfigurationUi configurationUi,
		ISoftwareUpdaterService softwareUpdaterService,
		IEoLogger logger) : base(configurationUi, softwareUpdaterService, logger)
	{
		_ = Initialize();
	}

	/// <inheritdoc />
	protected override async Task Initialize()
	{
		NameDictionary = [];
		await LoadDictionary(FilePath);
	}

	private async Task LoadDictionary(string path)
	{
		JsonDocument? json = await Load<JsonDocument>(path);

		if (json is null) return;

		foreach (JsonProperty property in json.RootElement.EnumerateObject())
		{
			if (property.NameEquals("version")) continue;
			if (property.Value.Deserialize<MissionData>() is not MissionData data) continue;

			if (!NameDictionary.ContainsKey(data.NameJp))
			{
				NameDictionary.Add(data.NameJp, data.Name);
			}
		}
	}

	public string Name(string rawData) => ConfigurationUi.DisableOtherTranslations switch
	{
		true => rawData,
		_ => NameDictionary.GetValueOrDefault(rawData, rawData),
	};
}
