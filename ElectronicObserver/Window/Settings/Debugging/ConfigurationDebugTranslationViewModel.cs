using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Dialog;

namespace ElectronicObserver.Window.Settings.Debugging;

public class ConfigurationDebugTranslationViewModel : TranslationBaseViewModel
{
	public string Debug_AlertOnError => ConfigRes.AlertOnError;
	
	public string Debug_LoadAPIListOnLoad => Properties.Window.Dialog.DialogConfiguration.Debug_LoadAPIListOnLoad;
	public string Debug_LoadAPIListOnLoadToolTip => Properties.Window.Dialog.DialogConfiguration.Debug_LoadAPIListOnLoadToolTip;

	public string Debug_ElectronicObserverApiUrl => Properties.Window.Dialog.DialogConfiguration.Debug_ElectronicObserverApiUrl;

	public string Debug_EnableDebugMenu => Properties.Window.Dialog.DialogConfiguration.Debug_EnableDebugMenu;
	public string Debug_EnableDebugMenuToolTip => Properties.Window.Dialog.DialogConfiguration.Debug_EnableDebugMenuToolTip;
}
