using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Settings.Notification;

public class ConfigurationNotificationTranslationViewModel : TranslationBaseViewModel
{
	public string Title => ConfigRes.Notification;
	public string silenceFullscreen => ConfigurationResources.silenceFullscreen;
	public string Notification_Silencio => ConfigurationResources.Notification_Silencio;
	public string Notification_SilencioToolTip => ConfigurationResources.Notification_SilencioToolTip;
	public string Notification_AnchorageRepair => ConfigRes.AnchorageRepairFinish + ConfigRes.NotificationSetting;
	public string ApplyonOK => ConfigRes.ApplyonOK;
	public string Notification_Damage => ConfigRes.TaihaAdvance + ConfigRes.NotificationSetting;
	public string Notification_Condition => ConfigRes.FatigueRestore + ConfigRes.NotificationSetting;
	public string Notification_Repair => ConfigRes.DockEnd + ConfigRes.NotificationSetting;
	public string Notification_Construction => ConfigRes.ConstructEnd + ConfigRes.NotificationSetting;
	public string Notification_Expedition => ConfigRes.ExpedReturn + ConfigRes.NotificationSetting;
	public string Notification_RemodelLevel => ConfigurationResources.Notification_RemodelLevel;
	public string Notification_Training => ConfigurationResources.Notification_Training;
	public string Notification_BaseAirCorps => ConfigurationResources.Notification_BaseAirCorps;
	public string Notification_BattleEnd => ConfigurationResources.Notification_BattleEnd;
	public string Notification_HomePortSupply => NotifierRes.HomePortSupply + ConfigRes.NotificationSetting;
}
