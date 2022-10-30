using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Dialog;

namespace ElectronicObserver.Window.Settings.SubWindow.Json;

public class ConfigurationJsonTranslationViewModel : TranslationBaseViewModel
{
	public string FormJson_AutoUpdate => Properties.Window.Dialog.DialogConfiguration.FormJson_AutoUpdate;
	public string FormJson_AutoUpdateToolTip => Properties.Window.Dialog.DialogConfiguration.FormJson_AutoUpdateToolTip;
	
	public string FormJson_UpdatesTree => Properties.Window.Dialog.DialogConfiguration.FormJson_UpdatesTree;
	public string FormJson_UpdatesTreeToolTip => Properties.Window.Dialog.DialogConfiguration.FormJson_UpdatesTreeToolTip;
	
	public string UpdateFilter => Properties.Window.Dialog.DialogConfiguration.UpdateFilter;
	public string FormJson_AutoUpdateFilterToolTip => Properties.Window.Dialog.DialogConfiguration.FormJson_AutoUpdateFilterToolTip;

	public string HiddenJSON => ConfigRes.HiddenJSON;
	public string AutoUpdateCouldIncreaseLoad => Properties.Window.Dialog.DialogConfiguration.AutoUpdateCouldIncreaseLoad;
}
