using System;
using System.ComponentModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Core;
using ElectronicObserver.Data;
using ElectronicObserver.KancolleApi.Types.ApiReqRanking.Models;
using ElectronicObserver.KancolleApi.Types.ApiReqRanking.Mxltvkpyuklh;
using ElectronicObserver.Observer;
using ElectronicObserver.Services;
using ElectronicObserver.Utility;
using ElectronicObserver.Utility.Mathematics;
using Jot;

namespace ElectronicObserver.Window.Wpf.SenkaLeaderboard;

public partial class SenkaLeaderboardManager : ObservableObject
{
	public SenkaLeaderboardViewModel CurrentCutoffData { get; } = new();

	private TimeChangeService TimeChangeService { get; }
	private Tracker Tracker { get; }

	[ObservableProperty]
	private partial SenkaLeaderboardRefreshKind CurrentSenkaLeaderboardRefreshKind { get; set; }

	public SenkaLeaderboardManager(TimeChangeService timeChangeService, Tracker tracker)
	{
		APIObserver.Instance.ApiReqRanking_Mxltvkpyuklh.ResponseReceived += HandleData;

		TimeChangeService = timeChangeService;
		TimeChangeService.HourChanged += () => CurrentSenkaLeaderboardRefreshKind = GetSankaLeaderboardRefreshKind();

		Tracker = tracker;

		CurrentSenkaLeaderboardRefreshKind = GetSankaLeaderboardRefreshKind();

		PropertyChanged += OnSenkaLeaderboardRefreshKindChanged;

		CurrentCutoffData.Update();
		StartJotTracking();
	}

	private void OnSenkaLeaderboardRefreshKindChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName is not nameof(CurrentSenkaLeaderboardRefreshKind)) return;

		CurrentCutoffData.Reset();
	}

	private SenkaLeaderboardRefreshKind GetSankaLeaderboardRefreshKind() => DateTimeHelper.GetJapanStandardTimeNow() switch
	{
		{ Hour: >= 15 or < 3 } => SenkaLeaderboardRefreshKind.MidDay,
		_ => SenkaLeaderboardRefreshKind.NewDay,
	};

	private void HandleData(string apiname, dynamic data)
	{
		if (CurrentCutoffData.IsDataReadyToBeSubmitted()) return;

		HandleData(data);
	}

	private void HandleData(dynamic data)
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

			// Due to how we parse data to guess a key, we have to run the decoding twice for the first "batch" for data to be accurate
			if (CurrentCutoffData.LoadedEntriesCount is 10)
			{
				foreach (ApiList entry in parsedData.ApiList)
				{
					CurrentCutoffData.HandleEntry(entry);
				}
			}
		}
		catch (Exception ex)
		{
			Logger.Add(2, BonodereSubmissionResources.BonodereError, ex);
		}
		finally
		{
			CurrentCutoffData.Update();
		}
	}

	private void StartJotTracking()
	{
		Tracker.Track(this);
	}
}
