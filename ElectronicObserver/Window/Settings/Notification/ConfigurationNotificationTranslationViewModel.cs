using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Dialog;

namespace ElectronicObserver.Window.Settings.Notification;

public class ConfigurationNotificationTranslationViewModel : TranslationBaseViewModel
{
	public string Title => ConfigRes.Notification;
	public string silenceFullscreen => Properties.Window.Dialog.DialogConfiguration.silenceFullscreen;
	public string Notification_Silencio => Properties.Window.Dialog.DialogConfiguration.Notification_Silencio;
	public string Notification_SilencioToolTip => Properties.Window.Dialog.DialogConfiguration.Notification_SilencioToolTip;
	public string Notification_AnchorageRepair => ConfigRes.AnchorageRepairFinish + ConfigRes.NotificationSetting;
	public string ApplyonOK => ConfigRes.ApplyonOK;
	public string Notification_Damage => ConfigRes.TaihaAdvance + ConfigRes.NotificationSetting;
	public string Notification_Condition => ConfigRes.FatigueRestore + ConfigRes.NotificationSetting;
	public string Notification_Repair => ConfigRes.DockEnd + ConfigRes.NotificationSetting;
	public string Notification_Construction => ConfigRes.ConstructEnd + ConfigRes.NotificationSetting;
	public string Notification_Expedition => ConfigRes.ExpedReturn + ConfigRes.NotificationSetting;
	public string Notification_RemodelLevel => Properties.Window.Dialog.DialogConfiguration.Notification_RemodelLevel;
	public string Notification_BaseAirCorps => Properties.Window.Dialog.DialogConfiguration.Notification_BaseAirCorps;
	public string Notification_BattleEnd => Properties.Window.Dialog.DialogConfiguration.Notification_BattleEnd;
}
