using ElectronicObserver.Core.Services;

namespace ElectronicObserver.Avalonia.Translation.QuestMetadata;

public sealed class QuestMetadataDataService(
	IConfigurationUi configurationUi,
	ISoftwareUpdaterService softwareUpdaterService,
	IEoLogger logger)
	: DataServiceBase(configurationUi, softwareUpdaterService, logger)
{
	protected override string FileName => "QuestsMetadata.json";
	protected override DataType DataType => DataType.Data;

	public Dictionary<int, Core.Types.Serialization.Quests.QuestMetadata> QuestsMetadataList { get; private set; } = [];

	/// <inheritdoc />
	public override async Task Initialize()
	{
		await LoadDictionary(FilePath);
	}

	private async Task LoadDictionary(string path)
	{
		QuestsMetadataList.Clear();

		List<Core.Types.Serialization.Quests.QuestMetadata>? json =
			await Load<List<Core.Types.Serialization.Quests.QuestMetadata>>(path);

		if (json is not null)
		{
			QuestsMetadataList = json.ToDictionary(item => item.ApiId, item => item);
		}
	}
}
