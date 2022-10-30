using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Dialog;

namespace ElectronicObserver.Window.Settings.SubWindow.Dock;

public class ConfigurationDockTranslationViewModel : TranslationBaseViewModel
{
	public string FormDock_BlinkAtCompletion => ConfigRes.BlinkAtCompletion;

	public string NameWidth => Properties.Window.Dialog.DialogConfiguration.NameWidth;
	public string FormArsenal_MaxShipNameWidthToolTip => Properties.Window.Dialog.DialogConfiguration.FormArsenal_MaxShipNameWidthToolTip;
}
