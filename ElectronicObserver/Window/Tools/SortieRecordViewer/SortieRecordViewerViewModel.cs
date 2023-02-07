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
using ElectronicObserver.Database.Sortie;
using ElectronicObserver.KancolleApi.Types;
using ElectronicObserver.KancolleApi.Types.ApiPort.Port;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.Battleresult;
using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.Properties.Window.Dialog;
using ElectronicObserver.Services;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Window.Tools.FleetImageGenerator;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Replay;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Data;
using ElectronicObserverTypes.Mocks;
using ElectronicObserverTypes.Serialization.DeckBuilder;
using ElectronicObserverTypes.Serialization.FitBonus;
using Microsoft.EntityFrameworkCore;

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
	private void CopyReplayToClipboard(SortieRecordViewModel? sortie)
	{
		if (sortie is null) return;

		if (!sortie.Model.ApiFiles.Any())
		{
			sortie.Model.ApiFiles = Db.ApiFiles
				.Where(f => f.SortieRecordId == sortie.Model.Id)
				.ToList();
		}

		ReplayData replay = sortie.Model.ToReplayData();

		replay.Battles = new();

		ReplayBattle battle = new();

		foreach (ApiFile apiFile in sortie.Model.ApiFiles.Where(f => f.ApiFileType is ApiFileType.Response))
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
				battle.FirstBattle = apiFile.GetResponseApiData();
			}

			if (IsSecondBattleApi(apiFile.Name))
			{
				battle.SecondBattle = apiFile.GetResponseApiData();
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
	private void OpenFleetImageGenerator(SortieRecordViewModel? sortie)
	{
		if (sortie is null) return;

		int hqLevel = KCDatabase.Instance.Admiral.Level;

		if (sortie.Model.ApiFiles.Any())
		{
			// get the last port response right before the sortie started
			ApiFile? portFile = Db.ApiFiles
				.Where(f => f.ApiFileType == ApiFileType.Response)
				.Where(f => f.Name == "api_port/port")
				.Where(f => f.TimeStamp < sortie.Model.ApiFiles.First().TimeStamp)
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
			sortie.Model.FleetData.Fleets.Skip(0).FirstOrDefault().MakeFleet(),
			sortie.Model.FleetData.Fleets.Skip(1).FirstOrDefault().MakeFleet(),
			sortie.Model.FleetData.Fleets.Skip(2).FirstOrDefault().MakeFleet(),
			sortie.Model.FleetData.Fleets.Skip(3).FirstOrDefault().MakeFleet(),
			sortie.Model.FleetData.AirBases.Skip(0).FirstOrDefault().MakeAirBase(),
			sortie.Model.FleetData.AirBases.Skip(1).FirstOrDefault().MakeAirBase(),
			sortie.Model.FleetData.AirBases.Skip(2).FirstOrDefault().MakeAirBase()
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
}
