using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Core.Types.Data;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.BossKill;

public partial class BossKillTaskViewModel(
	IKCDatabase kcDatabase,
	TaskDisplayerService taskDisplayerService,
	BossKillTaskModel model) : TaskViewModel
{
	private IKCDatabase KcDatabase { get; } = kcDatabase;
	private TaskDisplayerService TaskDisplayerService { get; } = taskDisplayerService;
	public override BossKillTaskModel Model { get; } = model;

	// Deferred on purpose - MapInfo is only populated once the game data has actually loaded,
	// which can happen well after this view model is constructed.
	public IEnumerable<MapInfoModel> Maps => KcDatabase.MapInfo.Values
		.OrderBy(m => m.MapAreaID)
		.ThenBy(m => m.MapInfoID)
		.Select(m => new MapInfoModel { AreaId = m.MapAreaID, InfoId = m.MapInfoID });

	public List<BattleRank> Ranks { get; set; } = [.. Enum.GetValues<BattleRank>()];

	[ObservableProperty] public partial MapInfoModel Map { get; set; } = model.Map;
	[ObservableProperty] public partial BattleRank Rank { get; set; } = model.Rank;
	[ObservableProperty] public override partial int Count { get; set; } = model.Count;
	[ObservableProperty] public override partial int Progress { get; set; }

	public override string Display => TaskDisplayerService.ToDisplay(this);

	partial void OnMapChanged(MapInfoModel value)
	{
		Model.Map = value;
		RaiseDisplayChanged();
	}

	partial void OnRankChanged(BattleRank value)
	{
		Model.Rank = value;
		RaiseDisplayChanged();
	}

	partial void OnCountChanged(int value)
	{
		Count = Math.Clamp(value, 1, int.MaxValue);
		Model.Count = Count;
		OnProgressChanged(Progress);
	}

	partial void OnProgressChanged(int value)
	{
		Progress = Math.Clamp(value, 0, Count);
		RaiseDisplayChanged();
	}

	public void Increment(BattleRank resultRank, int compassMapAreaId, int compassMapInfoId)
	{
		if (compassMapAreaId != Map.AreaId) return;
		if (compassMapInfoId != Map.InfoId) return;
		if (resultRank < Rank) return;

		Progress++;
	}
}
