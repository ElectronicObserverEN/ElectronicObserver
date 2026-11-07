using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipLevel;

public sealed partial class ShipLevelConditionViewModel : ConditionViewModel
{
	private ConditionDisplayerService ConditionDisplayerService { get; }
	public override ShipLevelCondition Model { get; }

	[ObservableProperty] public partial int Level { get; set; }

	public override string Display => ConditionDisplayerService.ToDisplay(this);

	public ShipLevelConditionViewModel(
		ConditionDisplayerService conditionDisplayerService,
		ShipLevelCondition model)
	{
		ConditionDisplayerService = conditionDisplayerService;
		Model = model;

		Level = Model.Level;

		ConditionChanged += RaiseDisplayChanged;
	}

	partial void OnLevelChanged(int value)
	{
		Model.Level = value;

		RaiseConditionChanged();
	}
}
