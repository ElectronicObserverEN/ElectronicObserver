using System.Diagnostics.CodeAnalysis;
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

	public DestinationDataService Destination { get; private set; }
	public QuestDataService Quest { get; private set; }
	public QuestMetadataDataService QuestsMetadata { get; private set; }
	public EquipmentDataService Equipment { get; private set; }
	public MissionDataService Mission { get; private set; }
	public ShipDataService Ship { get; private set; }
	public OperationDataService Operation { get; private set; }
	public LockDataService Lock { get; private set; }
	public FitBonusDataService FitBonus { get; private set; }
	public EquipmentUpgradeDataService EquipmentUpgrade { get; private set; }

	public DataService(
		IConfigurationUi configurationUi,
		ISoftwareUpdaterService softwareUpdaterService,
		IEoLogger eoLogger)
	{
		ConfigurationUi = configurationUi;
		SoftwareUpdaterService = softwareUpdaterService;
		EoLogger = eoLogger;

		Initialize();
	}

	[MemberNotNull(nameof(Destination))]
	[MemberNotNull(nameof(Quest))]
	[MemberNotNull(nameof(QuestsMetadata))]
	[MemberNotNull(nameof(Equipment))]
	[MemberNotNull(nameof(Mission))]
	[MemberNotNull(nameof(Ship))]
	[MemberNotNull(nameof(Operation))]
	[MemberNotNull(nameof(Lock))]
	[MemberNotNull(nameof(FitBonus))]
	[MemberNotNull(nameof(EquipmentUpgrade))]
	public void Initialize()
	{
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
}
