using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Avalonia.Services;
using ElectronicObserver.Core.Services;
using ElectronicObserver.Core.Services.Data;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Data;
using ElectronicObserver.Core.Types.Mocks;
using ElectronicObserver.Data;
using ElectronicObserver.Database;
using ElectronicObserver.Services;
using ElectronicObserver.TestData;
using ElectronicObserver.Utility;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Tools.AutoRefresh;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;
using ElectronicObserver.Window.Tools.SortieRecordViewer.SortieDetail;
using ElectronicObserver.Window.Wpf.ShipTrainingPlanner;
using ElectronicObserverCoreTests.Mocks;
using Jot;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static ElectronicObserver.Utility.Configuration.ConfigurationData;

namespace ElectronicObserverCoreTests;

public class Startup
{
	public void ConfigureServices(IServiceCollection services)
	{
		using TestDataContext testDb = new();
		Dictionary<ShipId, IShipDataMaster> masterShips = testDb.MasterShips
			.Select(s => s.ToMasterShip())
			.ToDictionary(s => s.ShipId);

		foreach (ShipDataMasterMock ship in masterShips.Values.Cast<ShipDataMasterMock>())
		{
			if (ship.RemodelBeforeShipID > 0)
			{
				ship.RemodelBeforeShip = masterShips[(ShipId)ship.RemodelBeforeShipID];
			}

			if (ship.RemodelAfterShipID > 0)
			{
				ship.RemodelAfterShip = masterShips[(ShipId)ship.RemodelAfterShipID];
			}
		}

		KCDatabaseMock kcdb = new();

		foreach (IShipDataMaster ship in masterShips.Values)
		{
			kcdb.MasterShips.Add(ship);
		}

		foreach (IEquipmentDataMaster equipment in testDb.MasterEquipment.Select(e => e.ToMasterEquipment()))
		{
			kcdb.MasterEquipments.Add(equipment);
		}

		InitializeKcDatabase(kcdb);

		Ioc.Default.ConfigureServices(new ServiceCollection()
			.AddSingleton<IKCDatabase>(kcdb)
			.AddSingleton<ColorService>()
			.AddSingleton<AutoRefreshTranslationViewModel>()
			.AddSingleton<SortieDetailTranslationViewModel>()
			.AddSingleton<EquipmentUpgradePlannerTranslationViewModel>()
			.AddSingleton<FormFleetTranslationViewModel>()
			.AddSingleton<FormFleetOverviewTranslationViewModel>()
			.AddSingleton<ShipTrainingPlannerTranslationViewModel>()
			.AddSingleton<PhaseFactory>()
			.AddSingleton<BattleFactory>()
			.AddSingleton<DataSerializationService>()
			.AddSingleton<ToolService>()
			.AddSingleton<TimeChangeService>()
			.AddSingleton<IConfigurationUi, ConfigUI>()
			.AddSingleton<IConfigurationConnection, ConfigConnection>()
			.AddSingleton<TransliterationService>()
			.AddSingleton<GameResourceHelper>()
			.AddSingleton<GameAssetDownloaderService>()
			.AddSingleton<ImageLoadService>()
			.AddSingleton<EquipmentUpgradePlanManager>()
			.AddSingleton<IClipboardService, ClipboardService>()
			.AddSingleton<ITransportGaugeService, TransportGaugeService>()
			.AddSingleton<ShipTrainingPlanViewerViewModel>()
			.AddSingleton(new Tracker(new TestStore()))
			.BuildServiceProvider());

		Directory.CreateDirectory("Record");

		using ElectronicObserverContext db = new();
		db.Database.MigrateAsync();

		// Download data 
		SoftwareUpdater.CheckUpdateAsync().Wait();
	}

	/// <summary>
	/// hack: this should ideally be removed
	/// </summary>
	private static void InitializeKcDatabase(KCDatabaseMock kcdb)
	{
		foreach (IShipDataMaster ship in kcdb.MasterShips.Values)
		{
			KCDatabase.Instance.MasterShips.Add(ship);
		}

		foreach (IEquipmentDataMaster equipment in kcdb.MasterEquipments.Values)
		{
			KCDatabase.Instance.MasterEquipments.Add(equipment);
		}

		foreach (UseItemId useItemId in Enum.GetValues<UseItemId>())
		{
			UseItemMasterMock itemMaster = new() { ItemID = useItemId };
			UseItemMock item = new()
			{
				ItemID = (int)itemMaster.ItemID,
				MasterUseItem = itemMaster,
			};

			KCDatabase.Instance.MasterUseItems.Add(itemMaster);
			KCDatabase.Instance.UseItems.Add(item);
		}
	}
}
