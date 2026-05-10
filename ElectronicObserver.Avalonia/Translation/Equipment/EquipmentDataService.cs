using ElectronicObserver.Core.Services;

namespace ElectronicObserver.Avalonia.Translation.Equipment;

public sealed class EquipmentDataService : DataServiceBase
{
	protected override string FileName => "equipment.json";
	protected override DataType DataType => DataType.Translation;

	private Dictionary<string, string> EquipmentList { get; set; } = [];
	private Dictionary<string, string> TypeList { get; set; } = [];

	public EquipmentDataService(
		IConfigurationUi configurationUi,
		ISoftwareUpdaterService softwareUpdaterService,
		IEoLogger logger) : base(configurationUi, softwareUpdaterService, logger)
	{
		_ = Initialize();
	}

	/// <inheritdoc />
	protected override async Task Initialize()
	{
		EquipmentList = [];
		TypeList = [];
		await LoadDictionary(FilePath);
	}

	private async Task LoadDictionary(string path)
	{
		EquipmentData? json = await Load<EquipmentData>(path);

		if (json == null) return;

		EquipmentList = json.Equipment;
		TypeList = json.EquipmentType;
	}

	public string Name(string rawData) => ConfigurationUi.JapaneseEquipmentName switch
	{
		true => rawData,
		_ => EquipmentList.GetValueOrDefault(rawData, rawData),
	};

	public string TypeName(string rawData) => ConfigurationUi.JapaneseEquipmentType switch
	{
		true => rawData,
		_ => TypeList.GetValueOrDefault(rawData, rawData),
	};
}
