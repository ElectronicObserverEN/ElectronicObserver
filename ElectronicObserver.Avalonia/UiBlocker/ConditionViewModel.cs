using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks;

namespace ElectronicObserver.Avalonia.UiBlocker;

public sealed partial class ConditionViewModel : ObservableObject
{
	private IMapInfoProvider MapProvider { get; }
	private Action<ConditionViewModel> OnRemove { get; }

	// hack: needed to avoid deleting Model.CellIds on initialization
	private bool IsInitializing { get; }

	public ConditionModel Model { get; }

	public List<ConditionType> Types { get; } = [.. Enum.GetValues<ConditionType>()];
	public List<BattleRank> Ranks { get; } = [.. Enum.GetValues<BattleRank>()];

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(ShowRank))]
	[NotifyPropertyChangedFor(nameof(ShowLocation))]
	[NotifyPropertyChangedFor(nameof(ShowNodes))]
	[NotifyPropertyChangedFor(nameof(Rank))]
	[NotifyPropertyChangedFor(nameof(SelectedMapOption))]
	public partial ConditionType Type { get; set; }

	[ObservableProperty] public partial bool Invert { get; set; }
	[ObservableProperty] public partial MapInfoModel? SelectedMapOption { get; set; }
	[ObservableProperty] public partial BattleRank? Rank { get; set; }

	public List<MapInfoModel?> MapOptions => field ??= [.. MapProvider.GetMaps()];

	private ObservableCollection<NodeOptionViewModel>? _nodes;

	public ObservableCollection<NodeOptionViewModel> Nodes
	{
		get
		{
			if (_nodes is null)
			{
				_nodes = [];
				RebuildNodes();
			}

			return _nodes;
		}
	}

	public bool ShowRank => Type is ConditionType.BattleRankAtMost or ConditionType.BattleRankAtLeast;
	public bool ShowLocation => Type == ConditionType.Location;
	public bool ShowNodes => ShowLocation && Model.Map is not null;

	public ConditionViewModel(ConditionModel model, IMapInfoProvider mapProvider, Action<ConditionViewModel> onRemove)
	{
		IsInitializing = true;

		Model = model;
		MapProvider = mapProvider;
		OnRemove = onRemove;

		Type = model.Type;
		Invert = model.Invert;
		Rank = model.Rank;
		SelectedMapOption = model.Map;

		IsInitializing = false;
	}

	partial void OnTypeChanged(ConditionType value)
	{
		Model.Type = value;

		if (IsInitializing) return;

		if (!ShowRank)
		{
			Model.Rank = null;
		}

		if (!ShowLocation)
		{
			Model.Map = null;
			Model.CellIds.Clear();
			SelectedMapOption = null;
		}
	}


	partial void OnInvertChanged(bool value)
	{
		Model.Invert = value;
	}

	partial void OnRankChanged(BattleRank? value)
	{
		Model.Rank = value;
	}

	partial void OnSelectedMapOptionChanged(MapInfoModel? value)
	{
		Model.Map = value;

		if (!IsInitializing)
		{
			Model.CellIds.Clear();
			RebuildNodes();
		}

		OnPropertyChanged();
		OnPropertyChanged(nameof(ShowNodes));
	}

	[RelayCommand]
	private void Remove() => OnRemove(this);

	private void RebuildNodes()
	{
		if (_nodes is null) return;

		_nodes.Clear();

		if (Model.Map is null) return;

		foreach (Node node in MapProvider.GetNodes(Model.Map))
		{
			NodeOptionViewModel nodeViewModel = new(node.Letter, node.Ids, Model);
			nodeViewModel.PropertyChanged += (_, _) => OnPropertyChanged(nameof(Nodes));
			_nodes.Add(nodeViewModel);
		}
	}
}
