using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Data;
using ElectronicObserver.Database;
using ElectronicObserver.Database.KancolleApi;
using ElectronicObserver.KancolleApi.Types;
using ElectronicObserver.KancolleApi.Types.ApiPort.Port;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.Battleresult;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Start;
using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.Properties.Window.Dialog;
using ElectronicObserver.Services;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Tools.FleetImageGenerator;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Replay;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Node;
using ElectronicObserver.Window.Tools.SortieRecordViewer.SortieDetail;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Attacks;
using ElectronicObserverTypes.Serialization.DeckBuilder;
using Microsoft.EntityFrameworkCore;
using DayAttack = ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase.DayAttack;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer;

public partial class SortieRecordViewerViewModel : WindowViewModelBase
{
	private ToolService ToolService { get; }
	private DataSerializationService DataSerializationService { get; }
	private ElectronicObserverContext Db { get; } = new();

	public SortieRecordViewerTranslationViewModel SortieRecordViewer { get; }

	private static string AllRecords { get; } = "*";

	public List<object> Worlds { get; }
	public List<object> Maps { get; }

	public object World { get; set; } = AllRecords;
	public object Map { get; set; } = AllRecords;

	private DateTime DateTimeBegin =>
		new(DateBegin.Year, DateBegin.Month, DateBegin.Day, TimeBegin.Hour, TimeBegin.Minute, TimeBegin.Second);
	private DateTime DateTimeEnd =>
		new(DateEnd.Year, DateEnd.Month, DateEnd.Day, TimeEnd.Hour, TimeEnd.Minute, TimeEnd.Second);

	public DateTime DateBegin { get; set; }
	public DateTime TimeBegin { get; set; }
	public DateTime DateEnd { get; set; }
	public DateTime TimeEnd { get; set; }
	public DateTime MinDate { get; set; }
	public DateTime MaxDate { get; set; }

	public string Today => $"{DialogDropRecordViewer.Today}: {DateTime.Now:yyyy/MM/dd}";

	public ObservableCollection<SortieRecordViewModel> Sorties { get; } = new();

	public SortieRecordViewModel? SelectedSortie { get; set; }

	public ObservableCollection<SortieRecordViewModel> SelectedSorties { get; set; } = new();

	public string? StatusBarText { get; set; }

	public SortieRecordViewerViewModel()
	{
		ToolService = Ioc.Default.GetRequiredService<ToolService>();
		DataSerializationService = Ioc.Default.GetRequiredService<DataSerializationService>();
		SortieRecordViewer = Ioc.Default.GetRequiredService<SortieRecordViewerTranslationViewModel>();

		Db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

		MinDate = Db.Sorties
			.Include(s => s.ApiFiles)
			.OrderBy(s => s.Id)
			.FirstOrDefault()
			?.ApiFiles.Min(f => f.TimeStamp)
			.ToLocalTime() ?? DateTime.Now;

		MaxDate = DateTime.Now.AddDays(1);

		DateBegin = MinDate.Date;
		DateEnd = MaxDate.Date;

		Worlds = Db.Worlds
			.Select(w => w.Id)
			.Distinct()
			.ToList()
			.Cast<object>()
			.Prepend(AllRecords)
			.ToList();

		Maps = Db.Maps
			.Select(m => m.MapId)
			.Distinct()
			.ToList()
			.Cast<object>()
			.Prepend(AllRecords)
			.ToList();

		SelectedSorties.CollectionChanged += (sender, args) =>
		{
			StatusBarText = string.Format(SortieRecordViewer.SelectedItems, SelectedSorties.Count, Sorties.Count);
		};
	}

	[RelayCommand]
	private void Search()
	{
		Sorties.Clear();

		List<SortieRecordViewModel> sorties = Db.Sorties
			.Where(s => World as string == AllRecords || s.World == World as int?)
			.Where(s => Map as string == AllRecords || s.Map == Map as int?)
			.Select(s => new
			{
				SortieRecord = s,
				s.ApiFiles.OrderBy(f => f.TimeStamp).First().TimeStamp,
			})
			.Where(s => s.TimeStamp > DateTimeBegin.ToUniversalTime())
			.Where(s => s.TimeStamp < DateTimeEnd.ToUniversalTime())
			.AsEnumerable()
			.Select(s => new SortieRecordViewModel(s.SortieRecord, s.TimeStamp))
			.OrderByDescending(s => s.Id)
			.ToList();

		foreach (SortieRecordViewModel sortie in sorties)
		{
			Sorties.Add(sortie);
		}
	}

	// normal battle - day
	// night node - battle
	// night to day - night
	// etc...
	private static bool IsFirstBattleApi(string name) => name is
		"api_req_sortie/battle" or // normal day
		"api_req_battle_midnight/sp_midnight" or // night node
		"api_req_sortie/airbattle" or // single air raid
		"api_req_sortie/ld_airbattle" or // single air raid
		"api_req_sortie/night_to_day" or // single night to day
		"api_req_sortie/ld_shooting" or // single fleet radar ambush
		"api_req_combined_battle/battle" or // combined normal
		"api_req_combined_battle/sp_midnight" or // combined night battle
		"api_req_combined_battle/airbattle" or // combined air exchange ?
		"api_req_combined_battle/battle_water" or // CTF TCF combined battle
		"api_req_combined_battle/ld_airbattle" or // air raid
		"api_req_combined_battle/ec_battle" or // CTF enemy combined battle
		"api_req_combined_battle/ec_night_to_day" or // enemy combined night to day
		"api_req_combined_battle/each_battle" or // STF combined vs combined
		"api_req_combined_battle/each_battle_water" or // STF combined
		"api_req_combined_battle/ld_shooting"; // combined radar ambush

	// normal battle - night
	// night to day - day
	// etc...
	private static bool IsSecondBattleApi(string name) => name is
		"api_req_battle_midnight/battle" or // normal night
		"api_req_combined_battle/midnight_battle" or // combined day to night
		"api_req_combined_battle/ec_midnight_battle"; // combined normal night battle

	private static bool IsBattleEndApi(string name) => name is
		"api_req_sortie/battleresult" or
		"api_req_combined_battle/battleresult" or
		"api_req_practice/battle_result";

	private static bool IsMapProgressApi(string name) => name is
		"api_req_map/start" or
		"api_req_map/next";

	[RelayCommand]
	private void CopyReplayToClipboard()
	{
		if (SelectedSortie is null) return;

		if (!SelectedSortie.Model.ApiFiles.Any())
		{
			SelectedSortie.Model.ApiFiles = Db.ApiFiles
				.Where(f => f.SortieRecordId == SelectedSortie.Model.Id)
				.ToList();
		}

		ReplayData replay = SelectedSortie.Model.ToReplayData();

		replay.Battles = new();

		ReplayBattle battle = new();

		foreach (ApiFile apiFile in SelectedSortie.Model.ApiFiles.Where(f => f.ApiFileType is ApiFileType.Response))
		{
			if (IsMapProgressApi(apiFile.Name))
			{
				IMapProgressApi? progress = apiFile.GetMapProgressApiData();

				if (progress is null)
				{
					// this shouldn't happen
					continue;
				}

				battle.Node = progress.ApiNo;
			}

			if (IsFirstBattleApi(apiFile.Name))
			{
				try
				{
					battle.FirstBattle = apiFile.GetResponseApiData();
				}
				catch (Exception e)
				{
					Logger.Add(2, SortieRecordViewer.FailedToParseApiData + e.StackTrace);
				}
			}

			if (IsSecondBattleApi(apiFile.Name))
			{
				try
				{
					battle.SecondBattle = apiFile.GetResponseApiData();
				}
				catch (Exception e)
				{
					Logger.Add(2, SortieRecordViewer.FailedToParseApiData + e.StackTrace);
				}
			}

			if (IsBattleEndApi(apiFile.Name))
			{
				ISortieBattleResultApi? result = apiFile.GetSortieBattleResultApi();

				if (result is null)
				{
					// this shouldn't happen
					continue;
				}

				battle.FirstBattle ??= new();
				battle.SecondBattle ??= new();
				battle.BaseExp = result.ApiGetBaseExp;
				battle.HqExp = result.ApiGetExp;
				battle.Drop = result.ApiGetShip?.ApiShipId ?? ShipId.Unknown;
				battle.Rating = result.ApiWinRank;
				battle.Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
				battle.Mvp = new() { result.ApiMvp, };

				if (result is ApiReqCombinedBattleBattleresultResponse combinedResult)
				{
					battle.Mvp.Add(combinedResult.ApiMvpCombined ?? -1);
				}

				replay.Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
				replay.Battles.Add(battle);
				battle = new();
			}
		}

		// there was battle data but no battle end
		if (battle.FirstBattle is not null || battle.SecondBattle is not null)
		{
			battle.FirstBattle ??= new();
			battle.SecondBattle ??= new();

			replay.Battles.Add(battle);
		}

		Clipboard.SetText(JsonSerializer.Serialize(replay));
	}

	[RelayCommand]
	private void OpenFleetImageGenerator()
	{
		if (SelectedSortie is null) return;

		int hqLevel = KCDatabase.Instance.Admiral.Level;

		if (SelectedSortie.Model.ApiFiles.Any())
		{
			// get the last port response right before the sortie started
			ApiFile? portFile = Db.ApiFiles
				.Where(f => f.ApiFileType == ApiFileType.Response)
				.Where(f => f.Name == "api_port/port")
				.Where(f => f.TimeStamp < SelectedSortie.Model.ApiFiles.First().TimeStamp)
				.OrderByDescending(f => f.TimeStamp)
				.FirstOrDefault();

			if (portFile is not null)
			{
				try
				{
					ApiPortPortResponse? port = JsonSerializer
						.Deserialize<ApiResponse<ApiPortPortResponse>>(portFile.Content)?.ApiData;

					if (port != null)
					{
						hqLevel = port.ApiBasic.ApiLevel;
					}
				}
				catch
				{
					// can probably ignore this
				}
			}
		}

		DeckBuilderData data = DataSerializationService.MakeDeckBuilderData
		(
			hqLevel,
			SelectedSortie.Model.FleetData.Fleets.Skip(0).FirstOrDefault().MakeFleet(),
			SelectedSortie.Model.FleetData.Fleets.Skip(1).FirstOrDefault().MakeFleet(),
			SelectedSortie.Model.FleetData.Fleets.Skip(2).FirstOrDefault().MakeFleet(),
			SelectedSortie.Model.FleetData.Fleets.Skip(3).FirstOrDefault().MakeFleet(),
			SelectedSortie.Model.FleetData.AirBases.Skip(0).FirstOrDefault().MakeAirBase(),
			SelectedSortie.Model.FleetData.AirBases.Skip(1).FirstOrDefault().MakeAirBase(),
			SelectedSortie.Model.FleetData.AirBases.Skip(2).FirstOrDefault().MakeAirBase()
		);

		FleetImageGeneratorImageDataModel model = new()
		{
			Fleet1Visible = data.Fleet1 is not null,
			Fleet2Visible = data.Fleet2 is not null,
			Fleet3Visible = data.Fleet3 is not null,
			Fleet4Visible = data.Fleet4 is not null,
		};

		ToolService.FleetImageGenerator(model, data);
	}

	[RelayCommand]
	private static void SelectToday(Calendar? calendar)
	{
		if (calendar is null) return;

		calendar.SelectedDate = DateTime.Now.Date;
	}

	[RelayCommand]
	private void ShowSortieDetails()
	{
		if (SelectedSortie is null) return;

		SortieDetailViewModel? sortieDetail = GenerateSortieDetailViewModel(SelectedSortie);

		if (sortieDetail is null) return;

		new SortieDetailWindow(sortieDetail).Show();

	}

	private static string CsvSeparator => ";";

	[RelayCommand]
	private void CopySmokerDataCsv()
	{
		List<(SortieDetailViewModel SortieDetail, BattleData Battle)> data = new();

		foreach (SortieRecordViewModel sortieRecord in SelectedSorties)
		{
			SortieDetailViewModel? sortieDetail = GenerateSortieDetailViewModel(sortieRecord);

			if (sortieDetail is null) return;

			BattleData? smokerBattle = null;

			foreach (BattleData battleData in sortieDetail.Nodes.OfType<BattleNode>().Select(n => n.FirstBattle))
			{
				bool activatedSmoker = battleData.Phases.OfType<PhaseSearching>().Any(p => p.SmokeCount > 0);

				if (activatedSmoker)
				{
					smokerBattle = battleData;
					break;
				}
			}

			if (smokerBattle is null) continue;

			data.Add((sortieDetail, smokerBattle));
		}

		List<string> csvData = new()
		{
			string.Join(CsvSeparator, new List<string>
			{
				"開始",
				"SN",
				"出撃回数",
				"煙幕",
				"自軍陣形",
				"敵軍陣形",
				"交戦形態",
				"フェーズ",
				"砲撃・雷撃",
				"攻撃艦_#",
				"攻撃艦",
				"防御艦_#",
				"防御艦",
				"Lv",
				"Cond",
				"回避",
				"CL",
				"自軍・敵軍",
			}),
		};
		int sampleNumber = 1;
		int sortieNumber = 1;

		foreach ((SortieDetailViewModel sortieDetail, BattleData battle) in data)
		{
			PhaseSearching searching = battle.Phases.OfType<PhaseSearching>().First();

			foreach (PhaseBase phase in battle.Phases.Where(p => p is PhaseShelling or PhaseTorpedo))
			{
				if (phase is PhaseShelling shelling)
				{
					foreach (PhaseShellingAttackViewModel attackDisplay in shelling.AttackDisplays)
					{
						foreach (DayAttack attack in attackDisplay.Attacks)
						{
							string csvLine = GetSmokerCsvLine(sortieDetail, searching, attackDisplay, attack, shelling.Title, sampleNumber, sortieNumber);
							csvData.Add(csvLine);
							sampleNumber++;
						}
					}
				}

				if (phase is PhaseTorpedo torpedo)
				{
					foreach (PhaseShellingAttackViewModel attackDisplay in torpedo.AttackDisplays)
					{
						foreach (DayAttack attack in attackDisplay.Attacks)
						{
							string csvLine = GetSmokerCsvLine(sortieDetail, searching, attackDisplay, attack, torpedo.Title, sampleNumber, sortieNumber);
							csvData.Add(csvLine);
							sampleNumber++;
						}
					}
				}
			}

			sortieNumber++;
		}

		Clipboard.SetText(string.Join("\n", csvData));
	}

	private static string GetSmokerCsvLine(SortieDetailViewModel sortieDetail, PhaseSearching searching,
		PhaseShellingAttackViewModel attackDisplay, DayAttack attack, string phaseTitle, int sampleNumber, int sortieNumber)
	{
		return string.Join(CsvSeparator, new List<string>
		{
			sortieDetail.StartTime?.ToLocalTime().ToString(),
			sampleNumber.ToString(),
			sortieNumber.ToString(),
			(searching.SmokeCount ?? 0).ToString(),
			Constants.GetFormation(searching.PlayerFormationType),
			Constants.GetFormation(searching.EnemyFormationType),
			Constants.GetEngagementForm(searching.EngagementType),
			phaseTitle,
			ElectronicObserverTypes.Attacks.DayAttack.AttackDisplay(attack.AttackKind),
			(attackDisplay.AttackerIndex.Index + 1).ToString(),
			attack.Attacker.Name,
			(attackDisplay.DefenderIndex.Index + 1).ToString(),
			attack.Defender.Name,
			attack.Defender.Level.ToString(),
			attack.Defender.Condition.ToString(),
			attack.Defender.EvasionTotal.ToString(),
			attack.CriticalFlag switch
			{
				HitType.Miss => "CL0",
				HitType.Hit => "CL1",
				HitType.Critical => "CL2",
				_ => throw new NotImplementedException(),
			},
			attackDisplay.AttackerIndex.FleetFlag switch
			{
				FleetFlag.Player => "自軍",
				FleetFlag.Enemy => "敵軍",
				_ => throw new NotImplementedException(),
			},
		});
	}

	private SortieDetailViewModel? GenerateSortieDetailViewModel(SortieRecordViewModel sortie)
	{
		try
		{
			if (!sortie.Model.ApiFiles.Any())
			{
				sortie.Model.ApiFiles = Db.ApiFiles
					.Where(f => f.SortieRecordId == sortie.Model.Id)
					.ToList();
			}

			List<IFleetData?> fleets = sortie.Model.FleetData.Fleets.Select(f => f.MakeFleet()).ToList();
			bool isCombinedFleet = sortie.Model.FleetData.CombinedFlag > 0;
			List<IBaseAirCorpsData> airBases = sortie.Model.FleetData.AirBases.Select(f => f.MakeAirBase()).ToList();

			ApiFile startRequestFile = sortie.Model.ApiFiles
				.First(f => f.ApiFileType is ApiFileType.Request && f.Name is "api_req_map/start");

			ApiReqMapStartRequest startRequest = JsonSerializer
				.Deserialize<ApiReqMapStartRequest>(startRequestFile.Content)
				?? throw new Exception();

			// fleetId is 1 based, so need to do -1 when fetching data from fleets
			if (!int.TryParse(startRequest.ApiDeckId, out int fleetId)) throw new Exception();
			if (fleets.Skip(fleetId - 1).FirstOrDefault() is not IFleetData fleet) throw new Exception();

			IFleetData? escortFleet = isCombinedFleet switch
			{
				true => fleets.Skip(fleetId).FirstOrDefault(),
				_ => null,
			};

			SortieDetailViewModel sortieDetail = new(sortie.World, sortie.Map, new(fleet, escortFleet, fleets, airBases));

			// todo: battle requests contain a flag if smoke screen was activated
			foreach (ApiFile apiFile in sortie.Model.ApiFiles.Where(f => f.ApiFileType is ApiFileType.Response))
			{
				sortieDetail.StartTime ??= apiFile.TimeStamp;

				object? battleData = apiFile.ApiFileType switch
				{
					ApiFileType.Response => apiFile.GetResponseApiData(),
					ApiFileType.Request => apiFile.GetRequestApiData(),
					_ => throw new NotImplementedException("Unknown api file type."),
				};

				if (battleData is null) continue;

				sortieDetail.AddApiFile(battleData);
			}

			sortieDetail.EnsureApiFilesProcessed();

			return sortieDetail;
		}
		catch (Exception e)
		{
			Logger.Add(2, "Failed to load sortie details: " + e.Message + e.StackTrace);
		}

		return null;
	}
}
