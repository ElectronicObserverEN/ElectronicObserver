using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ElectronicObserver.Database;
using ElectronicObserver.Resource;
using ElectronicObserver.ViewModels;

namespace ElectronicObserver.Window.Wpf.ShipTrainingPlanner;

public class ShipTrainingPlanViewerViewModel : AnchorableViewModel
{
	public ObservableCollection<ShipTrainingPlanViewModel> Plans { get; set; } = new();

	private ElectronicObserverContext DatabaseContext { get; } = new();

	public ShipTrainingPlanViewerViewModel() : base("ShipTrainingPlanViewer", "ShipTrainingPlanViewer", ImageSourceIcons.GetIcon(IconContent.ItemActionReport))
	{
	}

	public override void Loaded()
	{
		base.Loaded();

		Plans.Clear();

		List<ShipTrainingPlanModel> models = DatabaseContext.ShipTrainingPlans.ToList();

		foreach (ShipTrainingPlanModel model in models)
		{
			Plans.Add(new(model));
		}
	}
}
