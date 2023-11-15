using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Window.Wpf.ShipTrainingPlanner;

namespace ElectronicObserver.Notifier;

public class NotifierTrainingPlan : NotifierBase
{
	private ShipTrainingPlanViewerViewModel PlanManager { get; }

	public NotifierTrainingPlan(Utility.Configuration.ConfigurationData.ConfigNotifierBase config)
		: base(config)
	{
		PlanManager = Ioc.Default.GetRequiredService<ShipTrainingPlanViewerViewModel>();

		DialogData.Title = PlanManager.ShipTrainingPlanner.PlanCompleted;

		Initialize();
	}

	private void Initialize()
	{
		PlanManager.OnPlanCompleted += Notify;
	}

	private void Notify(ShipTrainingPlanViewModel plan)
	{
		DialogData.Message = string.Format(PlanManager.ShipTrainingPlanner.PlanCompletedNotification, plan.Ship.MasterShip.NameEN);

		base.Notify();
	}
}
