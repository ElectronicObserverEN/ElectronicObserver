using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Core.Types.Data;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.NodeReach;

public partial class NodeReachTaskViewModel : TaskViewModel
{
	private IKCDatabase KcDatabase { get; }
	private TaskDisplayerService TaskDisplayerService { get; }
	public override NodeReachTaskModel Model { get; }

	// Deferred on purpose - MapInfo is only populated once the game data has actually loaded,
	// which can happen well after this view model is constructed.
	public IEnumerable<MapInfoModel> Maps => KcDatabase.MapInfo.Values
		.OrderBy(m => m.MapAreaID)
		.ThenBy(m => m.MapInfoID)
		.Select(m => new MapInfoModel { AreaId = m.MapAreaID, InfoId = m.MapInfoID });

	[ObservableProperty] public partial MapInfoModel Map { get; set; }
	[ObservableProperty] public override partial int Count { get; set; }
	[ObservableProperty] public override partial int Progress { get; set; }
	[ObservableProperty] public partial string? Name { get; set; }

	public ObservableCollection<NodeIdViewModel> NodeIds { get; }

	public override string Display => TaskDisplayerService.ToDisplay(this);

	public NodeReachTaskViewModel(IKCDatabase kcDatabase, TaskDisplayerService taskDisplayerService, NodeReachTaskModel model)
	{
		KcDatabase = kcDatabase;
		TaskDisplayerService = taskDisplayerService;
		Model = model;

		Map = model.Map;
		Count = model.Count;
		Name = model.Name;
		NodeIds = [];

		foreach (int id in model.NodeIds)
		{
			AddNodeId(id);
		}
	}

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

	partial void OnNameChanged(string? value)
	{
		Model.Name = value;
		RaiseDisplayChanged();
	}

	private void UpdateNodeIds()
	{
		Model.NodeIds = [.. NodeIds.Select(n => n.Id)];
		RaiseDisplayChanged();
	}

	private void AddNodeId(int id)
	{
		NodeIdViewModel node = new(id);
		node.PropertyChanged += (_, _) => UpdateNodeIds();

		NodeIds.Add(node);
		UpdateNodeIds();
	}

	[RelayCommand]
	private void AddNodeId()
	{
		AddNodeId(1);
	}

	[RelayCommand]
	private void RemoveNodeId(NodeIdViewModel nodeId)
	{
		NodeIds.Remove(nodeId);
		UpdateNodeIds();
	}

	public void Increment(int compassMapAreaId, int compassMapInfoId, int nodeId)
	{
		if (compassMapAreaId != Map.AreaId) return;
		if (compassMapInfoId != Map.InfoId) return;
		if (!NodeIds.Any(n => n.Id == nodeId)) return;

		Progress++;
	}
}
