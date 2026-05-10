using ElectronicObserver.Core.Services;
using ElectronicObserver.Core.Types.Serialization.FitBonus;

namespace ElectronicObserver.Avalonia.Translation.FitBonus;

public sealed class FitBonusDataService(
	IConfigurationUi configurationUi,
	ISoftwareUpdaterService softwareUpdaterService,
	IEoLogger logger)
	: DataServiceBase(configurationUi, softwareUpdaterService, logger)
{
	protected override string FileName => "FitBonuses.json";
	protected override DataType DataType => DataType.Data;

	public List<FitBonusPerEquipment> FitBonusList { get; private set; } = [];

	/// <inheritdoc />
	public override async Task Initialize()
	{
		await LoadDictionary(FilePath);
	}

	private async Task LoadDictionary(string path)
	{
		FitBonusList.Clear();

		List<FitBonusPerEquipment>? json = await Load<List<FitBonusPerEquipment>>(path);

		if (json == null) return;

		FitBonusList = json;
	}
}
