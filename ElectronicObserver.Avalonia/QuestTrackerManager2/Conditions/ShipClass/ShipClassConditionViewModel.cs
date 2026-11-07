using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Core.Types.Data;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipClass;

public partial class ShipClassConditionViewModel : ConditionViewModel
{
	private IKCDatabase KcDatabase { get; }
	private ConditionDisplayerService ConditionDisplayerService { get; }
	public override ShipClassCondition Model { get; }

	// Deferred on purpose - MasterShips is only populated once the game data has actually loaded, which can
	// happen well after this view model is constructed.
	public IEnumerable<Core.Types.ShipClass> ShipClasses => KcDatabase.MasterShips.Values
		.Where(s => !s.IsAbyssalShip)
		.DistinctBy(s => s.ShipClass)
		.OrderBy(s => s.ShipType)
		.ThenBy(s => s.SortID)
		.Select(s => (Core.Types.ShipClass)s.ShipClass)
		.Prepend(Core.Types.ShipClass.Unknown);

	[ObservableProperty] public partial Core.Types.ShipClass ShipClass { get; set; }

	public override string Display => ConditionDisplayerService.ToDisplay(this);

	public ShipClassConditionViewModel(
		IKCDatabase kcDatabase,
		ConditionDisplayerService conditionDisplayerService,
		ShipClassCondition model)
	{
		KcDatabase = kcDatabase;
		ConditionDisplayerService = conditionDisplayerService;
		Model = model;

		ShipClass = model.ShipClass;

		ConditionChanged += RaiseDisplayChanged;
	}

	partial void OnShipClassChanged(Core.Types.ShipClass value)
	{
		Model.ShipClass = value;
		
		RaiseConditionChanged();
	}
}
