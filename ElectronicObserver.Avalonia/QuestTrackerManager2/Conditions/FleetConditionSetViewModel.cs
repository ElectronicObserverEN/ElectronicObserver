using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.Reference;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions;

public sealed partial class FleetConditionSetViewModel : ConditionDisplayerViewModel
{
	private ConditionFactory ConditionFactory { get; }
	private ConditionDisplayerService ConditionDisplayerService { get; }
	public FleetConditionSet Model { get; }

	public List<ConditionType> ConditionTypes { get; } = [.. Enum.GetValues<ConditionType>()];

	public ObservableCollection<ReferenceDefinitionViewModel> References { get; } = [];
	[ObservableProperty] public partial ConditionViewModel? Condition { get; set; }
	[ObservableProperty] public partial ConditionType ConditionType { get; set; }

	public override string Display => References.Count switch
	{
		0 => ConditionDisplayerService.ToDisplay(Condition),

		_ => $"""
			{string.Join("\n", References.Select(r => $"{r.Name} = {ConditionDisplayerService.ToDisplay(r.Condition)}"))}

			{ConditionDisplayerService.ToDisplay(Condition)}
			""",
	};

	public FleetConditionSetViewModel(
		ConditionFactory conditionFactory,
		ConditionDisplayerService conditionDisplayerService,
		FleetConditionSet model)
	{
		ConditionFactory = conditionFactory;
		ConditionDisplayerService = conditionDisplayerService;
		Model = model;

		foreach (ReferenceDefinition definition in model.Definitions)
		{
			AddReferenceViewModel(definition);
		}

		Condition = ConditionFactory.CreateViewModel(model.Condition, References);

		ConditionChanged += RaiseDisplayChanged;
	}

	partial void OnConditionChanged(ConditionViewModel? value)
	{
		Model.Condition = value?.Model;

		RaiseConditionChanged();
	}

	private void AddReferenceViewModel(ReferenceDefinition model)
	{
		ReferenceDefinitionViewModel vm = ConditionFactory.CreateViewModel(model, References);
		vm.ConditionChanged += RaiseConditionChanged;

		References.Add(vm);

		RaiseConditionChanged();
	}

	[RelayCommand]
	private void AddReference()
	{
		ReferenceDefinition model = new();

		Model.Definitions.Add(model);
		AddReferenceViewModel(model);
	}

	[RelayCommand]
	private void RemoveReference(ReferenceDefinitionViewModel vm)
	{
		References.Remove(vm);
		Model.Definitions.Remove(vm.Model);

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
