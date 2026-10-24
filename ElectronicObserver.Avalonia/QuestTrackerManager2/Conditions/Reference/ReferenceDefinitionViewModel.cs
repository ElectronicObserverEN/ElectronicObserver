using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.Reference;

public sealed partial class ReferenceDefinitionViewModel : ConditionDisplayerViewModel
{
	private ConditionFactory ConditionFactory { get; }
	private ConditionDisplayerService ConditionDisplayerService { get; }
	public ReferenceDefinition Model { get; }
	private ObservableCollection<ReferenceDefinitionViewModel> References { get; }

	public List<ConditionType> ConditionTypes { get; } = [.. Enum.GetValues<ConditionType>()];

	[ObservableProperty] public partial string? Name { get; set; }
	[ObservableProperty] public partial ConditionViewModel? Condition { get; set; }
	[ObservableProperty] public partial ConditionType ConditionType { get; set; }

	public override string Display => ConditionDisplayerService.ToDisplay(this);

	public ReferenceDefinitionViewModel(ConditionFactory conditionFactory,
		ConditionDisplayerService conditionDisplayerService,
		ReferenceDefinition model,
		ObservableCollection<ReferenceDefinitionViewModel> references)
	{
		ConditionFactory = conditionFactory;
		ConditionDisplayerService = conditionDisplayerService;
		Model = model;
		References = references;

		Name = Model.Name;
		CreateCondition(Model.Condition);

		ConditionChanged += RaiseDisplayChanged;
	}

	partial void OnNameChanged(string? value)
	{
		Model.Name = value;

		RaiseConditionChanged();
	}

	partial void OnConditionChanged(ConditionViewModel? value)
	{
		Model.Condition = value?.Model;

		RaiseConditionChanged();
	}

	private void CreateCondition(FleetCondition? model)
	{
		if (model is null) return;

		ConditionViewModel vm = ConditionFactory.CreateViewModel(model, References);
		vm.ConditionChanged += RaiseConditionChanged;
		Condition = vm;
	}

	[RelayCommand]
	private void CreateCondition()
	{
		FleetCondition? model = ConditionFactory.CreateModel(ConditionType);

		CreateCondition(model);
	}

	[RelayCommand]
	private void RemoveCondition()
	{
		Condition = null;
	}
}
