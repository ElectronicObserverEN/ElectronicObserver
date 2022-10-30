using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Dialog;

namespace ElectronicObserver.Window.Settings.SubWindow.Arsenal;

public class ConfigurationArsenalTranslationViewModel : TranslationBaseViewModel
{
	public string FormArsenal_ShowShipName => Properties.Window.Dialog.DialogConfiguration.FormArsenal_ShowShipName;
	
	public string BlinkAtCompletion => ConfigRes.BlinkAtCompletion;

	public string NameWidth => Properties.Window.Dialog.DialogConfiguration.NameWidth;
	public string FormArsenal_MaxShipNameWidthToolTip => Properties.Window.Dialog.DialogConfiguration.FormArsenal_MaxShipNameWidthToolTip;
}
