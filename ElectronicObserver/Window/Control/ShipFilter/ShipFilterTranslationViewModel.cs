using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Control.ShipFilter;

public class ShipFilterTranslationViewModel : TranslationBaseViewModel
{
	public string ASW => GeneralRes.ASW;
	public string Luck => GeneralRes.Luck;
	public string Daihatsu => ShipFilter.Daihatsu;
	public string Tank => ShipFilter.Tank;
	public string Fcf => ShipFilter.Fcf;
	public string Bulge => ShipFilter.Bulge;
	public string ShipTypeToggle => ShipFilter.ShipTypeToggle;
	public string Expansion => GeneralRes.Expansion;
	public string NameFilter => GeneralRes.ShipName;
}
