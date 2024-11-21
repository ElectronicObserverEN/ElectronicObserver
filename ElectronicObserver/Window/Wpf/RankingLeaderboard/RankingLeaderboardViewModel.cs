using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Common;
using ElectronicObserver.KancolleApi.Types.ApiReqRanking.Models;
using ElectronicObserver.KancolleApi.Types.ApiReqRanking.Mxltvkpyuklh;
using ElectronicObserver.Observer;
using ElectronicObserver.Services;

namespace ElectronicObserver.Window.Wpf.RankingLeaderboard;

public partial class RankingLeaderboardViewModel : UserControlViewModelBase
{
	private int[] PossibleRank { get; } = [8931, 1201, 1156, 5061, 4569, 4732, 3779, 4568, 5695, 4619, 4912, 5669, 6586];

	private List<int> PossibleUserKey { get; } = [];

	[ObservableProperty]
	private List<RankingEntryModel> _rankingData;

	public RankingLeaderboardViewModel()
	{
		APIObserver.Instance.ApiReqRanking_Mxltvkpyuklh.ResponseReceived += HandleData;

		TimeChangeService timeChangeService = Ioc.Default.GetRequiredService<TimeChangeService>();
		timeChangeService.RankingLeaderboardUpdate += OnRankingCutoffChanged;

		RankingData = NewLeaderboard();
	}

	private void OnRankingCutoffChanged()
	{
		RankingData = NewLeaderboard();
	}

	private List<RankingEntryModel> NewLeaderboard()
	{
		return Enumerable.Range(0, 500)
			.Select(position => new RankingEntryModel()
			{
				AdmiralName = "???",
				Comment = "???",
				MedalCount = 0,
				Points = 0,
				Position = position + 1,
			})
			.ToList();
	}

	private void HandleData(string apiname, dynamic data)
	{
		try
		{
			ApiReqRankingMxltvkpyuklhResponse parsedData = JsonSerializer.Deserialize<ApiReqRankingMxltvkpyuklhResponse>(data.ToString());

			// Ignore if page is outside T500
			if (parsedData.ApiDispPage > 50) return;

			foreach (ApiList entry in parsedData.ApiList)
			{
				RankingEntryModel? parsedEntry = HandleEntry(entry);

				if (parsedEntry is { })
				{
					RankingData[parsedEntry.Position - 1] = parsedEntry;
				}
			}
		}
		catch (Exception ex)
		{
			// TODO
		}
	}

	private bool CheckRate(int key, int userKey, decimal rate)
	{
		decimal points = rate / key / userKey - 91;
		return points == Math.Floor(points) && points >= 0;
	}

	private RankingEntryModel? HandleEntry(ApiList entry)
	{
		int key = PossibleRank[entry.ApiMxltvkpyuklh % 13];

		if (PossibleUserKey.Count is 0)
		{
			for (int userKey = 10; userKey < 100; userKey++)
			{
				if (CheckRate(key, userKey, entry.ApiWuhnhojjxmke))
				{
					PossibleUserKey.Add(userKey);
				}
			}
		}
		else
		{
			List<int> toRemove = [];

			foreach (int userKey in PossibleUserKey)
			{
				if (!CheckRate(key, userKey, entry.ApiWuhnhojjxmke))
				{
					toRemove.Add(userKey);
				}
			}

			PossibleUserKey.RemoveAll(toRemove.Contains);
		}

		if (PossibleUserKey.Count is 0) return null;

		return new RankingEntryModel
		{
			AdmiralName = entry.ApiMtjmdcwtvhdr,
			Comment = entry.ApiItbrdpdbkynm,
			MedalCount = entry.ApiItslcqtmrxtf / (key + 1853) - 157,
			Points = (int)Math.Floor(entry.ApiWuhnhojjxmke / key / PossibleUserKey.Last()) - 91,
			Position = entry.ApiMxltvkpyuklh,
		};
	}
}
