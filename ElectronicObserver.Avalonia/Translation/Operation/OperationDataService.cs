using ElectronicObserver.Core.Services;

namespace ElectronicObserver.Avalonia.Translation.Operation;

public sealed class OperationDataService : DataServiceBase
{
	protected override string FileName => "operation.json";
	protected override DataType DataType => DataType.Translation;

	private Dictionary<string, string> MapList { get; set; } = [];
	private Dictionary<string, string> FleetList { get; set; } = [];

	public OperationDataService(
		IConfigurationUi configurationUi,
		ISoftwareUpdaterService softwareUpdaterService,
		IEoLogger logger) : base(configurationUi, softwareUpdaterService, logger)
	{
		_ = Initialize();
	}

	/// <inheritdoc />
	protected override async Task Initialize()
	{
		MapList = [];
		FleetList = [];
		await LoadDictionary(FilePath);
	}

	private async Task LoadDictionary(string path)
	{
		OperationData? json = await Load<OperationData>(path);

		if (json == null) return;

		MapList = json.Map;
		FleetList = json.Fleet;
	}

	public string MapName(string rawData) => ConfigurationUi.DisableOtherTranslations switch
	{
		true => rawData,
		_ => MapList.GetValueOrDefault(rawData, rawData),
	};

	public string FleetName(string rawData) => ConfigurationUi.DisableOtherTranslations switch
	{
		true => rawData,
		_ => FleetList.GetValueOrDefault(rawData, rawData),
	};
}
