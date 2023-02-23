using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Data.Translation;
using ElectronicObserver.Database;
using ElectronicObserver.TestData;
using ElectronicObserver.Window.Tools.AutoRefresh;
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
			.AddSingleton<AutoRefreshTranslationViewModel>()
			.BuildServiceProvider());

		Directory.CreateDirectory("Record");

		await using ElectronicObserverContext db = new();
		await db.Database.MigrateAsync();

		// Download data 
		Directory.CreateDirectory(DataAndTranslationManager.DataFolder);
		await DownloadData("https://raw.githubusercontent.com/ElectronicObserverEN/Data/master/Data/EquipmentUpgrades.json", DataAndTranslationManager.DataFolder + @"\EquipmentUpgrades.json");
		await DownloadData("https://raw.githubusercontent.com/ElectronicObserverEN/Data/master/Data/FitBonuses.json", DataAndTranslationManager.DataFolder + @"\FitBonuses.json");
	}

	private async Task DownloadData(string url, string path)
	{
		using HttpClient client = new();
		using HttpResponseMessage response = await client.GetAsync(url);
		response.EnsureSuccessStatusCode();

		await File.WriteAllTextAsync(path, await response.Content.ReadAsStringAsync());
	}
}
