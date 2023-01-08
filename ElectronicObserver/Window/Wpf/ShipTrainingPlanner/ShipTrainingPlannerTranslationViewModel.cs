using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Dialog;

namespace ElectronicObserver.Window.Wpf.ShipTrainingPlanner;
public class ShipTrainingPlannerTranslationViewModel : TranslationBaseViewModel
{
	public string ViewTitle => ShipTrainingPlanner.ViewerTitle;
	public string AddShip => ShipTrainingPlanner.AddShip;
	public string RemovePlan => ShipTrainingPlanner.RemovePlan;
	public string ShipName => EncycloRes.ShipName;
	public string ShipType => EncycloRes.ShipType;
	public string Level => ShipTrainingPlanner.Level;
	public string HPBonus => ShipTrainingPlanner.HPBonus;
	public string ASWBonus => ShipTrainingPlanner.ASWBonus;
	public string Luck => EncycloRes.Luck;
	public string Current => ShipTrainingPlanner.Current;
	public string RemodelGoal => ShipTrainingPlanner.RemodelGoal;
}
