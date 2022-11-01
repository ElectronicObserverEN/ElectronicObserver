using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Dialog;

namespace ElectronicObserver.Window.Settings.Window;

public class ConfigurationWindowTranslationViewModel : TranslationBaseViewModel
{
	public string Life_CanCloseFloatWindowInLock => ConfigRes.Life_CanCloseFloatWindowInLock;
	public string Life_CanCloseFloatWindowInLockToolTip => Properties.Window.Dialog.DialogConfiguration.Life_CanCloseFloatWindowInLockToolTip;
	
	public string Life_LockLayout => Properties.Window.Dialog.DialogConfiguration.Life_LockLayout;
	public string Life_LockLayoutToolTip => Properties.Window.Dialog.DialogConfiguration.Life_LockLayoutToolTip;
	
	public string ClockMode => Properties.Window.Dialog.DialogConfiguration.ClockMode;

	public string Life_ClockFormat_ServerTime => Properties.Window.Dialog.DialogConfiguration.Life_ClockFormat_ServerTime;
	public string Life_ClockFormat_PvpReset => Properties.Window.Dialog.DialogConfiguration.Life_ClockFormat_PvpReset;
	public string Life_ClockFormat_QuestReset => Properties.Window.Dialog.DialogConfiguration.Life_ClockFormat_QuestReset;

	public string Life_ShowStatusBar => Properties.Window.Dialog.DialogConfiguration.Life_ShowStatusBar;
	public string Life_CheckUpdateInformation => Properties.Window.Dialog.DialogConfiguration.Life_CheckUpdateInformation;
	public string LayoutFile => ConfigRes.LayoutFile;
	public string Life_TopMost => Properties.Window.Dialog.DialogConfiguration.Life_TopMost;
	public string Life_ConfirmOnClosing => Properties.Window.Dialog.DialogConfiguration.Life_ConfirmOnClosing;
}
