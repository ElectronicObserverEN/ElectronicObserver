using System.Linq;
using ElectronicObserver.Avalonia.Dialogs;
using ElectronicObserver.Avalonia.Dialogs.EquipmentSelector;
using ElectronicObserver.Avalonia.Dialogs.ShipSelector;
using ElectronicObserver.Avalonia.QuestTrackerManager2;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks;
using ElectronicObserver.Avalonia.Samples.QuestTrackerManager;
using ElectronicObserver.Avalonia.Services;
using ElectronicObserver.Avalonia.Translation;
using ElectronicObserver.Core.Services;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Data;
using ElectronicObserver.Core.Types.Mocks;
using ElectronicObserver.TestData;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.Avalonia;
using Jab;

namespace ElectronicObserver.Avalonia.Samples;

[ServiceProvider]
[Singleton<ServiceProvider>]
[Singleton<IConfigurationConnection, ConfigurationConnection>]
[Singleton<IConfigurationUi, ConfigurationUi>]
[Singleton<IViewLocator, ViewLocator>]
[Singleton<IDialogManager, DialogManager>]
[Singleton<IDialogService, DialogService>]
[Singleton<Dialogs.DialogServiceBase, AvaloniaDialogService>]
[Singleton<GameResourceHelper>]
[Singleton<GameAssetDownloaderService>]
[Singleton<ImageLoadService>]
[Singleton<TransliterationService>]
[Singleton<ShipSelectorFactory>]
[Singleton<EquipmentSelectorFactory>]
[Singleton<ConditionDisplayerService>]
[Singleton<ConditionFactory>]
[Singleton<TaskDisplayerService>]
[Singleton<FleetEvaluationService>]
[Singleton<TaskFactory>]
[Singleton<IQuestProvider, QuestProvider>]
[Singleton(typeof(IClipboardService), Instance = nameof(ClipboardService))]
[Singleton<QuestTrackerManager2ViewModel>]
[Singleton<ISoftwareUpdaterService, SoftwareUpdaterService>]
[Singleton<IEoLogger, EoLogger>]
[Singleton<DataService>]
[Singleton<MainViewModel>]
[Singleton<IKCDatabase>(Factory = nameof(KcDatabaseFactory))]
public sealed partial class ServiceProvider
{
	public IClipboardService ClipboardService { get; set; } = null!;

	public IKCDatabase KcDatabaseFactory()
	{
		using TestDataContext testDb = new();

		IDDictionary<IShipDataMaster> masterShips = [with(testDb.MasterShips.Select(s => s.ToMasterShip()))];

		foreach (ShipDataMasterMock masterShip in masterShips.Values.OfType<ShipDataMasterMock>())
		{
			if (masterShip.RemodelBeforeShipID > 0)
			{
				masterShip.RemodelBeforeShip = masterShips[masterShip.RemodelBeforeShipID];
			}

			if (masterShip.RemodelAfterShipID > 0)
			{
				masterShip.RemodelAfterShip = masterShips[masterShip.RemodelAfterShipID];
			}
		}

		return new KCDatabaseMock()
		{
			MasterShips = masterShips,
			MasterEquipments = [with(testDb.MasterEquipment.Select(e => e.ToMasterEquipment()))],
		};
	}
}
