using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Dialog;

namespace ElectronicObserver.Window.Settings.SubWindow.Browser;

public class ConfigurationBrowserTranslationViewModel : TranslationBaseViewModel
{
	public string EnableBrowser => ConfigRes.EnableBrowser;

	public string Zoom => Properties.Window.Dialog.DialogConfiguration.Zoom;
	public string FormBrowser_ZoomFit => ConfigRes.ZoomToFit;
	public string FitHint => ConfigRes.FitHint;

	public string FormBrowser_ConfirmAtRefresh => Properties.Window.Dialog.DialogConfiguration.FormBrowser_ConfirmAtRefresh;
	public string FormBrowser_ConfirmAtRefreshToolTip => Properties.Window.Dialog.DialogConfiguration.FormBrowser_ConfirmAtRefreshToolTip;

	public string FormBrowser_AppliesStyleSheet => ConfigRes.ApplyStyleSheet;
	public string ApplyStyleSheetHint => ConfigRes.ApplyStyleSheetHint;

	public string FormBrowser_UseGadgetRedirect => Properties.Window.Dialog.DialogConfiguration.FormBrowser_UseGadgetRedirect;
	public string FormBrowser_UseGadgetRedirectToolTip => Properties.Window.Dialog.DialogConfiguration.FormBrowser_UseGadgetRedirectToolTip;
	
	public string FormBrowser_IsContextMenuEnabled => Properties.Window.Dialog.DialogConfiguration.FormBrowser_ShowContextMenu;
	public string FormBrowser_ShowContextMenuToolTip => Properties.Window.Dialog.DialogConfiguration.FormBrowser_ShowContextMenuToolTip;

	public string LoginURL => ConfigRes.LoginURL;

	public string ToolMenuDockStyle => ConfigRes.ToolMenuDockStyle;

	public string FormBrowser_IsDMMreloadDialogDestroyable => Properties.Window.Dialog.DialogConfiguration.FormBrowser_IsDMMreloadDialogDestroyable;
	public string FormBrowser_IsDMMreloadDialogDestroyableToolTip => Properties.Window.Dialog.DialogConfiguration.FormBrowser_IsDMMreloadDialogDestroyableToolTip;

	public string Screenshot => Properties.Window.Dialog.DialogConfiguration.Screenshot;

	public string FormBrowser_ScreenShotFormat_AvoidTwitterDeterioration => Properties.Window.Dialog.DialogConfiguration.FormBrowser_ScreenShotFormat_AvoidTwitterDeterioration;
	public string FormBrowser_ScreenShotFormat_AvoidTwitterDeteriorationToolTip => Properties.Window.Dialog.DialogConfiguration.FormBrowser_ScreenShotFormat_AvoidTwitterDeteriorationToolTip;

	public string Output => Properties.Window.Dialog.DialogConfiguration.Output;
	public string SaveLocation => ConfigRes.SaveLocation;

	public string FormBrowser_HardwareAccelerationEnabled => Properties.Window.Dialog.DialogConfiguration.FormBrowser_HardwareAccelerationEnabled;
	public string FormBrowser_HardwareAccelerationEnabledToolTip => Properties.Window.Dialog.DialogConfiguration.FormBrowser_HardwareAccelerationEnabledToolTip;

	public string FormBrowser_PreserveDrawingBuffer => Properties.Window.Dialog.DialogConfiguration.FormBrowser_PreserveDrawingBuffer;
	public string FormBrowser_PreserveDrawingBufferToolTip => Properties.Window.Dialog.DialogConfiguration.FormBrowser_PreserveDrawingBufferToolTip;

	public string FormBrowser_ForceColorProfile => Properties.Window.Dialog.DialogConfiguration.FormBrowser_ForceColorProfile;
	public string FormBrowser_ForceColorProfileToolTip => Properties.Window.Dialog.DialogConfiguration.FormBrowser_ForceColorProfileToolTip;

	public string FormBrowser_SavesBrowserLog => Properties.Window.Dialog.DialogConfiguration.FormBrowser_SavesBrowserLog;
	public string FormBrowser_SavesBrowserLogTooltip => Properties.Window.Dialog.DialogConfiguration.FormBrowser_SavesBrowserLogTooltip;

	public string FormBrowser_UseVulkanWorkaround => Properties.Window.Dialog.DialogConfiguration.FormBrowser_UseVulkanWorkaround;
	public string FormBrowser_UseVulkanWorkaroundToolTip => Properties.Window.Dialog.DialogConfiguration.FormBrowser_UseVulkanWorkaroundToolTip;

	public string RestartNotice => Properties.Window.Dialog.DialogConfiguration.RestartNotice;
}
