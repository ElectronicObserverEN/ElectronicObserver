using ElectronicObserver.Avalonia.Translation.Destination;
using ElectronicObserver.Avalonia.Translation.Equipment;
using ElectronicObserver.Avalonia.Translation.EquipmentUpgrade;
using ElectronicObserver.Avalonia.Translation.FitBonus;
using ElectronicObserver.Avalonia.Translation.Lock;
using ElectronicObserver.Avalonia.Translation.Mission;
using ElectronicObserver.Avalonia.Translation.Operation;
using ElectronicObserver.Avalonia.Translation.Quest;
using ElectronicObserver.Avalonia.Translation.QuestMetadata;
using ElectronicObserver.Avalonia.Translation.Ship;
using ElectronicObserver.Core.Services;

namespace ElectronicObserver.Avalonia.Translation;

public class DataService
{
	private IConfigurationUi ConfigurationUi { get; }
	private ISoftwareUpdaterService SoftwareUpdaterService { get; }
	private IEoLogger EoLogger { get; }

	public DestinationDataService Destination { get; }
	public QuestDataService Quest { get; }
	public QuestMetadataDataService QuestsMetadata { get; }
	public EquipmentDataService Equipment { get; }
	public MissionDataService Mission { get; }
	public ShipDataService Ship { get; }
	public OperationDataService Operation { get; }
	public LockDataService Lock { get; }
	public FitBonusDataService FitBonus { get; }
	public EquipmentUpgradeDataService EquipmentUpgrade { get; }

	public DataService(
		IConfigurationUi configurationUi,
		ISoftwareUpdaterService softwareUpdaterService,
		IEoLogger eoLogger)
	{
		ConfigurationUi = configurationUi;
		SoftwareUpdaterService = softwareUpdaterService;
		EoLogger = eoLogger;

		Destination = new(ConfigurationUi, SoftwareUpdaterService, EoLogger);
		Quest = new(ConfigurationUi, SoftwareUpdaterService, EoLogger);
		QuestsMetadata = new(ConfigurationUi, SoftwareUpdaterService, EoLogger);
		Equipment = new(ConfigurationUi, SoftwareUpdaterService, EoLogger);
		Mission = new(ConfigurationUi, SoftwareUpdaterService, EoLogger);
		Ship = new(ConfigurationUi, SoftwareUpdaterService, EoLogger);
		Operation = new(ConfigurationUi, SoftwareUpdaterService, EoLogger);
		Lock = new(ConfigurationUi, SoftwareUpdaterService, EoLogger);
		FitBonus = new(ConfigurationUi, SoftwareUpdaterService, EoLogger);
		EquipmentUpgrade = new(ConfigurationUi, SoftwareUpdaterService, EoLogger);
	}

	public async Task Initialize()
	{
		await Destination.Initialize();
		await Quest.Initialize();
		await QuestsMetadata.Initialize();
		await Equipment.Initialize();
		await Mission.Initialize();
		await Ship.Initialize();
		await Operation.Initialize();
		await Lock.Initialize();
		await FitBonus.Initialize();
		await EquipmentUpgrade.Initialize();
	}
}
