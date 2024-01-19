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

public abstract class SortieCostTestBase
{
	private static string DirectoryName => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

	protected abstract string RelativePath { get; }

	private string BasePath => Path.Join(DirectoryName, RelativePath);

	private async Task<T> GetDataFromFile<T>(string fileName) where T : new()
	{
		string path = Path.Join(BasePath, fileName);

		if (!File.Exists(path)) return new();

		Stream stream = File.OpenRead(path);
		T? data = await JsonSerializer.DeserializeAsync<T>(stream);

		Assert.NotNull(data);

		return data;
	}

	protected async Task<List<SortieCostViewModel>> MakeSortieCosts(string testFilePrefix,
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

	public virtual async Task SortieCostTest0(string testFilePrefix)
	{
		List<SortieCostViewModel> sortieCosts2 = await MakeSortieCosts(testFilePrefix, true);
		List<SortieCostViewModel> sortieCosts1 = await MakeSortieCosts(testFilePrefix);

		Assert.Equal(sortieCosts1.Count, sortieCosts2.Count);

		foreach ((SortieCostViewModel first, SortieCostViewModel second) in sortieCosts1.Zip(sortieCosts2))
		{
			Assert.Equal(first.SortieFleetSupplyCost, second.SortieFleetSupplyCost);
			Assert.Equal(first.SortieFleetRepairCost, second.SortieFleetRepairCost);
			Assert.Equal(first.TotalAirBaseSortieCost, second.TotalAirBaseSortieCost);
			Assert.Equal(first.TotalAirBaseSupplyCost, second.TotalAirBaseSupplyCost);
		}
	}
}
