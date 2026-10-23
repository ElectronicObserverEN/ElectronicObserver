using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.Reference;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.Group;

public sealed partial class GroupConditionViewModel : ConditionViewModel
{
	private ConditionFactory ConditionFactory { get; }
	private ConditionDisplayerService ConditionDisplayerService { get; }
	public override GroupCondition Model { get; }
	private ObservableCollection<ReferenceDefinitionViewModel> References { get; }

	public List<ConditionType> ConditionTypes { get; } = [.. Enum.GetValues<ConditionType>()];
	public List<LogicalOperator> Operators { get; } = [..Enum.GetValues<LogicalOperator>()];

	[ObservableProperty] public partial LogicalOperator Operator { get; set; }
	[ObservableProperty] public partial ConditionType ConditionType { get; set; } = ConditionType.ShipId;

	public ObservableCollection<ConditionViewModel> Conditions { get; } = [];

	public override string Display => ConditionDisplayerService.ToDisplay(this);

	public GroupConditionViewModel(
		ConditionFactory conditionFactory,
		ConditionDisplayerService conditionDisplayerService,
		GroupCondition model,
		ObservableCollection<ReferenceDefinitionViewModel> references)
	{
		ConditionFactory = conditionFactory;
		ConditionDisplayerService = conditionDisplayerService;
		Model = model;
		References = references;

		Operator = Model.Operator;

		foreach (FleetCondition child in model.Conditions)
		{
			AddChildViewModel(child);
		}

		ConditionChanged += RaiseDisplayChanged;

		Conditions.CollectionChanged += (_, _) =>
		{
			RaiseConditionChanged();
		};
	}

	partial void OnOperatorChanged(LogicalOperator value)
	{
		Model.Operator = value;

		RaiseConditionChanged();
	}

	private void AddChildViewModel(FleetCondition model)
	{
		ConditionViewModel vm = ConditionFactory.CreateViewModel(model, References);
		vm.ConditionChanged += RaiseConditionChanged;
		Conditions.Add(vm);
	}

	[RelayCommand]
	private void AddCondition()
	{
		FleetCondition? conditionModel = ConditionFactory.CreateModel(ConditionType);

		if (conditionModel is null) return;

		Model.Conditions.Add(conditionModel);
		AddChildViewModel(conditionModel);
	}

	[RelayCommand]
	private void RemoveCondition(ConditionViewModel vm)
	{
		Conditions.Remove(vm);
		Model.Conditions.Remove(vm.Model);

		RaiseConditionChanged();
	}
}
