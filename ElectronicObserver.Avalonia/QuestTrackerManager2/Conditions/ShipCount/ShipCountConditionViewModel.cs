using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.Reference;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipCount;

public sealed partial class CountConditionViewModel : ConditionViewModel
{
	private ConditionFactory ConditionFactory { get; }
	private ConditionDisplayerService ConditionDisplayerService { get; }
	public override ShipCountCondition Model { get; }
	private ObservableCollection<ReferenceDefinitionViewModel> References { get; }

	public List<ComparisonOperator> ComparisonOperators { get; } = [..Enum.GetValues<ComparisonOperator>()];
	public List<ConditionType> ConditionTypes { get; } = [.. Enum.GetValues<ConditionType>()];

	[ObservableProperty] public partial int Count { get; set; }
	[ObservableProperty] public partial ComparisonOperator ComparisonOperator { get; set; }
	[ObservableProperty] public partial ConditionViewModel? Condition { get; set; }
	[ObservableProperty] public partial ConditionType ConditionType { get; set; }

	public override string Display => ConditionDisplayerService.ToDisplay(this);
	
	public CountConditionViewModel(
		ConditionFactory conditionFactory,
		ConditionDisplayerService conditionDisplayerService,
		ShipCountCondition model,
		ObservableCollection<ReferenceDefinitionViewModel> references)
	{
		ConditionFactory = conditionFactory;
		ConditionDisplayerService = conditionDisplayerService;
		Model = model;
		References = references;

		Count = Model.Count;
		ComparisonOperator = Model.Operator;
		Condition = ConditionFactory.CreateViewModel(model.Condition, References);

		ConditionChanged += RaiseDisplayChanged;
	}

	partial void OnCountChanged(int value)
	{
		Model.Count = value;
		
		RaiseConditionChanged();
	}

	partial void OnComparisonOperatorChanged(ComparisonOperator value)
	{
		Model.Operator = value;
		
		RaiseConditionChanged();
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
