using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Settings.Window;

public class ConfigurationWindowTranslationViewModel : TranslationBaseViewModel
{
	public string Life_CanCloseFloatWindowInLock => ConfigRes.Life_CanCloseFloatWindowInLock;
	public string Life_CanCloseFloatWindowInLockToolTip => ConfigurationResources.Life_CanCloseFloatWindowInLockToolTip;
	
	public string Life_LockLayout => ConfigurationResources.Life_LockLayout;
	public string Life_LockLayoutToolTip => ConfigurationResources.Life_LockLayoutToolTip;
	
	public string ClockMode => ConfigurationResources.ClockMode;

	public string Life_ClockFormat_ServerTime => ConfigurationResources.Life_ClockFormat_ServerTime;
	public string Life_ClockFormat_PvpReset => ConfigurationResources.Life_ClockFormat_PvpReset;
	public string Life_ClockFormat_QuestReset => ConfigurationResources.Life_ClockFormat_QuestReset;

	public string Life_ShowStatusBar => ConfigurationResources.Life_ShowStatusBar;
	public string Life_CheckUpdateInformation => ConfigurationResources.Life_CheckUpdateInformation;
	public string LayoutFile => ConfigRes.LayoutFile;
	public string Life_TopMost => ConfigurationResources.Life_TopMost;
	public string Life_ConfirmOnClosing => ConfigurationResources.Life_ConfirmOnClosing;
}
