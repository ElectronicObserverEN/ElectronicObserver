using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipNationality;

public sealed partial class ShipNationalityConditionViewModel : ConditionViewModel
{
	private ConditionDisplayerService ConditionDisplayerService { get; }
	public override ShipNationalityCondition Model { get; }

	public List<Core.Types.ShipNationality> ShipNationalities { get; } =
		[..Enum.GetValues<Core.Types.ShipNationality>().Where(n => n is not Core.Types.ShipNationality.Unknown)];

	[ObservableProperty] public partial Core.Types.ShipNationality Nationality { get; set; }

	public override string Display => ConditionDisplayerService.ToDisplay(this);

	public ShipNationalityConditionViewModel(
		ConditionDisplayerService conditionDisplayerService,
		ShipNationalityCondition model)
	{
		ConditionDisplayerService = conditionDisplayerService;
		Model = model;

		Nationality = Model.Nationality;

		ConditionChanged += RaiseDisplayChanged;
	}

	partial void OnNationalityChanged(Core.Types.ShipNationality value)
	{
		Model.Nationality = value;

		RaiseConditionChanged();
	}
}
