using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Settings.SubWindow.Arsenal;

public class ConfigurationArsenalTranslationViewModel : TranslationBaseViewModel
{
	public string FormArsenal_ShowShipName => ConfigurationResources.FormArsenal_ShowShipName;
	
	public string BlinkAtCompletion => ConfigRes.BlinkAtCompletion;

	public string NameWidth => ConfigurationResources.NameWidth;
	public string FormArsenal_MaxShipNameWidthToolTip => ConfigurationResources.FormArsenal_MaxShipNameWidthToolTip;
}
