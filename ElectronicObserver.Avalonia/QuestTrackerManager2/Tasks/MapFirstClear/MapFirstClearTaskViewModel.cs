using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Core.Types.Data;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.MapFirstClear;

public partial class MapFirstClearTaskViewModel(
	IKCDatabase kcDatabase,
	TaskDisplayerService taskDisplayerService,
	MapFirstClearTaskModel model) : TaskViewModel
{
	private IKCDatabase KcDatabase { get; } = kcDatabase;
	private TaskDisplayerService TaskDisplayerService { get; } = taskDisplayerService;
	public override MapFirstClearTaskModel Model { get; } = model;

	// Deferred on purpose - MapInfo is only populated once the game data has actually loaded,
	// which can happen well after this view model is constructed.
	public IEnumerable<MapInfoModel> Maps => KcDatabase.MapInfo.Values
		.OrderBy(m => m.MapAreaID)
		.ThenBy(m => m.MapInfoID)
		.Select(m => new MapInfoModel { AreaId = m.MapAreaID, InfoId = m.MapInfoID });

	[ObservableProperty] public partial MapInfoModel Map { get; set; } = model.Map;
	[ObservableProperty] public override partial int Count { get; set; } = model.Count;
	[ObservableProperty] public override partial int Progress { get; set; }

	public override string Display => TaskDisplayerService.ToDisplay(this);

	partial void OnMapChanged(MapInfoModel value)
	{
		Model.Map = value;
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

	public void Increment(int compassMapAreaId, int compassMapInfoId)
	{
		if (compassMapAreaId != Map.AreaId) return;
		if (compassMapInfoId != Map.InfoId) return;

		Progress++;
	}
}
