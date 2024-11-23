using System;
using System.ComponentModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Data;
using ElectronicObserver.KancolleApi.Types.ApiReqRanking.Models;
using ElectronicObserver.KancolleApi.Types.ApiReqRanking.Mxltvkpyuklh;
using ElectronicObserver.Observer;
using ElectronicObserver.Services;

namespace ElectronicObserver.Window.Wpf.SenkaLeaderboard;

public partial class SenkaLeaderboardManager : ObservableObject
{
	public SenkaLeaderboardViewModel CurrentCutoffData { get; } = new();

	private TimeChangeService TimeChangeService { get; }

	[ObservableProperty]
	private RankingCutoffKind _currentRankingCutoffKind;

	public SenkaLeaderboardManager(TimeChangeService timeChangeService)
	{
		APIObserver.Instance.ApiReqRanking_Mxltvkpyuklh.ResponseReceived += HandleData;

		TimeChangeService = timeChangeService;
		TimeChangeService.HourChanged += () => CurrentRankingCutoffKind = GetRankingCutoffKind();

		CurrentRankingCutoffKind = GetRankingCutoffKind();

		PropertyChanged += OnRankingCutoffChanged;

		CurrentCutoffData.Update();
	}

	private void OnRankingCutoffChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName is not nameof(CurrentRankingCutoffKind)) return;

		CurrentCutoffData.Reset();
	}

	private RankingCutoffKind GetRankingCutoffKind()
	{
		DateTime time = DateTime.Now;

		if ((time.Day == DateTime.DaysInMonth(time.Year, time.Month) && time.Hour > 22) || (time.Day is 1 && time.Hour < 3))
		{
			return RankingCutoffKind.NewMonth;
		}

		return time.TimeOfDay switch
		{
			{ Hours: >= 15 or < 3 } => RankingCutoffKind.MidDay,
			_ => RankingCutoffKind.NewDay,
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
			// TODO
		}
		finally
		{
			CurrentCutoffData.Update();
		}
	}

	
}
