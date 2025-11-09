using ElectronicObserver.Core.Properties;
using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Settings.SubWindow.Browser;

public class ConfigurationBrowserTranslationViewModel : TranslationBaseViewModel
{
	public string EnableBrowser => ConfigRes.EnableBrowser;

	public string Zoom => ConfigurationResources.Zoom;
	public string FormBrowser_ZoomFit => ConfigRes.ZoomToFit;
	public string FitHint => ConfigRes.FitHint;

	public string FormBrowser_ConfirmAtRefresh => ConfigurationResources.FormBrowser_ConfirmAtRefresh;
	public string FormBrowser_ConfirmAtRefreshToolTip => ConfigurationResources.FormBrowser_ConfirmAtRefreshToolTip;

	public string FormBrowser_AppliesStyleSheet => ConfigRes.ApplyStyleSheet;
	public string ApplyStyleSheetHint => ConfigRes.ApplyStyleSheetHint;
	
	public string FormBrowser_IsContextMenuEnabled => ConfigurationResources.FormBrowser_ShowContextMenu;
	public string FormBrowser_ShowContextMenuToolTip => ConfigurationResources.FormBrowser_ShowContextMenuToolTip;

	public string LoginURL => ConfigRes.LoginURL;

	public string ToolMenuDockStyle => ConfigRes.ToolMenuDockStyle;

	public string FormBrowser_IsDMMreloadDialogDestroyable => ConfigurationResources.FormBrowser_IsDMMreloadDialogDestroyable;
	public string FormBrowser_IsDMMreloadDialogDestroyableToolTip => ConfigurationResources.FormBrowser_IsDMMreloadDialogDestroyableToolTip;

	public string Screenshot => ConfigurationResources.Screenshot;

	public string FormBrowser_ScreenShotFormat_AvoidTwitterDeterioration => ConfigurationResources.FormBrowser_ScreenShotFormat_AvoidTwitterDeterioration;
	public string FormBrowser_ScreenShotFormat_AvoidTwitterDeteriorationToolTip => ConfigurationResources.FormBrowser_ScreenShotFormat_AvoidTwitterDeteriorationToolTip;

	public string Output => ConfigurationResources.Output;
	public string ScreenshotMode => $"{ScreenshotModeResources.Mode}：";
	public string ScreenshotModeDescription => ScreenshotModeResources.Description;
	public string SaveLocation => ConfigRes.SaveLocation;

	public string FormBrowser_HardwareAccelerationEnabled => ConfigurationResources.FormBrowser_HardwareAccelerationEnabled;
	public string FormBrowser_HardwareAccelerationEnabledToolTip => ConfigurationResources.FormBrowser_HardwareAccelerationEnabledToolTip;

	public string FormBrowser_PreserveDrawingBuffer => ConfigurationResources.FormBrowser_PreserveDrawingBuffer;
	public string FormBrowser_PreserveDrawingBufferToolTip => ConfigurationResources.FormBrowser_PreserveDrawingBufferToolTip;

	public string FormBrowser_ForceColorProfile => ConfigurationResources.FormBrowser_ForceColorProfile;
	public string FormBrowser_ForceColorProfileToolTip => ConfigurationResources.FormBrowser_ForceColorProfileToolTip;

	public string FormBrowser_SavesBrowserLog => ConfigurationResources.FormBrowser_SavesBrowserLog;
	public string FormBrowser_SavesBrowserLogTooltip => ConfigurationResources.FormBrowser_SavesBrowserLogTooltip;

	public string FormBrowser_UseVulkanWorkaround => ConfigurationResources.FormBrowser_UseVulkanWorkaround;
	public string FormBrowser_UseVulkanWorkaroundToolTip => ConfigurationResources.FormBrowser_UseVulkanWorkaroundToolTip;

	public string RestartNotice => ConfigurationResources.RestartNotice;
}
