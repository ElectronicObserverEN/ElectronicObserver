using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using ElectronicObserver.Database;
using ElectronicObserver.Database.DataMigration;
using ElectronicObserver.Database.KancolleApi;
using ElectronicObserver.Database.Sortie;
using ElectronicObserver.Services;
using ElectronicObserver.Window.Tools.SortieRecordViewer;
using ElectronicObserver.Window.Tools.SortieRecordViewer.SortieCostViewer;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ElectronicObserverCoreTests.SortieCost;

public class SortieCostTests
{
	private static string BasePath =>
		Path.Join(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "SortieCost");

	private static async Task<T> GetDataFromFile<T>(string fileName)
	{
		Stream stream = File.OpenRead(Path.Join(BasePath, fileName));
		T? data = await JsonSerializer.DeserializeAsync<T>(stream);

		Assert.NotNull(data);

		return data;
	}

	private static async Task<List<SortieCostViewModel>> MakeSortieCosts(string testFilePrefix,
		bool clearFleetAfterBattleData = false)
	{
		ToolService toolService = new(new());
		SortieRecordMigrationService sortieRecordMigrationService = new(toolService);

		await using ElectronicObserverContext db = new(true);
		await db.Database.EnsureDeletedAsync();
		await db.Database.EnsureCreatedAsync();

		List<ApiFile> apiFiles = await GetDataFromFile<List<ApiFile>>($"{testFilePrefix}ApiFiles.json");
		List<SortieRecord> sortieRecords = await GetDataFromFile<List<SortieRecord>>($"{testFilePrefix}SortieRecords.json");

		if (clearFleetAfterBattleData)
		{
			foreach (SortieRecord sortie in sortieRecords)
			{
				sortie.FleetAfterSortieData = null;
			}
		}

		await db.AddRangeAsync(sortieRecords);
		await db.AddRangeAsync(apiFiles);
		await db.SaveChangesAsync();

		List<SortieRecordViewModel> sorties = await db.Sorties
			.Include(s => s.ApiFiles)
			.Select(s => new SortieRecordViewModel(s, s.ApiFiles.Select(f => f.TimeStamp).Min()))
			.ToListAsync();

		List<SortieCostViewModel> sortieCosts = [];

		foreach (SortieRecordViewModel sortie in sorties)
		{
			sortieCosts.Add(new(db, toolService, sortieRecordMigrationService, sortie));
		}

		return sortieCosts;
	}

	[Theory(DisplayName = "no fleet after sortie data")]
	[InlineData("SortieCostTest2")]
	[InlineData("SortieCostTest3")]
	public async Task SortieCostTest0(string testFilePrefix)
	{
		List<SortieCostViewModel> sortieCosts1 = await MakeSortieCosts(testFilePrefix);
		List<SortieCostViewModel> sortieCosts2 = await MakeSortieCosts(testFilePrefix, true);

		Assert.Equal(sortieCosts1.Count, sortieCosts2.Count);

		foreach ((SortieCostViewModel first, SortieCostViewModel second) in sortieCosts1.Zip(sortieCosts2))
		{
			Assert.Equal(first.SortieFleetSupplyCost, second.SortieFleetSupplyCost);
			Assert.Equal(first.SortieFleetRepairCost, second.SortieFleetRepairCost);
			Assert.Equal(first.TotalAirBaseSortieCost, second.TotalAirBaseSortieCost);
			Assert.Equal(first.TotalAirBaseSupplyCost, second.TotalAirBaseSupplyCost);
		}
	}

	[Fact(DisplayName = "Double 7-4 resource run without resupply with AB")]
	public async Task SortieCostTest1()
	{
		List<SortieCostViewModel> sortieCosts = await MakeSortieCosts("SortieCostTest1");

		Assert.Equal(2, sortieCosts.Count);

		SortieCostModel firstSortieFleetCost = new() { Fuel = 33, Ammo = 31 };
		SortieCostModel secondSortieFleetCost = new() { Fuel = 27, Ammo = 32 };
		SortieCostModel airBaseSortieCost = new() { Fuel = 8, Ammo = 6 };

		Assert.Equal(firstSortieFleetCost, sortieCosts[0].SortieFleetSupplyCost);
		Assert.Equal(secondSortieFleetCost, sortieCosts[1].SortieFleetSupplyCost);

		Assert.Equal(airBaseSortieCost, sortieCosts[0].TotalAirBaseSortieCost);
		Assert.Equal(airBaseSortieCost, sortieCosts[1].TotalAirBaseSortieCost);

		Assert.Equal(new(), sortieCosts[0].TotalAirBaseSupplyCost);
		Assert.Equal(new(), sortieCosts[1].TotalAirBaseSupplyCost);

		Assert.Equal(firstSortieFleetCost + airBaseSortieCost, sortieCosts[0].TotalCost);
		Assert.Equal(secondSortieFleetCost + airBaseSortieCost, sortieCosts[1].TotalCost);
	}

	[Fact(DisplayName = "6-5 with AB")]
	public async Task SortieCostTest2()
	{
		List<SortieCostViewModel> sortieCosts = await MakeSortieCosts("SortieCostTest2");

		Assert.Single(sortieCosts);

		SortieCostModel firstSortieFleetCost = new() { Fuel = 368, Ammo = 511, Bauxite = 150 };
		SortieCostModel airBase1SortieCost = new() { Fuel = 108, Ammo = 48 };
		SortieCostModel airBase2SortieCost = new() { Fuel = 99, Ammo = 47 };
		SortieCostModel airBase1ResupplyCost = new() { Fuel = 78, Ammo = 130 };
		SortieCostModel airBase2ResupplyCost = new() { Fuel = 81, Ammo = 135 };

		Assert.Equal(firstSortieFleetCost, sortieCosts[0].SortieFleetSupplyCost);
		Assert.Equal(airBase1SortieCost + airBase2SortieCost, sortieCosts[0].TotalAirBaseSortieCost);
		Assert.Equal(airBase1ResupplyCost + airBase2ResupplyCost, sortieCosts[0].TotalAirBaseSupplyCost);
	}

	[Fact(DisplayName = "Sortie record version 0→1 test")]
	public async Task SortieCostTest3()
	{
		List<SortieCostViewModel> sortieCosts = await MakeSortieCosts("SortieCostTest3");

		Assert.Single(sortieCosts);

		SortieCostModel resupplyCost = new() { Fuel = 525, Ammo = 532, Bauxite = 200 };
		SortieCostModel souyaRepairCost = new() { Fuel = 16, Steel = 30 };
		SortieCostModel musashiRepairCost = new() { Fuel = 52, Steel = 99 };

		Assert.Equal(resupplyCost, sortieCosts[0].SortieFleetSupplyCost);
		Assert.Equal(souyaRepairCost + musashiRepairCost, sortieCosts[0].SortieFleetRepairCost);
	}
}
