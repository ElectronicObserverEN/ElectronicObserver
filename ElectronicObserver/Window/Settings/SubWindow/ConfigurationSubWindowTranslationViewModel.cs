using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Settings.SubWindow;

public class ConfigurationSubWindowTranslationViewModel : TranslationBaseViewModel
{
	public string Fleet => ConfigurationResources.Fleet;
	public string Arsenal => ConfigRes.Arsenal;
	public string Dock => ConfigRes.Dock;
	public string HQ => ConfigurationResources.HQ;
	public string Compass => GeneralRes.Compass;
	public string Quests => ConfigRes.Quests;
	public string Group => ConfigRes.Group;
	public string ShipTraining => ConfigRes.ShipTraining;
	public string Combat => ConfigurationResources.Combat;
	public string Browser => ConfigurationResources.Browser;
	public string AB => ConfigurationResources.AB;
	public string JSON => "JSON";
}
