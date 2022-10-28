using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Dialog;

namespace ElectronicObserver.Window.Settings.BGM;

public class ConfigurationBGMTranslationViewModel : TranslationBaseViewModel
{
	public string BGMPlayer_SyncBrowserMute => Properties.Window.Dialog.DialogConfiguration.BGMPlayer_SyncBrowserMute;
	public string BGMPlayer_SyncBrowserMuteToolTip => Properties.Window.Dialog.DialogConfiguration.BGMPlayer_SyncBrowserMuteToolTip;
	public string BGMPlayer_SetVolumeAll => ConfigRes.BGMPlayer_SetVolumeAll;
	public string BGMPlayer_Enabled => ConfigRes.BGMPlayer_Enabled;
	public string BGMPlayer_ColumnContent => ConfigRes.Scene;
	public string BGMPlayer_ColumnPath => ConfigRes.FileName;
	public string BGMPlayer_ColumnSetting => ConfigRes.Settings;
	public string Edit => Properties.Window.Dialog.DialogConfiguration.Edit;
}
