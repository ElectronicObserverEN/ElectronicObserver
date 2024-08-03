using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Common.Datagrid;
using ElectronicObserver.Data;
using ElectronicObserver.Database;
using ElectronicObserver.Database.KancolleApi;
using ElectronicObserver.KancolleApi.Types;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.Battleresult;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Next;
using ElectronicObserver.KancolleApi.Types.ApiReqMission.Result;
using ElectronicObserver.KancolleApi.Types.ApiReqPractice.BattleResult;
using ElectronicObserver.KancolleApi.Types.ApiReqQuest.Clearitemget;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Battleresult;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Utility;
using ElectronicObserverTypes;
using Microsoft.EntityFrameworkCore;

namespace ElectronicObserver.Window.Tools.SenkaViewer;

public partial class SenkaViewerViewModel : WindowViewModelBase
{
	private ElectronicObserverContext Db { get; } = new();
	private ResourceRecord ResourceRecord { get; }
	public SenkaViewerTranslationViewModel SenkaViewer { get; }

	public List<SenkaRecord> SelectedSenkaRecords { get; set; } = new();

	private List<SenkaRecord> TotalCalculationList => SelectedSenkaRecords switch
	{
		{ Count: > 0 } => SelectedSenkaRecords,
		_ => DataGridViewModel.ItemsSource.ToList(),
	};

	public string TotalSenka => $"{SenkaViewer.Senka}：{TotalCalculationList.Sum(s => s.TotalSenkaGains):0.##}";
	public string TotalNormalSenka => $"{SenkaViewer.NormalSenka}：{TotalCalculationList.Sum(s => s.EstimatedHqExpSenkaGains):0.##}";
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

	public string Today => $"{DropRecordViewerResources.Today}: {DateTime.Now:yyyy/MM/dd}";

	public DataGridViewModel<SenkaRecord> DataGridViewModel { get; } = new();

	private DateTime SearchStartTime { get; set; }
	public string? StatusBarText { get; private set; }

	private static Func<ElectronicObserverContext, Task<DateTime>> MinDateQuery { get; } = EF
		.CompileAsyncQuery((ElectronicObserverContext db) => db.ApiFiles.Min(f => f.TimeStamp));

	public SenkaViewerViewModel()
	{
		SenkaViewer = Ioc.Default.GetRequiredService<SenkaViewerTranslationViewModel>();
		ResourceRecord = RecordManager.Instance.Resource;

		MinDate = new(DateTime.Now.Year, DateTime.Now.Month, 1);
		MaxDate = DateTime.Now.AddDays(1);

		DateBegin = MinDate.Date;
		DateEnd = MaxDate.Date;

		Task.Run(async () =>
		{
			ElectronicObserverContext db = new();

			DateTime minDate = await MinDateQuery(db);

			if (MinDate < minDate)
			{
				minDate = MinDate;
			}

			await App.Current!.Dispatcher.BeginInvoke(() =>
			{
				MinDate = minDate;
			});
		});
	}

	private static Func<ElectronicObserverContext, DateTime, DateTime, IAsyncEnumerable<ApiFile>> ApiFilesQuery { get; }
		= EF.CompileAsyncQuery((ElectronicObserverContext db, DateTime begin, DateTime end)
			=> db.ApiFiles
				.AsNoTracking()
				.Where(f => f.ApiFileType == ApiFileType.Response)
				.Where(f => f.TimeStamp > begin)
				.Where(f => f.TimeStamp < end)
				.Where(f =>
					f.Name == "api_req_sortie/battleresult" ||
					f.Name == "api_req_combined_battle/battleresult" ||
					f.Name == "api_req_practice/battle_result" ||
					f.Name == "api_req_mission/result" ||
					f.Name == "api_req_map/next" ||
					f.Name == "api_req_quest/clearitemget"));


	[RelayCommand(IncludeCancelCommand = true)]
	private async Task Search(CancellationToken ct)
	{
		SearchStartTime = DateTime.UtcNow;
		StatusBarText = EncycloRes.SearchingNow;

		try
		{
			List<ApiFile> apiFiles = await Task.Run(async () =>
			{
				DateTime begin = DateTimeBegin.ToUniversalTime();
				DateTime end = DateTimeEnd.ToUniversalTime();

				return await ApiFilesQuery(Db, begin, end).ToListAsync(ct);
			}, ct);

			if (!apiFiles.Any()) return;

			DateTime start = DateTimeBegin.ToUniversalTime();
			DateTime end = DateTimeEnd.ToUniversalTime();

			if (end > DateTime.UtcNow)
			{
				end = DateTime.UtcNow;
			}

			List<SenkaRecord> senkaRecords = GenerateSenkaRecords(start, end);

			foreach (SenkaRecord senkaRecord in senkaRecords)
			{
				int? startHqExp = ResourceRecord.GetRecord(senkaRecord.Start.ToLocalTime())?.HQExp;
				int? endHqExp = ResourceRecord.GetRecord(senkaRecord.End.ToLocalTime())?.HQExp;

				startHqExp ??= ResourceRecord.Record.LastOrDefault()?.HQExp;
				endHqExp ??= KCDatabase.Instance.Admiral switch
				{
					{ IsAvailable: true } a => a.Exp,
					_ => ResourceRecord.Record.LastOrDefault()?.HQExp,
				};


				senkaRecord.EstimatedHqExpSenkaGains = (startHqExp, endHqExp) switch
				{
					(int s, int e) => (e - s) * 7 / 10000.0,
					_ => 0,
				};

				// todo: some values are still off compared to EstimatedHqExpSenkaGains even with full data
				// the conditions probably aren't complete here
				/*
				senkaRecord.HqExpSenkaGains = apiFiles
					.GroupBy(f => f.SortieRecordId)
					.Where(g => g.Key is null || (g.Max(f => f.TimeStamp > senkaRecord.Start) && g.Max(f => f.TimeStamp < senkaRecord.End)))
					.SelectMany(g => g.Select(f => f))
					.Where(f => f.SortieRecordId is not null || f.TimeStamp > senkaRecord.Start)
					.Where(f => f.SortieRecordId is not null || f.TimeStamp < senkaRecord.End)
					.Sum(GetHqExp) * 7 / 10000;
				*/
				senkaRecord.ExtraOperationSenkaGains = apiFiles
					.Where(f => f.TimeStamp > senkaRecord.Start)
					.Where(f => f.TimeStamp < senkaRecord.End)
					.Sum(GetExtraOperationSenka);

				senkaRecord.QuestSenkaGains = apiFiles
					.Where(f => f.TimeStamp > senkaRecord.Start)
					.Where(f => f.TimeStamp < senkaRecord.End)
					.Sum(GetQuestSenka);
			}

			DataGridViewModel.ItemsSource.Clear();
			DataGridViewModel.AddRange(senkaRecords);

			int searchTime = (int)(DateTime.UtcNow - SearchStartTime).TotalMilliseconds;

			StatusBarText = $"{EncycloRes.SearchComplete} ({searchTime} ms)";
		}
		catch (OperationCanceledException)
		{
			StatusBarText = EncycloRes.SearchCancelled;
		}
		catch (Exception e)
		{
			Logger.Add(2, $"Unknown error while loading data: {e.Message}{e.StackTrace}");
		}
	}

	public static DateTime GetSessionStart(DateTime time)
	{
		DateTime endOfMonth = GetEndOfMonth(time);
		DateTime previousDay = time.AddDays(-1);

		return time.Hour switch
		{
			_ when time >= endOfMonth => endOfMonth,
			< 5 => new DateTime(previousDay.Year, previousDay.Month, previousDay.Day, 17, 0, 0, DateTimeKind.Utc),
			< 17 => new(time.Year, time.Month, time.Day, 5, 0, 0, DateTimeKind.Utc),
			_ => new(time.Year, time.Month, time.Day, 17, 0, 0, DateTimeKind.Utc),
		};
	}

	public static DateTime GetSessionEnd(DateTime time)
	{
		DateTime endOfMonth = GetEndOfMonth(time);
		DateTime nextDay = time.AddDays(1);

		DateTime sessionEnd = time.Hour switch
		{
			>= 17 => new(nextDay.Year, nextDay.Month, nextDay.Day, 5, 0, 0, DateTimeKind.Utc),
			>= 5 => new(time.Year, time.Month, time.Day, 17, 0, 0, DateTimeKind.Utc),
			_ => new(time.Year, time.Month, time.Day, 5, 0, 0, DateTimeKind.Utc),
		};

		return (sessionEnd > endOfMonth && time < endOfMonth) switch
		{
			true => endOfMonth,
			_ => sessionEnd,
		};
	}

	public static DateTime GetDayStart(DateTime time)
	{
		DateTime endOfMonth = GetEndOfMonth(time);
		DateTime previousDay = time.AddDays(-1);

		return time.Hour switch
		{
			_ when time >= endOfMonth => endOfMonth,
			< 17 => new DateTime(previousDay.Year, previousDay.Month, previousDay.Day, 17, 0, 0, DateTimeKind.Utc),
			_ => new(time.Year, time.Month, time.Day, 17, 0, 0, DateTimeKind.Utc),
		};
	}

	public static DateTime GetDayEnd(DateTime time)
	{
		DateTime endOfMonth = GetEndOfMonth(time);
		DateTime nextDay = time.AddDays(1);

		DateTime sessionEnd = time.Hour switch
		{
			_ when time >= endOfMonth => new(nextDay.Year, nextDay.Month, nextDay.Day, 17, 0, 0, DateTimeKind.Utc),
			>= 17 => new(nextDay.Year, nextDay.Month, nextDay.Day, 17, 0, 0, DateTimeKind.Utc),
			_ => new(time.Year, time.Month, time.Day, 17, 0, 0, DateTimeKind.Utc),
		};

		return (sessionEnd > endOfMonth && time < endOfMonth) switch
		{
			true => endOfMonth,
			_ => sessionEnd,
		};
	}

	public static DateTime GetMonthStart(DateTime time)
	{
		DateTime endOfMonth = GetEndOfMonth(time);
		DateTime previousMonth = new DateTime(time.Year, time.Month, 1, 0, 0, 0, DateTimeKind.Utc).AddDays(-1);

		return (time >= endOfMonth) switch
		{
			true => endOfMonth,
			_ => GetEndOfMonth(previousMonth),
		};
	}

	public static DateTime GetMonthEnd(DateTime time)
	{
		DateTime endOfMonth = GetEndOfMonth(time);
		DateTime nextMonth = endOfMonth.AddDays(1);

		return (time >= endOfMonth) switch
		{
			true => GetEndOfMonth(nextMonth),
			_ => endOfMonth,
		};
	}

	private static DateTime GetEndOfMonth(DateTime time) =>
		new(time.Year, time.Month, DateTime.DaysInMonth(time.Year, time.Month), 13, 0, 0, DateTimeKind.Utc);

	/// <summary>
	/// 05:00 UTC = 14:00 JST <br />
	/// 13:00 UTC = 22:00 JST <br />
	/// 17:00 UTC = 02:00 JST <br />
	/// </summary>
	public static List<SenkaRecord> GenerateSenkaRecords(DateTime start, DateTime end)
	{
		start = GetSessionStart(start);
		end = GetSessionEnd(end);

		List<SenkaRecord> senkaRecords = new();

		do
		{
			DateTime currentEnd = GetSessionEnd(start);

			senkaRecords.Insert(0, new() { Start = start, End = currentEnd, });

			start = currentEnd;
		} while (start < end);

		return senkaRecords;
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
			?.ApiCount ?? 0,

		_ => 0,
	};

	[RelayCommand]
	private void SelectToday(Calendar? calendar)
	{
		if (calendar is null) return;

		calendar.SelectedDate = DateTime.Now.Date;
	}
}
