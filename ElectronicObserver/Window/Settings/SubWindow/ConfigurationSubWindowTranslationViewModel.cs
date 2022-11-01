using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Dialog;

namespace ElectronicObserver.Window.Settings.SubWindow;

public class ConfigurationSubWindowTranslationViewModel : TranslationBaseViewModel
{
	public string Fleet => Properties.Window.Dialog.DialogConfiguration.Fleet;
	public string Arsenal => ConfigRes.Arsenal;
	public string Dock => ConfigRes.Dock;
	public string HQ => Properties.Window.Dialog.DialogConfiguration.HQ;
	public string Compass => GeneralRes.Compass;
	public string Quests => ConfigRes.Quests;
	public string Group => ConfigRes.Group;
	public string Combat => Properties.Window.Dialog.DialogConfiguration.Combat;
	public string Browser => Properties.Window.Dialog.DialogConfiguration.Browser;
	public string AB => Properties.Window.Dialog.DialogConfiguration.AB;
	public string JSON => "JSON";
}
