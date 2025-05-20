using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using ElectronicObserver.Avalonia.Services;
using ElectronicObserver.Data.Battle;
using ElectronicObserver.Database;
using ElectronicObserver.Database.Sortie;
using ElectronicObserver.Services;
using ElectronicObserver.Window.Tools.SortieRecordViewer;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Node;
using ElectronicObserver.Window.Tools.SortieRecordViewer.SortieDetail;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Mocks;
using Microsoft.EntityFrameworkCore;
using BattleRanks = ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums.BattleRank;
using BattleBaseAirRaid = ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.BattleBaseAirRaid;
using Xunit;

namespace ElectronicObserverCoreTests.BattleRank;

[Collection(DatabaseCollection.Name)]
public class BattleRankTests(DatabaseFixture database)
{
	private static string DirectoryName => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
	private static string RelativePath => "BattleRank";
	private static string BasePath => Path.Join(DirectoryName, RelativePath);

	private DatabaseFixture Database { get; } = database;
	
	private static async Task<T> GetDataFromFile<T>(string fileName) where T : new()
	{
		string path = Path.Join(BasePath, fileName);

		if (!File.Exists(path)) return new();

		Stream stream = File.OpenRead(path);
		T? data = await JsonSerializer.DeserializeAsync<T>(stream);

		Assert.NotNull(data);

		return data;
	}

	private static async Task<List<SortieRecordViewModel>> MakeSortieRecords(ElectronicObserverContext db,
		string fileName)
	{
		List<SortieRecord> sortieRecords = await GetDataFromFile<List<SortieRecord>>(fileName);

		await db.AddRangeAsync(sortieRecords);
		await db.SaveChangesAsync();

		List<SortieRecordViewModel> sorties = await db.Sorties
			.Include(s => s.ApiFiles)
			.Select(s => new SortieRecordViewModel(s, s.ApiFiles.Select(f => f.TimeStamp).Min(), null!))
			.ToListAsync();

		return sorties;
	}

	private static async Task<List<SortieDetailViewModel>> MakeSortieDetails(string fileName)
	{
		await using ElectronicObserverContext db = new(true);
		await db.Database.EnsureDeletedAsync();
		await db.Database.EnsureCreatedAsync();

		ToolService toolService = new(new());

		List<SortieRecordViewModel> sortieRecords = await MakeSortieRecords(db, fileName);

		List<SortieDetailViewModel> sortieDetails = sortieRecords
			.Select(r => toolService.GenerateSortieDetailViewModel(db, r.Model))
			.OfType<SortieDetailViewModel>()
			.ToList();

		return sortieDetails;
	}

	[Fact(DisplayName = "Battle against untargetable enemy, rank should be S")]
	public async Task SortieDetailTest1()
	{
		List<SortieDetailViewModel> sortieDetails = await MakeSortieDetails("BattleRankTest01.json");

		Assert.Single(sortieDetails);
		Assert.True(sortieDetails[0].Nodes.Count > 4);

		// Early Spring 2023 - E1-H2  
		BattleNode battle = (BattleNode)sortieDetails[0].Nodes[4];

		BattleRankPrediction prediction = new NormalBattleRankPrediction()
		{
			FriendlyMainFleetBefore = battle.FirstBattle.FleetsBeforeBattle.Fleet,
			FriendlyMainFleetAfter = battle.LastBattle.FleetsAfterBattle.Fleet,

			FriendlyEscortFleetBefore = battle.FirstBattle.FleetsBeforeBattle.EscortFleet,
			FriendlyEscortFleetAfter = battle.LastBattle.FleetsAfterBattle.EscortFleet,

			EnemyMainFleetBefore = battle.FirstBattle.FleetsBeforeBattle.EnemyFleet!,
			EnemyMainFleetAfter = battle.LastBattle.FleetsAfterBattle.EnemyFleet!,

			EnemyEscortFleetBefore = battle.FirstBattle.FleetsBeforeBattle.EnemyEscortFleet,
			EnemyEscortFleetAfter = battle.LastBattle.FleetsAfterBattle.EnemyEscortFleet,
		};

		Assert.Equal(BattleRanks.S, prediction.PredictRank());
	}

	[Fact(DisplayName = "Battle against untargetable enemy, rank should be SS")]
	public async Task SortieDetailTest2()
	{
		List<SortieDetailViewModel> sortieDetails = await MakeSortieDetails("BattleRankTest02.json");

		Assert.Single(sortieDetails);
		Assert.True(sortieDetails[0].Nodes.Count > 4);

		// Early Spring 2023 - E1-H2 
		BattleNode battle = (BattleNode)sortieDetails[0].Nodes[4];

		BattleRankPrediction prediction = new NormalBattleRankPrediction()
		{
			FriendlyMainFleetBefore = battle.FirstBattle.FleetsBeforeBattle.Fleet,
			FriendlyMainFleetAfter = battle.LastBattle.FleetsAfterBattle.Fleet,

			FriendlyEscortFleetBefore = battle.FirstBattle.FleetsBeforeBattle.EscortFleet,
			FriendlyEscortFleetAfter = battle.LastBattle.FleetsAfterBattle.EscortFleet,

			EnemyMainFleetBefore = battle.FirstBattle.FleetsBeforeBattle.EnemyFleet!,
			EnemyMainFleetAfter = battle.LastBattle.FleetsAfterBattle.EnemyFleet!,

			EnemyEscortFleetBefore = battle.FirstBattle.FleetsBeforeBattle.EnemyEscortFleet,
			EnemyEscortFleetAfter = battle.LastBattle.FleetsAfterBattle.EnemyEscortFleet,
		};

		Assert.Equal(BattleRanks.SS, prediction.PredictRank());
	}

	[Fact(DisplayName = "Battle with enemies sank by opening torpedo, rank should be SS on second node and S on last one")]
	public async Task SortieDetailTest3()
	{
		List<SortieDetailViewModel> sortieDetails = await MakeSortieDetails("BattleRankTest03.json");

		Assert.Single(sortieDetails);
		Assert.True(sortieDetails[0].Nodes.Count > 1);

		// Early Spring 2023 - E2-O
		BattleNode battle = (BattleNode)sortieDetails[0].Nodes[1];

		BattleRankPrediction prediction = new NormalBattleRankPrediction()
		{
			FriendlyMainFleetBefore = battle.FirstBattle.FleetsBeforeBattle.Fleet,
			FriendlyMainFleetAfter = battle.LastBattle.FleetsAfterBattle.Fleet,

			FriendlyEscortFleetBefore = battle.FirstBattle.FleetsBeforeBattle.EscortFleet,
			FriendlyEscortFleetAfter = battle.LastBattle.FleetsAfterBattle.EscortFleet,

			EnemyMainFleetBefore = battle.FirstBattle.FleetsBeforeBattle.EnemyFleet!,
			EnemyMainFleetAfter = battle.LastBattle.FleetsAfterBattle.EnemyFleet!,

			EnemyEscortFleetBefore = battle.FirstBattle.FleetsBeforeBattle.EnemyEscortFleet,
			EnemyEscortFleetAfter = battle.LastBattle.FleetsAfterBattle.EnemyEscortFleet,
		};

		Assert.Equal(BattleRanks.SS, prediction.PredictRank());

		// Early Spring 2023 - E2-O
		battle = (BattleNode)sortieDetails[0].Nodes.Last();

		prediction = new NormalBattleRankPrediction()
		{
			FriendlyMainFleetBefore = battle.FirstBattle.FleetsBeforeBattle.Fleet,
			FriendlyMainFleetAfter = battle.LastBattle.FleetsAfterBattle.Fleet,

			FriendlyEscortFleetBefore = battle.FirstBattle.FleetsBeforeBattle.EscortFleet,
			FriendlyEscortFleetAfter = battle.LastBattle.FleetsAfterBattle.EscortFleet,

			EnemyMainFleetBefore = battle.FirstBattle.FleetsBeforeBattle.EnemyFleet!,
			EnemyMainFleetAfter = battle.LastBattle.FleetsAfterBattle.EnemyFleet!,

			EnemyEscortFleetBefore = battle.FirstBattle.FleetsBeforeBattle.EnemyEscortFleet,
			EnemyEscortFleetAfter = battle.LastBattle.FleetsAfterBattle.EnemyEscortFleet,
		};

		Assert.Equal(BattleRanks.S, prediction.PredictRank());
	}

	[Fact(DisplayName = "Air raid with only escort getting damaged, rank should be A")]
	public async Task SortieDetailTest4()
	{
		List<SortieDetailViewModel> sortieDetails = await MakeSortieDetails("BattleRankTest04.json");

		Assert.Single(sortieDetails);
		Assert.True(sortieDetails[0].Nodes.Count > 2);

		// Early Spring 2023 - E3-O
		BattleNode battle = (BattleNode)sortieDetails[0].Nodes[2];

		BattleRankPrediction prediction = new AirRaidBattleRankPrediction()
		{
			FriendlyMainFleetBefore = battle.FirstBattle.FleetsBeforeBattle.Fleet,
			FriendlyMainFleetAfter = battle.LastBattle.FleetsAfterBattle.Fleet,

			FriendlyEscortFleetBefore = battle.FirstBattle.FleetsBeforeBattle.EscortFleet,
			FriendlyEscortFleetAfter = battle.LastBattle.FleetsAfterBattle.EscortFleet,

			EnemyMainFleetBefore = battle.FirstBattle.FleetsBeforeBattle.EnemyFleet!,
			EnemyMainFleetAfter = battle.LastBattle.FleetsAfterBattle.EnemyFleet!,

			EnemyEscortFleetBefore = battle.FirstBattle.FleetsBeforeBattle.EnemyEscortFleet,
			EnemyEscortFleetAfter = battle.LastBattle.FleetsAfterBattle.EnemyEscortFleet,
		};

		Assert.Equal(BattleRanks.A, prediction.PredictRank());
	}

	[Fact(DisplayName = "Everythign dies in opening + jets, rank should be SS")]
	public async Task SortieDetailTest5()
	{
		List<SortieDetailViewModel> sortieDetails = await MakeSortieDetails("BattleRankTest05.json");

		Assert.Single(sortieDetails);
		Assert.True(sortieDetails[0].Nodes.Count >1);

		// 1-1 Boss
		BattleNode battle = (BattleNode)sortieDetails[0].Nodes[1];

		BattleRankPrediction prediction = new NormalBattleRankPrediction()
		{
			FriendlyMainFleetBefore = battle.FirstBattle.FleetsBeforeBattle.Fleet,
			FriendlyMainFleetAfter = battle.LastBattle.FleetsAfterBattle.Fleet,

			FriendlyEscortFleetBefore = battle.FirstBattle.FleetsBeforeBattle.EscortFleet,
			FriendlyEscortFleetAfter = battle.LastBattle.FleetsAfterBattle.EscortFleet,

			EnemyMainFleetBefore = battle.FirstBattle.FleetsBeforeBattle.EnemyFleet!,
			EnemyMainFleetAfter = battle.LastBattle.FleetsAfterBattle.EnemyFleet!,

			EnemyEscortFleetBefore = battle.FirstBattle.FleetsBeforeBattle.EnemyEscortFleet,
			EnemyEscortFleetAfter = battle.LastBattle.FleetsAfterBattle.EnemyEscortFleet,
		};

		Assert.Equal(BattleRanks.SS, prediction.PredictRank());
	}

	/// <summary>
	/// https://twitter.com/nao_truk/status/1765961898955284520/photo/1
	/// </summary>
	/// <returns></returns>
	[Fact(DisplayName = "Case with retreated ships affecting the HP rates, rank should be C")]
	public void SortieDetailTest6()
	{
		BattleRankPrediction prediction = new NormalBattleRankPrediction()
		{
			FriendlyMainFleetBefore = new FleetDataMock()
			{
				MembersInstance = new(
				[
					new ShipDataMock(Database.MasterShips[ShipId.NisshinA])
					{
						HPCurrent = 55,
					},
					new ShipDataMock(Database.MasterShips[ShipId.MakigumoKaiNi])
					{
						HPCurrent = 38,
					},
					new ShipDataMock(Database.MasterShips[ShipId.KasumiKaiNi])
					{
						HPCurrent = 37,
					},
					new ShipDataMock(Database.MasterShips[ShipId.UmikazeKaiNi])
					{
						HPCurrent = 37,
					},
					new ShipDataMock(Database.MasterShips[ShipId.KawakazeKaiNi])
					{
						HPCurrent = 37,
					},
					new ShipDataMock(Database.MasterShips[ShipId.YuraKaiNi])
					{
						HPCurrent = 10,
					},
				]),
				EscapedShipList = new([5]), // Yura escaped
			},
			FriendlyMainFleetAfter = new FleetDataMock()
			{
				MembersInstance = new(
				[
					new ShipDataMock(Database.MasterShips[ShipId.NisshinA])
					{
						HPCurrent = 55,
					},
					new ShipDataMock(Database.MasterShips[ShipId.MakigumoKaiNi])
					{
						HPCurrent = 38,
					},
					new ShipDataMock(Database.MasterShips[ShipId.KasumiKaiNi])
					{
						HPCurrent = 16,
					},
					new ShipDataMock(Database.MasterShips[ShipId.UmikazeKaiNi])
					{
						HPCurrent = 37,
					},
					new ShipDataMock(Database.MasterShips[ShipId.KawakazeKaiNi])
					{
						HPCurrent = 9,
					},
					new ShipDataMock(Database.MasterShips[ShipId.YuraKaiNi])
					{
						HPCurrent = 10,
					},
				]),
				EscapedShipList = new([5]), // Yura escaped
			},

			FriendlyEscortFleetBefore = new FleetDataMock()
			{
				MembersInstance = new(
				[
					new ShipDataMock(Database.MasterShips[ShipId.YuubariKaiNiToku])
					{
						HPCurrent = 10,
					},
					new ShipDataMock(Database.MasterShips[ShipId.AbukumaKaiNi])
					{
						HPCurrent = 51,
					},
					new ShipDataMock(Database.MasterShips[ShipId.HatsushimoKaiNi])
					{
						HPCurrent = 17,
					},
					new ShipDataMock(Database.MasterShips[ShipId.AmatsukazeKaiNi])
					{
						HPCurrent = 40,
					},
					new ShipDataMock(Database.MasterShips[ShipId.KawakazeKaiNi])
					{
						HPCurrent = 37,
					},
					new ShipDataMock(Database.MasterShips[ShipId.YuraKaiNi])
					{
						HPCurrent = 43,
					},
				]),
				EscapedShipList = new([3]), // Amatsukaze escaped
			},
			FriendlyEscortFleetAfter = new FleetDataMock()
			{
				MembersInstance = new(
				[
					new ShipDataMock(Database.MasterShips[ShipId.YuubariKaiNiToku])
					{
						HPCurrent = 10,
					},
					new ShipDataMock(Database.MasterShips[ShipId.AbukumaKaiNi])
					{
						HPCurrent = 7,
					},
					new ShipDataMock(Database.MasterShips[ShipId.HatsushimoKaiNi])
					{
						HPCurrent = 8,
					},
					new ShipDataMock(Database.MasterShips[ShipId.AmatsukazeKaiNi])
					{
						HPCurrent = 40,
					},
					new ShipDataMock(Database.MasterShips[ShipId.KawakazeKaiNi])
					{
						HPCurrent = 34,
					},
					new ShipDataMock(Database.MasterShips[ShipId.YuraKaiNi])
					{
						HPCurrent = 43,
					},
				]),
				EscapedShipList = new([3]), // Amatsukaze escaped
			},

			EnemyMainFleetBefore = new FleetDataMock()
			{
				MembersInstance = new(
				[
					new ShipDataMock(Database.MasterShips[(ShipId)1527]) // Ri-Class Flagship
					{
						HPCurrent = 76,
					},
					new ShipDataMock(Database.MasterShips[(ShipId)1527]) // Ri-Class Flagship
					{
						HPCurrent = 76,
					},
					new ShipDataMock(Database.MasterShips[(ShipId)1591]) // Tsu-Class 
					{
						HPCurrent = 48,
					},
					new ShipDataMock(Database.MasterShips[(ShipId)1527]) // Ni-Class Late Model
					{
						HPCurrent = 40,
					},
					new ShipDataMock(Database.MasterShips[(ShipId)1527]) // Ni-Class Late Model
					{
						HPCurrent = 40,
					},
					new ShipDataMock(Database.MasterShips[(ShipId)1527]) // Ni-Class Late Model
					{
						HPCurrent = 40,
					},
				]),
			},
			EnemyMainFleetAfter = new FleetDataMock()
			{
				MembersInstance = new(
				[
					new ShipDataMock(Database.MasterShips[(ShipId)1527]) // Ri-Class Flagship
					{
						HPCurrent = 44,
					},
					new ShipDataMock(Database.MasterShips[(ShipId)1527]) // Ri-Class Flagship
					{
						HPCurrent = 32,
					},
					new ShipDataMock(Database.MasterShips[(ShipId)1591]) // Tsu-Class 
					{
						HPCurrent = 35,
					},
					new ShipDataMock(Database.MasterShips[(ShipId)1527]) // Ni-Class Late Model
					{
						HPCurrent = 0,
					},
					new ShipDataMock(Database.MasterShips[(ShipId)1527]) // Ni-Class Late Model
					{
						HPCurrent = 0,
					},
					new ShipDataMock(Database.MasterShips[(ShipId)1527]) // Ni-Class Late Model
					{
						HPCurrent = 0,
					},
				]),
			},
		};

		Assert.Equal(BattleRanks.C, prediction.PredictRank());
	}

	[Fact(DisplayName = "LBAS raid - no damage")]
	public async Task SortieDetailTest7()
	{
		List<SortieDetailViewModel> sortieDetails = await MakeSortieDetails("BattleRankTest06.json");

		Assert.Single(sortieDetails);
		Assert.True(sortieDetails[0].Nodes.Count > 2);
		Assert.NotNull(sortieDetails[0].Nodes[2].AirBaseRaid);

		// Raid on 3rd node
		BattleBaseAirRaid battle = sortieDetails[0].Nodes[2].AirBaseRaid!;

		BattleRankPrediction prediction = new BaseAirRaidBattleRankPrediction()
		{
			AirBaseBeforeAfter = battle.AirBaseBeforeAfter,

			EnemyMainFleetBefore = battle.FleetsBeforeBattle.EnemyFleet!,
			EnemyMainFleetAfter = battle.FleetsAfterBattle.EnemyFleet!,

			EnemyEscortFleetBefore = battle.FleetsBeforeBattle.EnemyEscortFleet,
			EnemyEscortFleetAfter = battle.FleetsAfterBattle.EnemyEscortFleet,
		};

		prediction.PredictRank();

		Assert.Equal(600, prediction.FriendlyHpBefore);
		Assert.Equal(600, prediction.FriendlyHpAfter);
		Assert.Equal(0, prediction.FriendHpRate);
	}

	[Fact(DisplayName = "LBAS raid - with damage")]
	public async Task SortieDetailTest8()
	{
		List<SortieDetailViewModel> sortieDetails = await MakeSortieDetails("BattleRankTest07.json");

		Assert.Single(sortieDetails);
		Assert.True(sortieDetails[0].Nodes.Count > 2);
		Assert.NotNull(sortieDetails[0].Nodes[2].AirBaseRaid);

		// Raid on 3rd node
		BattleBaseAirRaid battle = sortieDetails[0].Nodes[2].AirBaseRaid!;

		BattleRankPrediction prediction = new BaseAirRaidBattleRankPrediction()
		{
			AirBaseBeforeAfter = battle.AirBaseBeforeAfter,

			EnemyMainFleetBefore = battle.FleetsBeforeBattle.EnemyFleet!,
			EnemyMainFleetAfter = battle.FleetsAfterBattle.EnemyFleet!,

			EnemyEscortFleetBefore = battle.FleetsBeforeBattle.EnemyEscortFleet,
			EnemyEscortFleetAfter = battle.FleetsAfterBattle.EnemyEscortFleet,
		};

		prediction.PredictRank();

		Assert.Equal(600, prediction.FriendlyHpBefore);
		Assert.Equal(406, prediction.FriendlyHpAfter);
		Assert.Equal(0.32, prediction.FriendHpRate, 2);
	}
}
