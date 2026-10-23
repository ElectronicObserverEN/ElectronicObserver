using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.Reference;

public sealed partial class ReferenceConditionViewModel : ConditionViewModel
{
	private ConditionDisplayerService ConditionDisplayerService { get; }
	public override ReferenceCondition Model { get; }
	public ObservableCollection<ReferenceDefinitionViewModel> References { get; }

	[ObservableProperty] public partial ReferenceDefinitionViewModel? Reference { get; set; }

	public override string Display => ConditionDisplayerService.ToDisplay(this);

	public ReferenceConditionViewModel(
		ConditionDisplayerService conditionDisplayerService,
		ReferenceCondition model,
		ObservableCollection<ReferenceDefinitionViewModel> references)
	{
		ConditionDisplayerService = conditionDisplayerService;
		Model = model;
		References = references;

		Reference = References.FirstOrDefault(r => r.Name == Model.Reference);

		ConditionChanged += RaiseDisplayChanged;
	}

	partial void OnReferenceChanged(ReferenceDefinitionViewModel? value)
	{
		Model.Reference = value?.Name;

		RaiseConditionChanged();
	}
}
