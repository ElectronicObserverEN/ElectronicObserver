using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Window.Control.ShipFilter;

public class ShipFilterTranslationViewModel : ObservableObject
{
	public string ASW => GeneralRes.ASW;
	public string Luck => GeneralRes.Luck;
	public string Daihatsu => ShipFilter.Daihatsu;
	public string Tank => ShipFilter.Tank;
	public string Fcf => ShipFilter.Fcf;
	public string ShipTypeToggle => ShipFilter.ShipTypeToggle;
	public string Expansion => GeneralRes.Expansion;
	public string NameFilter => GeneralRes.ShipName;
}
