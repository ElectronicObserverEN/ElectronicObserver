using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipType;

public sealed partial class ShipTypeConditionViewModel : ConditionViewModel
{
	private ConditionDisplayerService ConditionDisplayerService { get; }
	public override ShipTypeCondition Model { get; }

	public List<ShipTypes> ShipTypes { get; } =
		[.. Enum.GetValues<ShipTypes>().Where(t => t is not (Core.Types.ShipTypes.Unknown or Core.Types.ShipTypes.All))];

	[ObservableProperty] public partial ShipTypes ShipType { get; set; } = Core.Types.ShipTypes.Destroyer;

	public override string Display => ConditionDisplayerService.ToDisplay(this);

	public ShipTypeConditionViewModel(
		ConditionDisplayerService conditionDisplayerService,
		ShipTypeCondition model)
	{
		ConditionDisplayerService = conditionDisplayerService;
		Model = model;

		ShipType = Model.ShipType;

		ConditionChanged += RaiseDisplayChanged;
	}

	partial void OnShipTypeChanged(ShipTypes value)
	{
		Model.ShipType = value;

		RaiseConditionChanged();
	}
}
