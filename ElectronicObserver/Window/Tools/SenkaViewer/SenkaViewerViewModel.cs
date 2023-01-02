using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Database;
using ElectronicObserver.Database.KancolleApi;
using ElectronicObserver.KancolleApi.Types;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.Battleresult;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Next;
using ElectronicObserver.KancolleApi.Types.ApiReqMission.Result;
using ElectronicObserver.KancolleApi.Types.ApiReqPractice.BattleResult;
using ElectronicObserver.KancolleApi.Types.ApiReqQuest.Clearitemget;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Battleresult;
using ElectronicObserver.Properties.Window.Dialog;
using ElectronicObserverTypes;
using Microsoft.EntityFrameworkCore;

namespace ElectronicObserver.Window.Tools.SenkaViewer;

public partial class SenkaViewerViewModel : WindowViewModelBase
{
	private ElectronicObserverContext Db { get; } = new();
	public SenkaViewerTranslationViewModel SenkaViewer { get; }

	public List<SenkaRecord> SenkaRecords { get; set; } = new();
	public List<SenkaRecord> SelectedSenkaRecords { get; set; } = new();

	private List<SenkaRecord> TotalCalculationList => SelectedSenkaRecords switch
	{
		{ Count: > 0 } => SelectedSenkaRecords,
		_ => SenkaRecords,
	};

	public string TotalSenka => $"{SenkaViewer.Senka}：{TotalCalculationList.Sum(s => s.TotalSenkaGains):0.##}";
	public string TotalNormalSenka => $"{SenkaViewer.NormalSenka}：{TotalCalculationList.Sum(s => s.HqExpSenkaGains):0.##}";
	public string TotalExtraOperationSenka => $"{SenkaViewer.ExtraOperationSenka}：{TotalCalculationList.Sum(s => s.ExtraOperationSenkaGains):0.##}";
	public string TotalQuestSenka => $"{SenkaViewer.QuestSenka}：{TotalCalculationList.Sum(s => s.QuestSenkaGains):0.##}";

	private DateTime DateTimeBegin => new(DateBegin.Year, DateBegin.Month, DateBegin.Day, TimeBegin.Hour, TimeBegin.Minute, TimeBegin.Second);
	private DateTime DateTimeEnd => new(DateEnd.Year, DateEnd.Month, DateEnd.Day, TimeEnd.Hour, TimeEnd.Minute, TimeEnd.Second);

	public DateTime DateBegin { get; set; }
	public DateTime TimeBegin { get; set; }
	public DateTime DateEnd { get; set; }
	public DateTime TimeEnd { get; set; }
	public DateTime MinDate { get; set; }
	public DateTime MaxDate { get; set; }

	public string Today => $"{DialogDropRecordViewer.Today}: {DateTime.Now:yyyy/MM/dd}";

	public SenkaViewerViewModel()
	{
		SenkaViewer = Ioc.Default.GetRequiredService<SenkaViewerTranslationViewModel>();

		MinDate = Db.ApiFiles.Min(f => f.TimeStamp);
		MaxDate = DateTime.Now.AddDays(1);

		DateBegin = MinDate.Date;
		DateEnd = MaxDate.Date;
	}

	[RelayCommand]
	private void Search()
	{
		List<ApiFile> apiFiles = Db.ApiFiles
			.AsNoTracking()
			.Where(f => f.ApiFileType == ApiFileType.Response)
			.Where(f => f.TimeStamp > DateTimeBegin.ToUniversalTime())
			.Where(f => f.TimeStamp < DateTimeEnd.ToUniversalTime())
			.Where(f =>
				f.Name == "api_req_sortie/battleresult" ||
				f.Name == "api_req_combined_battle/battleresult" ||
				f.Name == "api_req_practice/battle_result" ||
				f.Name == "api_req_mission/result" ||
				f.Name == "api_req_map/next" ||
				f.Name == "api_req_quest/clearitemget")
			.ToList();

		if (!apiFiles.Any()) return;

		DateTime start = apiFiles.Min(f => f.TimeStamp);
		start = DateTime.SpecifyKind(start, DateTimeKind.Utc);

		DateTime end = apiFiles.Max(f => f.TimeStamp);
		end = DateTime.SpecifyKind(end, DateTimeKind.Utc);

		List<SenkaRecord> senkaRecords = GenerateSenkaRecords(start, end);

		foreach (SenkaRecord senkaRecord in senkaRecords)
		{
			senkaRecord.HqExpSenkaGains = apiFiles
				.Where(f => f.TimeStamp > senkaRecord.Start)
				.Where(f => f.TimeStamp < senkaRecord.End)
				.Sum(GetHqExp) * 7 / 10000;

			senkaRecord.ExtraOperationSenkaGains = apiFiles
				.Where(f => f.TimeStamp > senkaRecord.Start)
				.Where(f => f.TimeStamp < senkaRecord.End)
				.Sum(GetExtraOperationSenka);

			senkaRecord.QuestSenkaGains = apiFiles
				.Where(f => f.TimeStamp > senkaRecord.Start)
				.Where(f => f.TimeStamp < senkaRecord.End)
				.Sum(GetQuestSenka);
		}

		SenkaRecords = senkaRecords;
	}

	// 05:00 UTC = 14:00 JST
	// 13:00 UTC = 22:00 JST
	// 17:00 UTC = 02:00 JST
	public static List<SenkaRecord> GenerateSenkaRecords(DateTime start, DateTime end)
	{
		DateTime endOfMonth = new(start.Year, start.Month, DateTime.DaysInMonth(start.Year, start.Month), 13, 0, 0, DateTimeKind.Utc);

		start = start.Hour switch
		{
			_ when start > endOfMonth => new(start.Year, start.Month, start.Day, 13, 0, 0, DateTimeKind.Utc),
			< 5 => new(start.Year, start.Month, start.Day - 1, 17, 0, 0, DateTimeKind.Utc),
			< 17 => new(start.Year, start.Month, start.Day, 5, 0, 0, DateTimeKind.Utc),
			_ => new(start.Year, start.Month, start.Day, 17, 0, 0, DateTimeKind.Utc),
		};

		end = end.Hour switch
		{
			> 17 => new(end.Year, end.Month, end.Day + 1, 5, 0, 0, DateTimeKind.Utc),
			> 5 => new(end.Year, end.Month, end.Day, 17, 0, 0, DateTimeKind.Utc),
			_ => new(end.Year, end.Month, end.Day, 5, 0, 0, DateTimeKind.Utc),
		};

		if (start >= endOfMonth)
		{
			endOfMonth = GetNextEndOfMonth(endOfMonth);
		}

		List<SenkaRecord> senkaRecords = new();

		while (start < end)
		{
			DateTime currentEnd = start.AddHours(start.Hour switch
			{
				5 or 17 => 12,
				13 => 4,
			});

			if (currentEnd > endOfMonth)
			{
				currentEnd = endOfMonth;
				endOfMonth = GetNextEndOfMonth(endOfMonth);
			}

			senkaRecords.Insert(0, new()
			{
				Start = start,
				End = currentEnd,
			});

			start = currentEnd;
		}

		return senkaRecords;
	}

	private static DateTime GetNextEndOfMonth(DateTime reference)
	{
		DateTime firstDayThisMonth = new DateTime(reference.Year, reference.Month, 1);
		DateTime firstDayPlusTwoMonths = firstDayThisMonth.AddMonths(2);
		DateTime lastDayNextMonth = firstDayPlusTwoMonths.AddDays(-1);

		return new(lastDayNextMonth.Year, lastDayNextMonth.Month, lastDayNextMonth.Day, 22, 0, 0);
	}

	private double GetHqExp(ApiFile apiFile) => apiFile.Name switch
	{
		"api_req_sortie/battleresult" => JsonSerializer.Deserialize<ApiResponse<ApiReqSortieBattleresultResponse>>(apiFile.Content)?.ApiData.ApiGetExp ?? 0,
		"api_req_combined_battle/battleresult" => JsonSerializer.Deserialize<ApiResponse<ApiReqCombinedBattleBattleresultResponse>>(apiFile.Content)?.ApiData.ApiGetExp ?? 0,
		"api_req_practice/battle_result" => JsonSerializer.Deserialize<ApiResponse<ApiReqPracticeBattleResultResponse>>(apiFile.Content)?.ApiData.ApiGetExp ?? 0,

		"api_req_mission/result" => JsonSerializer.Deserialize<ApiResponse<ApiReqMissionResultResponse>>(apiFile.Content)?.ApiData.ApiGetExp ?? 0,

		_ => 0,
	};

	private double GetExtraOperationSenka(ApiFile apiFile) => apiFile.Name switch
	{
		"api_req_sortie/battleresult" => JsonSerializer.Deserialize<ApiResponse<ApiReqSortieBattleresultResponse>>(apiFile.Content)?.ApiData.ApiGetExmapRate switch
		{
			JsonElement { ValueKind: JsonValueKind.Number } i => i.GetInt32(),
			JsonElement { ValueKind: JsonValueKind.String } s => int.Parse(s.GetString()!),
			_ => 0,
		},
		"api_req_combined_battle/battleresult" => JsonSerializer.Deserialize<ApiResponse<ApiReqCombinedBattleBattleresultResponse>>(apiFile.Content)?.ApiData.ApiGetExmapRate switch
		{
			JsonElement { ValueKind: JsonValueKind.Number } i => i.GetInt32(),
			JsonElement { ValueKind: JsonValueKind.String } s => int.Parse(s.GetString()!),
			_ => 0,
		},
		"api_req_map/next" => JsonSerializer.Deserialize<ApiResponse<ApiReqMapNextResponse>>(apiFile.Content)?.ApiData.ApiGetEoRate ?? 0,

		_ => 0,
	};

	private double GetQuestSenka(ApiFile apiFile) => apiFile.Name switch
	{
		"api_req_quest/clearitemget" => JsonSerializer.Deserialize<ApiResponse<ApiReqQuestClearitemgetResponse>>(apiFile.Content)?.ApiData.ApiBounus
			.FirstOrDefault(b => b.ApiType == UseItemId.Senka)
			?.ApiCount?? 0,

		_ => 0,
	};

	[RelayCommand]
	private void SelectToday(Calendar? calendar)
	{
		if (calendar is null) return;

		calendar.SelectedDate = DateTime.Now.Date;
	}
}
