using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.Reference;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.Not;

public sealed partial class NotConditionViewModel : ConditionViewModel
{
	private ConditionFactory ConditionFactory { get; }
	private ConditionDisplayerService ConditionDisplayerService { get; }
	public override NotCondition Model { get; }
	private ObservableCollection<ReferenceDefinitionViewModel> References { get; }

	public List<ConditionType> ConditionTypes { get; } = [.. Enum.GetValues<ConditionType>()];

	[ObservableProperty] public partial ConditionViewModel? Condition { get; set; }
	[ObservableProperty] public partial ConditionType ConditionType { get; set; }

	public override string Display => ConditionDisplayerService.ToDisplay(this);

	public NotConditionViewModel(
		ConditionFactory conditionFactory,
		ConditionDisplayerService conditionDisplayerService,
		NotCondition model,
		ObservableCollection<ReferenceDefinitionViewModel> references)
	{
		ConditionFactory = conditionFactory;
		ConditionDisplayerService = conditionDisplayerService;
		Model = model;
		References = references;

		Condition = ConditionFactory.CreateViewModel(model.Condition, References);

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
