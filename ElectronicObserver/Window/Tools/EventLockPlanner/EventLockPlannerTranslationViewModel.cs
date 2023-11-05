using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Tools.EventLockPlanner;

public class EventLockPlannerTranslationViewModel : TranslationBaseViewModel
{
	public string Title => EventLockPlannerResources.Title;

	public string File => EventLockPlannerResources.File;
	public string CopyToClipboard => EventLockPlannerResources.CopyToClipboard;
	public string LoadFromClipboard => EventLockPlannerResources.LoadFromClipboard;
	public string LoadEventLocks => EventLockPlannerResources.LoadEventLocks;

	public string View => EventLockPlannerResources.View;
	public string ShowFinishedPhases => EventLockPlannerResources.ShowFinishedPhases;

	public string LockLoadWarningText => EventLockPlannerResources.LockLoadWarningText;
	public string Warning => EventLockPlannerResources.Warning;
	public string FailedToLoadLockData => EventLockPlannerResources.FailedToLoadLockData;

	public string AddLock => EventLockPlannerResources.AddLock;
	public string RemoveLock => EventLockPlannerResources.RemoveLock;

	public string AddPhase => EventLockPlannerResources.AddPhase;
	public string RemovePhase => EventLockPlannerResources.RemovePhase;
	public string AssignLock => EventLockPlannerResources.AssignLock;
	public string IsFinished => EventLockPlannerResources.IsFinished;

	public string Remove => EventLockPlannerResources.Remove;

	public string ShipTypeToggle => EventLockPlannerResources.ShipTypeToggle;

	public string ShipType => ShipGroupResources.ShipView_ShipType;
	public string Name => ShipGroupResources.ShipView_Name;
	public string Firepower => GeneralRes.Firepower;
	public string NightBattlePower => ShipGroupResources.ShipView_NightBattlePower;
	public string ASW => GeneralRes.ASW;
	public string Luck => GeneralRes.Luck;
	public string Daihatsu => EventLockPlannerResources.Daihatsu;
	public string Tank => EventLockPlannerResources.Tank;
	public string Fcf => EventLockPlannerResources.Fcf;
	public string Expansion => GeneralRes.Expansion;

	public string InvalidModelState => EventLockPlannerResources.InvalidModelState;
}
