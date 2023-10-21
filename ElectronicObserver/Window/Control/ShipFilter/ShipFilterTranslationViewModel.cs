using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Control.ShipFilter;

public class ShipFilterTranslationViewModel : TranslationBaseViewModel
{
	public string ASW => GeneralRes.ASW;
	public string Luck => GeneralRes.Luck;
	public string Daihatsu => ShipFilterResources.Daihatsu;
	public string Tank => ShipFilterResources.Tank;
	public string Fcf => ShipFilterResources.Fcf;
	public string Bulge => ShipFilterResources.Bulge;
	public string SeaplaneFighter => ShipFilterResources.SeaplaneFighter;
	public string ShipTypeToggle => ShipFilterResources.ShipTypeToggle;
	public string Expansion => GeneralRes.Expansion;
	public string NameFilter => GeneralRes.ShipName;
}
