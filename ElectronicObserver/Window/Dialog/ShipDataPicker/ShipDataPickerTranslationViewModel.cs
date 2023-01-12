using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Dialog.ShipDataPicker;

public class ShipDataPickerTranslationViewModel : TranslationBaseViewModel
{
	public string Title => ShipDataPicker.Title;

	public string File => ShipDataPicker.File;
	public string CopyToClipboard => ShipDataPicker.CopyToClipboard;
	public string LoadFromClipboard => ShipDataPicker.LoadFromClipboard;
	public string LoadEventLocks => ShipDataPicker.LoadEventLocks;

	public string View => ShipDataPicker.View;
	public string ShowFinishedPhases => ShipDataPicker.ShowFinishedPhases;

	public string LockLoadWarningText => ShipDataPicker.LockLoadWarningText;
	public string Warning => ShipDataPicker.Warning;
	public string FailedToLoadLockData => ShipDataPicker.FailedToLoadLockData;

	public string AddLock => ShipDataPicker.AddLock;
	public string RemoveLock => ShipDataPicker.RemoveLock;

	public string AddPhase => ShipDataPicker.AddPhase;
	public string RemovePhase => ShipDataPicker.RemovePhase;
	public string AssignLock => ShipDataPicker.AssignLock;
	public string IsFinished => ShipDataPicker.IsFinished;

	public string Remove => ShipDataPicker.Remove;

	public string ShipTypeToggle => ShipDataPicker.ShipTypeToggle;

	public string ShipType => Properties.Window.FormShipGroup.ShipView_ShipType;
	public string Name => Properties.Window.FormShipGroup.ShipView_Name;
	public string Firepower => GeneralRes.Firepower;
	public string NightBattlePower => Properties.Window.FormShipGroup.ShipView_NightBattlePower;
	public string ASW => GeneralRes.ASW;
	public string Luck => GeneralRes.Luck;
	public string Daihatsu => ShipDataPicker.Daihatsu;
	public string Tank => ShipDataPicker.Tank;
	public string Fcf => ShipDataPicker.Fcf;
	public string Expansion => GeneralRes.Expansion;

	public string InvalidModelState => ShipDataPicker.InvalidModelState;
}
