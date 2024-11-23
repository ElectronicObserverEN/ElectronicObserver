using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Settings;

public class ConfigurationTranslationViewModel : TranslationBaseViewModel
{
	public string Title => ConfigRes.Settings;

	public string Communication => ConfigRes.Communication;
	public string UI => "UI";
	public string Log => ConfigRes.Log;
	public string Behavior => ConfigurationResources.TabPage4;
	public string Debug => ConfigRes.Debug;
	public string Window => ConfigRes.Window;
	public string SubWindow => ConfigurationResources.Window;
	public string Notification => ConfigRes.Notification;
	public string BGM => "BGM";
	public string Data => ConfigRes.Data;

	public string Log_PlayTime => ConfigurationResources.Log_PlayTime;

	public string OK => "OK";
	public string Cancel => ConfigRes.Cancel;
}
