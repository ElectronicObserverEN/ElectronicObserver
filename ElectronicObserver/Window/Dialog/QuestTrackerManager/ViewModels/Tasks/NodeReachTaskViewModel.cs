using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Data;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Tasks;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels.Tasks;

public partial class NodeReachTaskViewModel : ObservableObject, IQuestTaskViewModel
{
	public IEnumerable<MapInfoModel> AllMaps { get; }

	public NodeReachTaskModel Model { get; set; }
	public ObservableCollection<NodeIdViewModel> NodeIds { get; } = new();

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
		
		sb.Append(QuestTracking.Reach);
		sb.Append(Model.Count);

		return sb.ToString();
	}

	public NodeReachTaskViewModel(NodeReachTaskModel model)
	{
		Model = model;
		foreach (int id in Model.NodeIds)
		{
			AddNodeId(id);
		}

		AllMaps = KCDatabase.Instance.MapInfo.Values
			.Select(m => new MapInfoModel(m.MapAreaID, m.MapInfoID));

		Model.PropertyChanged += UpdateDisplay;

		PropertyChanged += (_, e) =>
		{
			if (e.PropertyName is not nameof(Display)) return;

			KCDatabase.Instance.Quest.OnQuestUpdated();
		};

		NodeIds.CollectionChanged += UpdateNodeIds;
	}

	private void UpdateDisplay(object? sender, object e)
	{
		OnPropertyChanged(nameof(Display));
	}

	private void UpdateNodeIds(object? sender, object e)
	{
		Model.NodeIds = NodeIds.Select(n => n.Id);
		UpdateDisplay(sender, e);
	}

	[RelayCommand]
	private void AddNodeId()
	{
		AddNodeId(1);
	}

	private void AddNodeId(int i)
	{
		NodeIdViewModel node = new() { Id = i };
		node.PropertyChanged += UpdateNodeIds;

		NodeIds.Add(node);
	}

	[RelayCommand]
	private void RemoveNodeId(NodeIdViewModel nodeId)
	{
		NodeIds.Remove(nodeId);
	}

	public void Increment(int compassMapAreaId, int compassMapInfoId, int nodeId)
	{
		if (compassMapAreaId == Model.Map.AreaId &&
			compassMapInfoId == Model.Map.InfoId &&
			Model.NodeIds.Contains(nodeId)
			)
		{
			Model.Progress++;
		}
	}
}
