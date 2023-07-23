using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Settings.Notification.Base;

public class ConfigurationNotificationBaseTranslationViewModel : TranslationBaseViewModel
{
	public string IsEnabled => NotifyRes.EnableNotify;

	public string GroupSound => NotifyRes.Sound;
	public string PlaysSound => NotifyRes.Enable;
	public string LoopsSound => ConfigRes.Loop;
	public string Volume => ConfigRes.Volume;
	public string SoundVolumeToolTip => ConfigurationNotifierResources.SoundVolumeToolTip;
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
	public string ShowWithActivationToolTip => ConfigurationNotifierResources.ShowWithActivationToolTip;

	public string TestNotification => ConfigurationNotifierResources.TestNotification;

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

	public string GroupAnchorageRepair => ConfigurationNotifierResources.GroupAnchorageRepair;
	public string TriggerWhen => ConfigurationNotifierResources.TriggerWhen;
	public string AnchorageRepairNotificationLevelToolTip => ConfigurationNotifierResources.AnchorageRepairNotificationLevelToolTip;

	public string GroupBaseAirCorps => ConfigurationNotifierResources.GroupBaseAirCorps;
	public string BaseAirCorps_NotSupplied => ConfigurationNotifierResources.BaseAirCorps_NotSupplied;
	public string BaseAirCorps_NotSuppliedToolTip => ConfigurationNotifierResources.BaseAirCorps_NotSuppliedToolTip;
	public string BaseAirCorps_Standby => ConfigurationNotifierResources.BaseAirCorps_Standby;
	public string BaseAirCorps_StandbyToolTip => ConfigurationNotifierResources.BaseAirCorps_StandbyToolTip;
	public string BaseAirCorps_NormalMap => ConfigurationNotifierResources.BaseAirCorps_NormalMap;
	public string BaseAirCorps_NormalMapToolTip => ConfigurationNotifierResources.BaseAirCorps_NormalMapToolTip;
	public string BaseAirCorps_SquadronRelocation => ConfigurationNotifierResources.BaseAirCorps_SquadronRelocation;
	public string BaseAirCorps_SquadronRelocationToolTip => ConfigurationNotifierResources.BaseAirCorps_SquadronRelocationToolTip;
	public string BaseAirCorps_Tired => ConfigurationNotifierResources.BaseAirCorps_Tired;
	public string BaseAirCorps_TiredToolTip => ConfigurationNotifierResources.BaseAirCorps_TiredToolTip;
	public string BaseAirCorps_Retreat => ConfigurationNotifierResources.BaseAirCorps_Retreat;
	public string BaseAirCorps_RetreatToolTip => ConfigurationNotifierResources.BaseAirCorps_RetreatToolTip;
	public string BaseAirCorps_EventMap => ConfigurationNotifierResources.BaseAirCorps_EventMap;
	public string BaseAirCorps_EventMapToolTip => ConfigurationNotifierResources.BaseAirCorps_EventMapToolTip;
	public string BaseAirCorps_EquipmentRelocation => ConfigurationNotifierResources.BaseAirCorps_EquipmentRelocation;
	public string BaseAirCorps_EquipmentRelocationToolTip => ConfigurationNotifierResources.BaseAirCorps_EquipmentRelocationToolTip;
	public string BaseAirCorps_NotOrganized => ConfigurationNotifierResources.BaseAirCorps_NotOrganized;
	public string BaseAirCorps_NotOrganizedToolTip => ConfigurationNotifierResources.BaseAirCorps_NotOrganizedToolTip;
	public string BaseAirCorps_Rest => ConfigurationNotifierResources.BaseAirCorps_Rest;
	public string BaseAirCorps_RestToolTip => ConfigurationNotifierResources.BaseAirCorps_RestToolTip;
	
	public string GroupBattleEnd => ConfigurationNotifierResources.GroupBattleEnd;
	public string BattleEnd_IdleTimerEnabled => ConfigurationNotifierResources.BattleEnd_IdleTimerEnabled;
	public string BattleEnd_IdleTimerEnabledToolTip => ConfigurationNotifierResources.BattleEnd_IdleTimerEnabledToolTip;
	public string BattleEnd_IdleTime => ConfigurationNotifierResources.BattleEnd_IdleTime;
	public string BattleEnd_IdleTimeToolTip => ConfigurationNotifierResources.BattleEnd_IdleTimeToolTip;

	public string SettingsWillBeAppliedForTest => ConfigurationNotifierResources.SettingsWillBeAppliedForTest;

	public string Error => ConfigurationResources.DialogCaptionErrorTitle;

	public string Title => NotifyRes.Title;
}
