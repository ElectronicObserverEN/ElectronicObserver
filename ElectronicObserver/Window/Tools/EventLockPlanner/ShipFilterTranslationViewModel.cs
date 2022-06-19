using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Window.Tools.EventLockPlanner;

public class ShipFilterTranslationViewModel : ObservableObject
{
	public string ASW => GeneralRes.ASW;
	public string Luck => GeneralRes.Luck;
	public string Daihatsu => EventLockPlanner.Daihatsu;
	public string Tank => EventLockPlanner.Tank;
	public string Fcf => EventLockPlanner.Fcf;
	public string Expansion => GeneralRes.Expansion;
	public string NameFilter => GeneralRes.ShipName;
}
