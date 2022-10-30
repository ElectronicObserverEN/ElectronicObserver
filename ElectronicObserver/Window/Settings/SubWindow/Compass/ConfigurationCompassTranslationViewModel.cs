using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Dialog;

namespace ElectronicObserver.Window.Settings.SubWindow.Compass;

public class ConfigurationCompassTranslationViewModel : TranslationBaseViewModel
{
	public string NameWidth => Properties.Window.Dialog.DialogConfiguration.NameWidth;
	public string FormArsenal_MaxShipNameWidthToolTip => Properties.Window.Dialog.DialogConfiguration.FormArsenal_MaxShipNameWidthToolTip;
	public string FormFleet_IsScrollable => Properties.Window.Dialog.DialogConfiguration.FormFleet_IsScrollable;
	public string FormFleet_IsScrollableToolTip => Properties.Window.Dialog.DialogConfiguration.FormFleet_IsScrollableToolTip;
	public string CandidateDisplayCount => ConfigRes.CandidateDisplayCount;
}
