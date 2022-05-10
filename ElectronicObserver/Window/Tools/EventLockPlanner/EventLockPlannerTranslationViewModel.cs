using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Tools.EventLockPlanner;

public class EventLockPlannerTranslationViewModel : TranslationBaseViewModel
{
	public string Title => EventLockPlanner.Title;

	public string AddLock => EventLockPlanner.AddLock;
	public string RemoveLock => EventLockPlanner.RemoveLock;

	public string ShipType => Properties.Window.FormShipGroup.ShipView_ShipType;
	public string Name => Properties.Window.FormShipGroup.ShipView_Name;
	public string Firepower => GeneralRes.Firepower;
	public string NightBattlePower => Properties.Window.FormShipGroup.ShipView_NightBattlePower;
	public string ASW => GeneralRes.ASW;
	public string Luck => GeneralRes.Luck;
	public string Daihatsu => EventLockPlanner.Daihatsu;
	public string Tank => EventLockPlanner.Tank;
}
