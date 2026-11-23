using System.Collections.ObjectModel;

namespace ElectronicObserver.Avalonia.UiBlocker;

public sealed class BlockerEngine(
	IUiBlockerService uiBlockerService,
	ObservableCollection<UiBlockerViewModel> blockers)
{
	private IUiBlockerService UiBlockerService { get; } = uiBlockerService;
	private ObservableCollection<UiBlockerViewModel> Blockers { get; } = blockers;

	public void OnPort(IGameStateProvider stateProvider) =>
		Trigger(UiBlockerTrigger.Port, stateProvider);

	public void OnSortieAdvance(IGameStateProvider stateProvider) =>
		Trigger(UiBlockerTrigger.SortieAdvance, stateProvider);

	public void OnBattleStarted(IGameStateProvider stateProvider) =>
		Trigger(UiBlockerTrigger.BattleStarted, stateProvider);

	public void OnBattleEnded(IGameStateProvider stateProvider) =>
		Trigger(UiBlockerTrigger.BattleEnded, stateProvider);

	public void HideBlocker(UiBlockerViewModel uiBlocker)
	{
		UiBlockerService.HideBlockers(uiBlocker.UiElements);
	}

	private void Trigger(UiBlockerTrigger trigger, IGameStateProvider stateProvider)
	{
		List<UiElementViewModel> elements = [.. Blockers.Where(b => b.IsEnabled).SelectMany(b => b.UiElements)];

		IEnumerable<UiElementViewModel> showBlockers = elements
			.Where(e => e.Model.ShowTriggers.Contains(trigger))
			.Where(e => ConditionsPass(e.Model, stateProvider));

		UiBlockerService.ShowBlockers(showBlockers);

		UiBlockerService.HideBlockers(elements.Where(e => e.Model.HideTriggers.Contains(trigger)));
	}

	private static bool ConditionsPass(UiElementModel e, IGameStateProvider stateProvider) =>
		e.Conditions.All(c => ConditionEvaluator.Evaluate(c, stateProvider));
}
