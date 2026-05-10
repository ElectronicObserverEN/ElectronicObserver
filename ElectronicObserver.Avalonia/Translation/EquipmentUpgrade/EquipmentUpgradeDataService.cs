using ElectronicObserver.Core.Services;
using ElectronicObserver.Core.Types.Serialization.EquipmentUpgrade;

namespace ElectronicObserver.Avalonia.Translation.EquipmentUpgrade;

public sealed class EquipmentUpgradeDataService : DataServiceBase
{
	protected override string FileName => "EquipmentUpgrades.json";
	protected override DataType DataType => DataType.Data;

	public List<EquipmentUpgradeDataModel> UpgradeList { get; private set; } = [];

	public EquipmentUpgradeDataService(
		IConfigurationUi configurationUi,
		ISoftwareUpdaterService softwareUpdaterService,
		IEoLogger logger) : base(configurationUi, softwareUpdaterService, logger)
	{
		_ = Initialize();
	}

	/// <inheritdoc />
	protected override async Task Initialize()
	{
		await LoadDictionary(FilePath);
	}

	private async Task LoadDictionary(string path)
	{
		UpgradeList = await Load<List<EquipmentUpgradeDataModel>>(path) ?? [];
	}
}
