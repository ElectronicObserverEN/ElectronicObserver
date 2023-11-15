using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Data;
using ElectronicObserver.Window.Wpf.ShipTrainingPlanner;
using ElectronicObserverTypes;

namespace ElectronicObserver.Notifier;

public class NotifierTrainingPlan : NotifierBase
{
	private ShipTrainingPlanViewerViewModel PlanManager { get; }

	public NotifierTrainingPlan(Utility.Configuration.ConfigurationData.ConfigNotifierBase config)
		: base(config)
	{
		DialogData.Title = NotifierRes.RemodelTitle;

		PlanManager = Ioc.Default.GetRequiredService<ShipTrainingPlanViewerViewModel>();

		Initialize();
	}

	private void Initialize()
	{
		PlanManager.OnPlanCompleted += Notify;
	}

	private void Notify(ShipTrainingPlanViewModel plan)
	{
		DialogData.Message = "TODO";

		base.Notify();
	}
}
