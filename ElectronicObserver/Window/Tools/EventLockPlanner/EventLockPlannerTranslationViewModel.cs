using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Tools.EventLockPlanner;

public class EventLockPlannerTranslationViewModel : TranslationBaseViewModel
{
	public string Title => EventLockPlanner.Title;

	public string File => EventLockPlanner.File;
	public string CopyToClipboard => EventLockPlanner.CopyToClipboard;
	public string LoadFromClipboard => EventLockPlanner.LoadFromClipboard;
	public string LoadEventLocks => EventLockPlanner.LoadEventLocks;

	public string View => EventLockPlanner.View;
	public string ShowFinishedPhases => EventLockPlanner.ShowFinishedPhases;

	public string LockLoadWarningText => EventLockPlanner.LockLoadWarningText;
	public string Warning => EventLockPlanner.Warning;
	public string FailedToLoadLockData => EventLockPlanner.FailedToLoadLockData;

	public string AddLock => EventLockPlanner.AddLock;
	public string RemoveLock => EventLockPlanner.RemoveLock;

	public string AddPhase => EventLockPlanner.AddPhase;
	public string RemovePhase => EventLockPlanner.RemovePhase;
	public string AssignLock => EventLockPlanner.AssignLock;
	public string IsFinished => EventLockPlanner.IsFinished;

	public string Remove => EventLockPlanner.Remove;

	public string ShipTypeToggle => EventLockPlanner.ShipTypeToggle;

	public string ShipType => Properties.Window.FormShipGroup.ShipView_ShipType;
	public string Name => Properties.Window.FormShipGroup.ShipView_Name;
	public string Firepower => GeneralRes.Firepower;
	public string NightBattlePower => Properties.Window.FormShipGroup.ShipView_NightBattlePower;
	public string ASW => GeneralRes.ASW;
	public string Luck => GeneralRes.Luck;
	public string Daihatsu => EventLockPlanner.Daihatsu;
	public string Tank => EventLockPlanner.Tank;
	public string Fcf => EventLockPlanner.Fcf;
	public string Expansion => GeneralRes.Expansion;
}
