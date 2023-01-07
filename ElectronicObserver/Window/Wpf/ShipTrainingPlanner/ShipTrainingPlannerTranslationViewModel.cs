using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Wpf.ShipTrainingPlanner;
public class ShipTrainingPlannerTranslationViewModel : TranslationBaseViewModel
{
	public string ViewTitle => ShipTrainingPlanner.ViewerTitle;
	public string AddShip => ShipTrainingPlanner.AddShip;
	public string RemovePlan => ShipTrainingPlanner.RemovePlan;
}
