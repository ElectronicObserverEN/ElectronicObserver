using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Dialog;

namespace ElectronicObserver.Window.Settings.Notification.Base;

public class ConfigurationNotificationBaseTranslationViewModel : TranslationBaseViewModel
{
	public string IsEnabled => NotifyRes.EnableNotify;

	public string GroupSound => NotifyRes.Sound;
	public string PlaysSound => NotifyRes.Enable;
	public string LoopsSound => ConfigRes.Loop;
	public string Volume => ConfigRes.Volume;
	public string SoundVolumeToolTip => Properties.Window.Dialog.DialogConfigurationNotifier.SoundVolumeToolTip;
	public string Dir => "Dir";
	public string SoundPathDirectorizeToolTip => NotifyRes.SoundPathDirectorizeToolTip;

	public string GroupImage => NotifyRes.Image;
	public string DrawsImage => NotifyRes.Enable;

	public string GroupDialog => NotifyRes.NotifyDialog;
	public string ShowsDialog => NotifyRes.Enable;
	public string Alignment => NotifyRes.Alignment + ":";
	public string AlignHint => NotifyRes.AlignHint;
	public string Location => NotifyRes.Location + ":";
	public string LocYHint => NotifyRes.LocYHint;
	public string LocXHint => NotifyRes.LocXHint;
	public string DrawsMessage => NotifyRes.DisplayMessage;
	public string DisplayMessageHint => NotifyRes.DisplayMessageHint;
	public string HasFormBorder => NotifyRes.DisplayWindowBorder;
	public string WindowBorderHint => NotifyRes.WindowBorderHint;

	public string HurryBy => NotifyRes.HurryBy + ":";
	public string HurryHint => NotifyRes.HurryHint;
	public string Sec => NotifyRes.Sec;
	public string AutoClose => NotifyRes.AutoClose + ":";
	public string IntervalHint => NotifyRes.IntervalHint;
	public string TopMostFlag => NotifyRes.ShowOnTop;

	public string ForeColorPreview => NotifyRes.ForeColorDisplay + ":";
	public string ForeColorDispHint => NotifyRes.ForeColorDispHint;

	public string BackColorPreview => NotifyRes.BackColorDisplay + ":";
	public string BackColorDispHint => NotifyRes.BackColorDispHint;

	public string CloseOn => NotifyRes.CloseOn + "：";

	public string ButtonOk => "OK";
	public string ButtonCancel => ConfigRes.Cancel;
	public string ButtonTest => NotifyRes.Test;
	public string ShowWithActivation => NotifyRes.ShowWithActivation;
	public string ShowWithActivationToolTip => Properties.Window.Dialog.DialogConfigurationNotifier.ShowWithActivationToolTip;

	public string TestNotification => Properties.Window.Dialog.DialogConfigurationNotifier.TestNotification;

	public string GroupDamage => NotifyRes.DamageOptions;
	public string NotifiesBefore => NotifyRes.NotifyBefore;
	public string NotifyBeforeHint => NotifyRes.NotifyBeforeHint;
	public string ContainsNotLockedShip => NotifyRes.IncludeUnlocked;
	public string NotifiesAtEndpoint => NotifyRes.NotifyEndNodes;
	public string NotifiesNow => NotifyRes.NotifyNow;
	public string NotifyNowHint => NotifyRes.NotifyNowHint;
	public string ContainsSafeShip => NotifyRes.IncludeDamecon;
	public string MinLv => NotifyRes.MinLv + ":";
	public string LvHint => NotifyRes.LvHint;
	public string NotifiesAfter => NotifyRes.NotifyAfter;
	public string NotifyAfterHint => NotifyRes.NotifyAfterHint;
	public string ContainsFlagship => NotifyRes.IncludeFlagship;

	public string GroupAnchorageRepair => Properties.Window.Dialog.DialogConfigurationNotifier.GroupAnchorageRepair;
	public string TriggerWhen => Properties.Window.Dialog.DialogConfigurationNotifier.TriggerWhen;
	public string AnchorageRepairNotificationLevelToolTip => Properties.Window.Dialog.DialogConfigurationNotifier.AnchorageRepairNotificationLevelToolTip;

	public string GroupBaseAirCorps => Properties.Window.Dialog.DialogConfigurationNotifier.GroupBaseAirCorps;
	public string BaseAirCorps_NotSupplied => Properties.Window.Dialog.DialogConfigurationNotifier.BaseAirCorps_NotSupplied;
	public string BaseAirCorps_NotSuppliedToolTip => Properties.Window.Dialog.DialogConfigurationNotifier.BaseAirCorps_NotSuppliedToolTip;
	public string BaseAirCorps_Standby => Properties.Window.Dialog.DialogConfigurationNotifier.BaseAirCorps_Standby;
	public string BaseAirCorps_StandbyToolTip => Properties.Window.Dialog.DialogConfigurationNotifier.BaseAirCorps_StandbyToolTip;
	public string BaseAirCorps_NormalMap => Properties.Window.Dialog.DialogConfigurationNotifier.BaseAirCorps_NormalMap;
	public string BaseAirCorps_NormalMapToolTip => Properties.Window.Dialog.DialogConfigurationNotifier.BaseAirCorps_NormalMapToolTip;
	public string BaseAirCorps_SquadronRelocation => Properties.Window.Dialog.DialogConfigurationNotifier.BaseAirCorps_SquadronRelocation;
	public string BaseAirCorps_SquadronRelocationToolTip => Properties.Window.Dialog.DialogConfigurationNotifier.BaseAirCorps_SquadronRelocationToolTip;
	public string BaseAirCorps_Tired => Properties.Window.Dialog.DialogConfigurationNotifier.BaseAirCorps_Tired;
	public string BaseAirCorps_TiredToolTip => Properties.Window.Dialog.DialogConfigurationNotifier.BaseAirCorps_TiredToolTip;
	public string BaseAirCorps_Retreat => Properties.Window.Dialog.DialogConfigurationNotifier.BaseAirCorps_Retreat;
	public string BaseAirCorps_RetreatToolTip => Properties.Window.Dialog.DialogConfigurationNotifier.BaseAirCorps_RetreatToolTip;
	public string BaseAirCorps_EventMap => Properties.Window.Dialog.DialogConfigurationNotifier.BaseAirCorps_EventMap;
	public string BaseAirCorps_EventMapToolTip => Properties.Window.Dialog.DialogConfigurationNotifier.BaseAirCorps_EventMapToolTip;
	public string BaseAirCorps_EquipmentRelocation => Properties.Window.Dialog.DialogConfigurationNotifier.BaseAirCorps_EquipmentRelocation;
	public string BaseAirCorps_EquipmentRelocationToolTip => Properties.Window.Dialog.DialogConfigurationNotifier.BaseAirCorps_EquipmentRelocationToolTip;
	public string BaseAirCorps_NotOrganized => Properties.Window.Dialog.DialogConfigurationNotifier.BaseAirCorps_NotOrganized;
	public string BaseAirCorps_NotOrganizedToolTip => Properties.Window.Dialog.DialogConfigurationNotifier.BaseAirCorps_NotOrganizedToolTip;
	public string BaseAirCorps_Rest => Properties.Window.Dialog.DialogConfigurationNotifier.BaseAirCorps_Rest;
	public string BaseAirCorps_RestToolTip => Properties.Window.Dialog.DialogConfigurationNotifier.BaseAirCorps_RestToolTip;
	
	public string GroupBattleEnd => Properties.Window.Dialog.DialogConfigurationNotifier.GroupBattleEnd;
	public string BattleEnd_IdleTimerEnabled => Properties.Window.Dialog.DialogConfigurationNotifier.BattleEnd_IdleTimerEnabled;
	public string BattleEnd_IdleTimerEnabledToolTip => Properties.Window.Dialog.DialogConfigurationNotifier.BattleEnd_IdleTimerEnabledToolTip;
	public string BattleEnd_IdleTime => Properties.Window.Dialog.DialogConfigurationNotifier.BattleEnd_IdleTime;
	public string BattleEnd_IdleTimeToolTip => Properties.Window.Dialog.DialogConfigurationNotifier.BattleEnd_IdleTimeToolTip;

	public string SettingsWillBeAppliedForTest => Properties.Window.Dialog.DialogConfigurationNotifier.SettingsWillBeAppliedForTest;

	public string Error => Properties.Window.Dialog.DialogConfiguration.DialogCaptionErrorTitle;

	public string Title => NotifyRes.Title;
}
