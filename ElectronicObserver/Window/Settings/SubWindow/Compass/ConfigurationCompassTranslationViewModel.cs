using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Settings.SubWindow.Compass;

public class ConfigurationCompassTranslationViewModel : TranslationBaseViewModel
{
	public string NameWidth => ConfigurationResources.NameWidth;
	public string FormArsenal_MaxShipNameWidthToolTip => ConfigurationResources.FormArsenal_MaxShipNameWidthToolTip;
	public string FormFleet_IsScrollable => ConfigurationResources.FormFleet_IsScrollable;
	public string FormFleet_IsScrollableToolTip => ConfigurationResources.FormFleet_IsScrollableToolTip;
	public string CandidateDisplayCount => ConfigRes.CandidateDisplayCount;
	public string Compass_DisplayAllPossibleEnemyComps => ConfigurationResources.Compass_DisplayAllPossibleEnemyComps;
	public string Compass_DisplayAllPossibleEnemyCompsToolTip => ConfigurationResources.Compass_DisplayAllPossibleEnemyCompsTooltip;
}
