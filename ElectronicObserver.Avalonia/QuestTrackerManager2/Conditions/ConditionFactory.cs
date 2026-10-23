using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using ElectronicObserver.Avalonia.Dialogs;
using ElectronicObserver.Avalonia.Dialogs.ShipSelector;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.Group;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.Not;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.Reference;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipClass;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipCount;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipId;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipLevel;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipNationality;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipPosition;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipType;
using ElectronicObserver.Core.Types.Data;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions;

public sealed class ConditionFactory(
	IKCDatabase kcDatabase,
	ConditionDisplayerService conditionDisplayerService,
	DialogServiceBase dialogService,
	ShipSelectorFactory shipSelectorFactory)
{
	private IKCDatabase KcDatabase { get; } = kcDatabase;
	private ConditionDisplayerService ConditionDisplayerService { get; } = conditionDisplayerService;
	private DialogServiceBase DialogService { get; } = dialogService;
	private ShipSelectorFactory ShipSelectorFactory { get; } = shipSelectorFactory;

	public FleetCondition? CreateModel(ConditionType conditionType) => conditionType switch
	{
		ConditionType.Group => new GroupCondition(),
		ConditionType.Not => new NotCondition(),
		ConditionType.Reference => new ReferenceCondition(),
		ConditionType.ShipId => new ShipIdCondition(),
		ConditionType.ShipType => new ShipTypeCondition(),
		ConditionType.ShipClass => new ShipClassCondition(),
		ConditionType.ShipPosition => new PositionCondition(),
		ConditionType.ShipCount => new ShipCountCondition(),
		ConditionType.ShipLevel => new ShipLevelCondition(),
		ConditionType.ShipNationality => new ShipNationalityCondition(),

		_ => null,
	};

	[return: NotNullIfNotNull(nameof(condition))]
	public ConditionViewModel? CreateViewModel(FleetCondition? condition,
		ObservableCollection<ReferenceDefinitionViewModel> references)
		=> condition switch
		{
			GroupCondition g => new GroupConditionViewModel(this, ConditionDisplayerService, g, references),
			NotCondition n => new NotConditionViewModel(this, ConditionDisplayerService, n, references),
			ReferenceCondition r => new ReferenceConditionViewModel(ConditionDisplayerService, r, references),
			ShipIdCondition i => new ShipIdConditionViewModel(KcDatabase, ConditionDisplayerService, DialogService, ShipSelectorFactory, i),
			ShipTypeCondition t => new ShipTypeConditionViewModel(ConditionDisplayerService, t),
			ShipClassCondition c => new ShipClassConditionViewModel(KcDatabase, ConditionDisplayerService, c),
			PositionCondition p => new PositionConditionViewModel(this, ConditionDisplayerService, p, references),
			ShipCountCondition c => new CountConditionViewModel(this, ConditionDisplayerService, c, references),
			ShipLevelCondition l => new ShipLevelConditionViewModel(ConditionDisplayerService, l),
			ShipNationalityCondition n => new ShipNationalityConditionViewModel(ConditionDisplayerService, n),

			_ => null,
		};

	public ReferenceDefinitionViewModel CreateViewModel(ReferenceDefinition condition,
		ObservableCollection<ReferenceDefinitionViewModel> references)
		=> new(this, ConditionDisplayerService, condition, references);
}
