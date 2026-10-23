using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.Reference;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipPosition;

public sealed partial class PositionConditionViewModel : ConditionViewModel
{
	private ConditionFactory ConditionFactory { get; }
	private ConditionDisplayerService ConditionDisplayerService { get; }
	public override PositionCondition Model { get; }
	private ObservableCollection<ReferenceDefinitionViewModel> References { get; }

	public List<ConditionType> ConditionTypes { get; } = [.. Enum.GetValues<ConditionType>()];

	[ObservableProperty] public partial ConditionViewModel? Condition { get; set; }
	[ObservableProperty] public partial ConditionType ConditionType { get; set; }

	public ObservableCollection<PositionViewModel> Positions { get; } = [];

	public override string Display => ConditionDisplayerService.ToDisplay(this);

	public PositionConditionViewModel(
		ConditionFactory conditionFactory,
		ConditionDisplayerService conditionDisplayerService,
		PositionCondition model,
		ObservableCollection<ReferenceDefinitionViewModel> references)
	{
		ConditionFactory = conditionFactory;
		ConditionDisplayerService = conditionDisplayerService;
		Model = model;
		References = references;

		Condition = ConditionFactory.CreateViewModel(model.Condition, References);

		foreach (int position in Enumerable.Range(1, 6))
		{
			PositionViewModel vm = new()
			{
				IsChecked = model.Positions.Contains(position),
				Position = position,
			};

			vm.PropertyChanged += (_, _) =>
			{
				Model.Positions = [..Positions.Where(p => p.IsChecked).Select(p => p.Position)];

				RaiseConditionChanged();
			};

			Positions.Add(vm);
		}

		ConditionChanged += RaiseDisplayChanged;
	}

	partial void OnConditionChanged(ConditionViewModel? value)
	{
		Model.Condition = value?.Model;
		
		RaiseConditionChanged();
	}

	[RelayCommand]
	private void CreateCondition()
	{
		FleetCondition? model = ConditionFactory.CreateModel(ConditionType);

		if (model is null) return;
		
		ConditionViewModel vm = ConditionFactory.CreateViewModel(model, References);
		vm.ConditionChanged += RaiseConditionChanged;
		Condition = vm;
	}

	[RelayCommand]
	private void RemoveCondition()
	{
		Condition = null;
	}
}
