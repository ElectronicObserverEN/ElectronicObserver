﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using ElectronicObserver.Data.Battle;
using ElectronicObserver.Database.Sortie;
using ElectronicObserver.Database;
using ElectronicObserver.Services;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Node;
using ElectronicObserver.Window.Tools.SortieRecordViewer.SortieDetail;
using ElectronicObserver.Window.Tools.SortieRecordViewer;
using ElectronicObserverTypes;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System;

namespace ElectronicObserverCoreTests.BattleRank;

public class BattleRankTests
{
	private static string DirectoryName => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
	private static string RelativePath => "BattleRank";
	private static string BasePath => Path.Join(DirectoryName, RelativePath);

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
			.Select(s => new SortieRecordViewModel(s, s.ApiFiles.Select(f => f.TimeStamp).Min()))
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
		Assert.True(sortieDetails[0].Nodes.Count > 1);

		// Early Spring 2023 - Edge 15  
		BattleNode battle = (BattleNode)sortieDetails[0].Nodes[4];

		BattleRankPrediction prediction = new()
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

		Assert.Equal(ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums.BattleRank.S, prediction.PredictRank());
	}

	[Fact(DisplayName = "Battle against untargetable enemy, rank should be SS")]
	public async Task SortieDetailTest2()
	{
		List<SortieDetailViewModel> sortieDetails = await MakeSortieDetails("BattleRankTest02.json");

		Assert.Single(sortieDetails);
		Assert.True(sortieDetails[0].Nodes.Count > 1);

		// Early Spring 2023 - Edge 15  
		BattleNode battle = (BattleNode)sortieDetails[0].Nodes[4];

		BattleRankPrediction prediction = new()
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

		Assert.Equal(ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums.BattleRank.SS, prediction.PredictRank());
	}
}
