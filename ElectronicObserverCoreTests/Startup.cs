using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Data;
using ElectronicObserver.Database;
using ElectronicObserver.Services;
using ElectronicObserver.TestData;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Tools.AutoRefresh;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;
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

		await InitializeKcDatabase(kcdb);

		Ioc.Default.ConfigureServices(new ServiceCollection()
			.AddSingleton<IKCDatabase>(kcdb)
			.AddSingleton<ColorService>()
			.AddSingleton<AutoRefreshTranslationViewModel>()
			.AddSingleton<SortieDetailTranslationViewModel>()
			.AddSingleton<EquipmentUpgradePlannerTranslationViewModel>()
			.AddSingleton<PhaseFactory>()
			.AddSingleton<BattleFactory>()
			.AddSingleton<DataSerializationService>()
			.AddSingleton<ToolService>()
			.AddSingleton<TimeChangeService>()
			.AddSingleton<EquipmentUpgradePlanManager>()
			.BuildServiceProvider());

		Directory.CreateDirectory("Record");

		await using ElectronicObserverContext db = new();
		await db.Database.MigrateAsync();

		// Download data 
		await SoftwareUpdater.CheckUpdateAsync();
	}

	/// <summary>
	/// hack: this should ideally be removed
	/// </summary>
	private static async Task InitializeKcDatabase(KCDatabaseMock kcdb)
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
