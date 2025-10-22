using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Data;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Tasks;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels.Tasks;

public class NodeIdViewModel : ObservableObject
{
	public int Id { get; set; }
}

public partial class BattleNodeIdTaskViewModel : ObservableObject, IQuestTaskViewModel
{
	public IEnumerable<MapInfoModel> AllMaps { get; }
	public IEnumerable<BattleRank> Ranks { get; }

	public BattleNodeIdTaskModel Model { get; set; }
	public ObservableCollection<NodeIdViewModel> NodeIds { get; }

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
		sb.Append(Model.Name switch
		{
			null or "" => $"({string.Join("・", Model.NodeIds)})",
			_ => Model.Name
		});

		sb.Append(CultureInfo.CurrentCulture.Name switch
		{
			"ja-JP" => "",
			_ => " "
		});

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

	public BattleNodeIdTaskViewModel(BattleNodeIdTaskModel model)
	{
		Model = model;
		NodeIds = new(Model.NodeIds.Select(i => new NodeIdViewModel { Id = i }));

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

		NodeIds.CollectionChanged += (sender, args) =>
		{
			Model.NodeIds = NodeIds.Select(n => n.Id);
		};
	}

	[RelayCommand]
	private void AddNodeId()
	{
		NodeIds.Add(new());
	}

	[RelayCommand]
	private void RemoveNodeId(NodeIdViewModel nodeId)
	{
		NodeIds.Remove(nodeId);
	}

	public void Increment(BattleRank resultRank, int compassMapAreaId, int compassMapInfoId, int nodeId)
	{
		if (compassMapAreaId == Model.Map.AreaId &&
			compassMapInfoId == Model.Map.InfoId &&
			resultRank >= Model.Rank &&
			Model.NodeIds.Contains(nodeId)
			)
		{
			Model.Progress++;
		}
	}
}
