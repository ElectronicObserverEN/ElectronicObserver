using System;
using System.ComponentModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Data;
using ElectronicObserver.KancolleApi.Types.ApiReqRanking.Models;
using ElectronicObserver.KancolleApi.Types.ApiReqRanking.Mxltvkpyuklh;
using ElectronicObserver.Observer;
using ElectronicObserver.Services;
using ElectronicObserver.Utility;
using ElectronicObserver.Utility.Mathematics;

namespace ElectronicObserver.Window.Wpf.SenkaLeaderboard;

public partial class SenkaLeaderboardManager : ObservableObject
{
	public SenkaLeaderboardViewModel CurrentCutoffData { get; } = new();

	private TimeChangeService TimeChangeService { get; }

	[ObservableProperty]
	private partial SenkaCutoffKind CurrentSenkaCutoffKind { get; set; }

	public SenkaLeaderboardManager(TimeChangeService timeChangeService)
	{
		APIObserver.Instance.ApiReqRanking_Mxltvkpyuklh.ResponseReceived += HandleData;

		TimeChangeService = timeChangeService;
		TimeChangeService.HourChanged += () => CurrentSenkaCutoffKind = GetSankaLeaderboardCutoffKind();

		CurrentSenkaCutoffKind = GetSankaLeaderboardCutoffKind();

		PropertyChanged += OnSenkaLeaderboardCutoffChanged;

		CurrentCutoffData.Update();
	}

	private void OnSenkaLeaderboardCutoffChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName is not nameof(CurrentSenkaCutoffKind)) return;

		CurrentCutoffData.Reset();
	}

	private SenkaCutoffKind GetSankaLeaderboardCutoffKind()
	{
		DateTime time = DateTimeHelper.GetJapanStandardTimeNow();

		if ((time.Day == DateTime.DaysInMonth(time.Year, time.Month) && time.Hour > 22) || (time.Day is 1 && time.Hour < 3))
		{
			return SenkaCutoffKind.NewMonth;
		}

		return time.TimeOfDay switch
		{
			{ Hours: >= 15 or < 3 } => SenkaCutoffKind.MidDay,
			_ => SenkaCutoffKind.NewDay,
		};
	}

	private void HandleData(string apiname, dynamic data)
	{
		try
		{
			ApiReqRankingMxltvkpyuklhResponse parsedData =
				JsonSerializer.Deserialize<ApiReqRankingMxltvkpyuklhResponse>(data.ToString());

			// Ignore if page is outside T500
			if (parsedData.ApiDispPage > 50) return;

			foreach (ApiList entry in parsedData.ApiList)
			{
				CurrentCutoffData.HandleEntry(entry);
			}
		}
		catch (Exception ex)
		{
			Logger.Add(2, "Bonodere error", ex);
		}
		finally
		{
			CurrentCutoffData.Update();
		}
	}
}
