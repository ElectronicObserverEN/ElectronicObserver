using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Data;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Tasks;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels.Tasks;

public class BossKillTaskViewModel : ObservableObject, IQuestTaskViewModel
{
	public IEnumerable<MapInfoModel> AllMaps { get; }
	public IEnumerable<BattleRank> Ranks { get; }

	public BossKillTaskModel Model { get; set; }

	public string Display => $"{Model.Progress}/{Model.Count} {GetClearCondition()}";
	public string? Progress => (Model.Progress >= Model.Count) switch
	{
		true => null,
		_ => Display
	};
	public string ClearCondition => GetClearCondition();

	private string GetClearCondition()
	{
		StringBuilder sb = new();

		sb.Append($"{Model.Map.AreaId}-{Model.Map.InfoId}");
		sb.Append(QuestTracking.ClearConditionBoss);

		sb.Append(Model.Rank switch
		{
			BattleRank.Any => QuestTracking.ClearConditionClear,
			BattleRank.D or BattleRank.C => Constants.GetWinRank((int)Model.Rank) + QuestTracking.ClearConditionOrHigher,
			BattleRank.B => "",
			BattleRank.A or BattleRank.S or BattleRank.SS =>
				Constants.GetWinRank((int)Model.Rank) + QuestTracking.ClearConditionRankVictories,

			_ or BattleRank.E => QuestTracking.ClearConditionBattle,
		});
		sb.Append(Model.Count);

		return sb.ToString();
	}

	public BossKillTaskViewModel(BossKillTaskModel model)
	{
		Model = model;

		AllMaps = KCDatabase.Instance.MapInfo.Values
			.Select(m => new MapInfoModel(m.MapAreaID, m.MapInfoID));
		Ranks = Enum.GetValues<BattleRank>();

		Model.PropertyChanged += (sender, args) =>
		{
			OnPropertyChanged(nameof(Display));
		};

		PropertyChanged += (_, e) =>
		{
			if (e.PropertyName is not nameof(Display)) return;

			KCDatabase.Instance.Quest.OnQuestUpdated();
		};
	}

	public void Increment(BattleRank resultRank, int compassMapAreaId, int compassMapInfoId)
	{
		if (compassMapAreaId == Model.Map.AreaId &&
			compassMapInfoId == Model.Map.InfoId &&
			resultRank >= Model.Rank)
		{
			Model.Progress++;
		}
	}
}
