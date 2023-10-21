using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Database;
using ElectronicObserver.Services;
using ElectronicObserver.TestData;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Tools.AutoRefresh;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;
using ElectronicObserver.Window.Tools.SortieRecordViewer.SortieDetail;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Data;
using ElectronicObserverTypes.Mocks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronicObserverCoreTests;

public class Startup
{
	public async void ConfigureServices(IServiceCollection services)
	{
		await using TestDataContext testDb = new();
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

		Ioc.Default.ConfigureServices(new ServiceCollection()
			.AddSingleton<IKCDatabase>(kcdb)
			.AddSingleton<ColorService>()
			.AddSingleton<AutoRefreshTranslationViewModel>()
			.AddSingleton<SortieDetailTranslationViewModel>()
			.AddSingleton<PhaseFactory>()
			.AddSingleton<BattleFactory>()
			.AddSingleton<DataSerializationService>()
			.AddSingleton<ToolService>()
			.BuildServiceProvider());

		Directory.CreateDirectory("Record");

		await using ElectronicObserverContext db = new();
		await db.Database.MigrateAsync();

		// Download data 
		await SoftwareUpdater.CheckUpdateAsync();
	}
}
