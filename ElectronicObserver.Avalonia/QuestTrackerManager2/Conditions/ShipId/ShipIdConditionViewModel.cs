using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Avalonia.Dialogs;
using ElectronicObserver.Avalonia.Dialogs.ShipSelector;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Data;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipId;

public partial class ShipIdConditionViewModel : ConditionViewModel
{
	private IKCDatabase KcDatabase { get; }
	private ConditionDisplayerService ConditionDisplayerService { get; }
	private ShipSelectorFactory ShipSelectorFactory { get; }
	private ShipSelectorViewModel ShipSelectorViewModel => ShipSelectorFactory.QuestTrackerManager;
	private DialogServiceBase DialogService { get; }
	public override ShipIdCondition Model { get; }

	public List<RemodelComparison> RemodelComparisonTypes { get; } = [.. Enum.GetValues<RemodelComparison>()];

	// Looked up live (not cached) for the same reason as ExpeditionTaskViewModel.SelectedExpedition -
	// MasterShips is only populated once the game data has actually loaded, which can happen well after this
	// view model is constructed. A one-time lookup that falls back to a hardcoded ship on a miss would silently
	// overwrite Model.Id with that fallback's ID instead of just leaving the persisted value alone.
	public IShipDataMaster? Ship
	{
		get => KcDatabase.MasterShips[(int)Model.Id];
		set
		{
			if (value is null) return;

			Model.Id = value.ShipId;
			OnPropertyChanged();
			RaiseConditionChanged();
		}
	}

	[ObservableProperty] public partial RemodelComparison RemodelComparisonType { get; set; }

	public override string Display => ConditionDisplayerService.ToDisplay(this);

	public ShipIdConditionViewModel(
		IKCDatabase kcDatabase,
		ConditionDisplayerService conditionDisplayerService,
		DialogServiceBase dialogService,
		ShipSelectorFactory shipSelectorFactory,
		ShipIdCondition model)
	{
		KcDatabase = kcDatabase;
		ConditionDisplayerService = conditionDisplayerService;
		DialogService = dialogService;
		ShipSelectorFactory = shipSelectorFactory;
		Model = model;

		RemodelComparisonType = model.Remodel;

		ConditionChanged += RaiseDisplayChanged;
	}

	partial void OnRemodelComparisonTypeChanged(RemodelComparison value)
	{
		Model.Remodel = value;

		RaiseConditionChanged();
	}

	[RelayCommand]
	private async Task AddShip()
	{
		await DialogService.ShowDialogAsync(ShipSelectorViewModel);

		if (ShipSelectorViewModel.SelectedShip is null) return;

		Ship = ShipSelectorViewModel.SelectedShip.MasterShip;
	}
}
