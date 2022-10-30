using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Dialog;

namespace ElectronicObserver.Window.Settings.SubWindow.Headquarters;

public class ConfigurationHeadquartersTranslationViewModel : TranslationBaseViewModel
{
	public string BlinkAtMaximum => ConfigRes.BlinkAtMaximum;

	public string Show => Properties.Window.Dialog.DialogConfiguration.Show;
	public string OtherItem => Properties.Window.Dialog.DialogConfiguration.OtherItem;
}
