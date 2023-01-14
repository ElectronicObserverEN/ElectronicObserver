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
	public string ASW => EncycloRes.ASW;
	public string HP => EncycloRes.HP;
	public string Level => ShipTrainingPlanner.Level;
	public string HPBonus => ShipTrainingPlanner.HPBonus;
	public string ASWBonus => ShipTrainingPlanner.ASWBonus;
	public string Luck => EncycloRes.Luck;
	public string RemodelGoal => ShipTrainingPlanner.RemodelGoal;
	public string EditPlan => ShipTrainingPlanner.EditPlan;
	public string Cancel => GeneralRes.Cancel;
	public string OK => "OK";
	public string NotifyAnyRemodelReady => ShipTrainingPlanner.NotifyAnyRemodelReady;
	public string NotifyAnyRemodelReadyToolTip => ShipTrainingPlanner.NotifyAnyRemodelReadyToolTip;
	public string Exp => ShipTrainingPlanner.Exp;
	public string Finished => ShipTrainingPlanner.Finished;
	public string DisplayFinished => ShipTrainingPlanner.DisplayFinished;
	public string RemoveFinishedPlans => ShipTrainingPlanner.RemoveFinishedPlans;
}
