using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Settings.Debugging;

public class ConfigurationDebugTranslationViewModel : TranslationBaseViewModel
{
	public string Debug_AlertOnError => ConfigRes.AlertOnError;
	
	public string Debug_LoadAPIListOnLoad => ConfigurationResources.Debug_LoadAPIListOnLoad;
	public string Debug_LoadAPIListOnLoadToolTip => ConfigurationResources.Debug_LoadAPIListOnLoadToolTip;
	
	public string Debug_EnableDebugMenu => ConfigurationResources.Debug_EnableDebugMenu;
	public string Debug_EnableDebugMenuToolTip => ConfigurationResources.Debug_EnableDebugMenuToolTip;
}
