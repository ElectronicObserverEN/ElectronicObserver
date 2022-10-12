using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Dialog;

namespace ElectronicObserver.Window.Settings;

public class ConfigurationTranslationViewModel : TranslationBaseViewModel
{
	public string Title => ConfigRes.Settings;

	public string Communication => ConfigRes.Communication;
	public string UI => "UI";
	public string Log => ConfigRes.Log;
	public string Behavior => Properties.Window.Dialog.DialogConfiguration.TabPage4;
	public string Debug => ConfigRes.Debug;
	public string Window => ConfigRes.Window;
	public string SubWindow => Properties.Window.Dialog.DialogConfiguration.Window;
	public string Notification => ConfigRes.Notification;
	public string BGM => "BGM";

	public string Log_PlayTime => Properties.Window.Dialog.DialogConfiguration.Log_PlayTime;

	public string OK => "OK";
	public string Cancel => ConfigRes.Cancel;
}
