using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Settings.BGM;

public class ConfigurationBGMTranslationViewModel : TranslationBaseViewModel
{
	public string BGMPlayer_SyncBrowserMute => ConfigurationResources.BGMPlayer_SyncBrowserMute;
	public string BGMPlayer_SyncBrowserMuteToolTip => ConfigurationResources.BGMPlayer_SyncBrowserMuteToolTip;
	public string BGMPlayer_SetVolumeAll => ConfigRes.BGMPlayer_SetVolumeAll;
	public string BGMPlayer_Enabled => ConfigRes.BGMPlayer_Enabled;
	public string BGMPlayer_ColumnContent => ConfigRes.Scene;
	public string BGMPlayer_ColumnPath => ConfigRes.FileName;
	public string BGMPlayer_ColumnSetting => ConfigRes.Settings;
	public string Edit => ConfigurationResources.Edit;
}
