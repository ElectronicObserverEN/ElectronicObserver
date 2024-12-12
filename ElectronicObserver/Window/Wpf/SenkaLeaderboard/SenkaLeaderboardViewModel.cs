﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common.Datagrid;
using ElectronicObserver.Data.Bonodere;
using ElectronicObserver.KancolleApi.Types.ApiReqRanking.Models;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility;
using ElectronicObserver.ViewModels;
using ElectronicObserver.Window.Control.Paging;

namespace ElectronicObserver.Window.Wpf.SenkaLeaderboard;

public partial class SenkaLeaderboardViewModel : AnchorableViewModel
{
	private int[] PossibleRank { get; } = [8931, 1201, 1156, 5061, 4569, 4732, 3779, 4568, 5695, 4619, 4912, 5669, 6586];

	private List<int> PossibleUserKey { get; } = [];

	[ObservableProperty]
	private partial List<SenkaEntryModel> SenkaData { get; set; }

	private Dictionary<int, ApiList> SenkaRawData { get; set; } = [];

	public PagingControlViewModel PagingViewModel { get; }

	public DataGridViewModel<SenkaEntryModel> DataGridViewModel { get; }

	[ObservableProperty]
	[NotifyCanExecuteChangedFor(nameof(SubmitDataCommand))]
	public partial int LoadedEntriesCount { get; set; }

	public BonodereSubmissionService BonodereSubmissionService { get; }

	public SenkaLeaderboardTranslationViewModel Translation { get; }

	public bool IsBonodereReady => !string.IsNullOrEmpty(Configuration.Config.DataSubmission.BonodereToken) && Configuration.Config.DataSubmission.BonodereIntegrationEnabled;

	public SenkaLeaderboardViewModel() : base(SenkaLeaderboardResources.Title, "SenkaLeaderboard", IconContent.FormResourceChart)
	{
		Translation = Ioc.Default.GetRequiredService<SenkaLeaderboardTranslationViewModel>();

		Title = Translation.Title;
		Translation.PropertyChanged += (_, _) => Title = Translation.Title;

		SenkaData = NewLeaderboard();

		BonodereSubmissionService = Ioc.Default.GetRequiredService<BonodereSubmissionService>();

		PagingViewModel = new();
		DataGridViewModel = new([]);
		Update();

		Configuration.Instance.ConfigurationChanged += () => OnPropertyChanged(nameof(IsBonodereReady));
	}

	public void Update()
	{
		PagingViewModel.Items = SenkaData.Cast<object>().ToList();
	}

	public void Reset()
	{
		SenkaData = NewLeaderboard();
		SenkaRawData = [];
		Update();
		UpdateEntryCount();
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
				IsKnown = false,
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
		if (ConvertApiToSenkaModel(entry) is not SenkaEntryModel parsedEntry) return;

		if (SenkaData.Count < entry.ApiMxltvkpyuklh) return;

		SenkaData[entry.ApiMxltvkpyuklh - 1] = parsedEntry;

		SenkaRawData.Add(entry.ApiMxltvkpyuklh, entry);

		UpdateEntryCount();

		if (LoadedEntriesCount % 10 is 0)
		{
			RunAnotherRoundOfDecoding();
		}

		PagingViewModel.DisplayPageFromElementKey(entry.ApiMxltvkpyuklh - 1);
	}

	private SenkaEntryModel? ConvertApiToSenkaModel(ApiList entry)
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

		return new SenkaEntryModel
		{
			AdmiralName = entry.ApiMtjmdcwtvhdr,
			Comment = entry.ApiItbrdpdbkynm,
			MedalCount = entry.ApiItslcqtmrxtf / (key + 1853) - 157,
			Points = (int)Math.Floor(entry.ApiWuhnhojjxmke / key / PossibleUserKey.Last()) - 91,
			Position = entry.ApiMxltvkpyuklh,
			IsKnown = true,
		};
	}

	private void RunAnotherRoundOfDecoding()
	{
		foreach (SenkaEntryModel entry in SenkaData.Where(entry => entry.IsKnown).OrderByDescending(entry => entry.Position))
		{
			if (SenkaRawData.TryGetValue(entry.Position, out ApiList? rawData) && ConvertApiToSenkaModel(rawData) is SenkaEntryModel parsedEntry)
			{
				entry.MedalCount = parsedEntry.MedalCount;
				entry.Points = parsedEntry.Points;
			}
		}
	}

	private void UpdateEntryCount()
	{
		LoadedEntriesCount = SenkaData.Count(entry => entry.IsKnown);
	}

	public bool IsDataReadyToBeSubmitted() => LoadedEntriesCount is 500;

	[RelayCommand(CanExecute = nameof(IsDataReadyToBeSubmitted))]
	private async Task SubmitData()
	{
		await BonodereSubmissionService.SubmitData(SenkaData);
	}
}
