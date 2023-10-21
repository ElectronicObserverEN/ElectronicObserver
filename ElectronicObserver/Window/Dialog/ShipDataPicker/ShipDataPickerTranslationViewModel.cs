using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Dialog.ShipDataPicker;

public class ShipDataPickerTranslationViewModel : TranslationBaseViewModel
{
	public string Title => ShipDataPickerResources.Title;

	public string File => ShipDataPickerResources.File;
	public string CopyToClipboard => ShipDataPickerResources.CopyToClipboard;
	public string LoadFromClipboard => ShipDataPickerResources.LoadFromClipboard;
	public string LoadEventLocks => ShipDataPickerResources.LoadEventLocks;

	public string View => ShipDataPickerResources.View;
	public string ShowFinishedPhases => ShipDataPickerResources.ShowFinishedPhases;

	public string LockLoadWarningText => ShipDataPickerResources.LockLoadWarningText;
	public string Warning => ShipDataPickerResources.Warning;
	public string FailedToLoadLockData => ShipDataPickerResources.FailedToLoadLockData;

	public string AddLock => ShipDataPickerResources.AddLock;
	public string RemoveLock => ShipDataPickerResources.RemoveLock;

	public string AddPhase => ShipDataPickerResources.AddPhase;
	public string RemovePhase => ShipDataPickerResources.RemovePhase;
	public string AssignLock => ShipDataPickerResources.AssignLock;
	public string IsFinished => ShipDataPickerResources.IsFinished;

	public string Remove => ShipDataPickerResources.Remove;

	public string ShipTypeToggle => ShipDataPickerResources.ShipTypeToggle;

	public string ShipType => ShipGroupResources.ShipView_ShipType;
	public string Name => ShipGroupResources.ShipView_Name;
	public string Firepower => GeneralRes.Firepower;
	public string NightBattlePower => ShipGroupResources.ShipView_NightBattlePower;
	public string ASW => GeneralRes.ASW;
	public string Luck => GeneralRes.Luck;
	public string Daihatsu => ShipDataPickerResources.Daihatsu;
	public string Tank => ShipDataPickerResources.Tank;
	public string Fcf => ShipDataPickerResources.Fcf;
	public string Expansion => GeneralRes.Expansion;

	public string InvalidModelState => ShipDataPickerResources.InvalidModelState;
}
