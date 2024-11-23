﻿using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.KancolleApi.Types.ApiReqRanking.Models;
using ElectronicObserver.ViewModels;
using ElectronicObserver.Window.Control.Paging;

namespace ElectronicObserver.Window.Wpf.SenkaLeaderboard;

public partial class SenkaLeaderboardViewModel : AnchorableViewModel
{
	private int[] PossibleRank { get; } = [8931, 1201, 1156, 5061, 4569, 4732, 3779, 4568, 5695, 4619, 4912, 5669, 6586];

	private List<int> PossibleUserKey { get; } = [];

	[ObservableProperty]
	private List<SenkaEntryModel> _rankingData;

	public PagingControlViewModel PagingViewModel { get; }

	public int LoadedEntriesCount => RankingData.Count(entry => entry.Points > 0);

	public SenkaLeaderboardViewModel() : base("Senka leaderboard", "SenkaLeaderboard", null)
	{
		RankingData = NewLeaderboard();

		PagingViewModel = new();
		Update();
	}

	public void Update()
	{
		PagingViewModel.Items = RankingData.Cast<object>().ToList();
	}

	public void Reset()
	{
		RankingData = NewLeaderboard();
		Update();
	}

	private List<SenkaEntryModel> NewLeaderboard()
	{
		return Enumerable.Range(0, 500)
			.Select(position => new SenkaEntryModel()
			{
				AdmiralName = "???",
				Comment = "???",
				MedalCount = 0,
				Points = 0,
				Position = position + 1,
			})
			.ToList();
	}
	private bool CheckRate(int key, int userKey, decimal rate)
	{
		decimal points = rate / key / userKey - 91;
		return points == Math.Floor(points) && points >= 0;
	}

	public void HandleEntry(ApiList entry)
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

		if (PossibleUserKey.Count is 0) return;
		if (RankingData.Count < entry.ApiMxltvkpyuklh) return;

		RankingData[entry.ApiMxltvkpyuklh - 1] = new SenkaEntryModel
		{
			AdmiralName = entry.ApiMtjmdcwtvhdr,
			Comment = entry.ApiItbrdpdbkynm,
			MedalCount = entry.ApiItslcqtmrxtf / (key + 1853) - 157,
			Points = (int)Math.Floor(entry.ApiWuhnhojjxmke / key / PossibleUserKey.Last()) - 91,
			Position = entry.ApiMxltvkpyuklh,
		};

		OnPropertyChanged(nameof(LoadedEntriesCount));
	}
}
