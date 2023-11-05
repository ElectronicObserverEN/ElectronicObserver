using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Settings.SubWindow.Json;

public class ConfigurationJsonTranslationViewModel : TranslationBaseViewModel
{
	public string FormJson_AutoUpdate => ConfigurationResources.FormJson_AutoUpdate;
	public string FormJson_AutoUpdateToolTip => ConfigurationResources.FormJson_AutoUpdateToolTip;
	
	public string FormJson_UpdatesTree => ConfigurationResources.FormJson_UpdatesTree;
	public string FormJson_UpdatesTreeToolTip => ConfigurationResources.FormJson_UpdatesTreeToolTip;
	
	public string UpdateFilter => ConfigurationResources.UpdateFilter;
	public string FormJson_AutoUpdateFilterToolTip => ConfigurationResources.FormJson_AutoUpdateFilterToolTip;

	public string HiddenJSON => ConfigRes.HiddenJSON;
	public string AutoUpdateCouldIncreaseLoad => ConfigurationResources.AutoUpdateCouldIncreaseLoad;
}
