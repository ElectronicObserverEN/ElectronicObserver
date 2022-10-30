using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Dialog;

namespace ElectronicObserver.Window.Settings.SubWindow.Group;

public class ConfigurationGroupTranslationViewModel : TranslationBaseViewModel
{
	public string AutoUpdate => ConfigRes.AutoUpdate;
	public string AutoUpdateHint => ConfigRes.AutoUpdateHint;

	public string FormShipGroup_ShowStatusBar => Properties.Window.Dialog.DialogConfiguration.FormShipGroup_ShowStatusBar;
	public string ShowStatusbarHint => ConfigRes.ShowStatusbarHint;

	public string ShipNameSortMethod => ConfigRes.ShipNameSortMethod;
}
