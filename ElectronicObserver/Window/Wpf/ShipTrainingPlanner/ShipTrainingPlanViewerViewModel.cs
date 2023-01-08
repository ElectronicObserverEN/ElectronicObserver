using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Behaviors.PersistentColumns;
using ElectronicObserver.Data;
using ElectronicObserver.Database;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.ViewModels;
using ElectronicObserver.Window.Dialog.ShipDataPicker;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;
using Jot;

namespace ElectronicObserver.Window.Wpf.ShipTrainingPlanner;

public partial class ShipTrainingPlanViewerViewModel : AnchorableViewModel
{
	public ObservableCollection<ShipTrainingPlanViewModel> Plans { get; set; } = new();

	public int MaximumLevel => ExpTable.ShipMaximumLevel;

	private ElectronicObserverContext DatabaseContext { get; } = new();

	public List<ColumnProperties> ColumnProperties { get; set; } = new();
	public List<SortDescription> SortDescriptions { get; set; } = new();

	public ShipTrainingPlannerTranslationViewModel ShipTrainingPlanner { get; } = new();

	private Tracker Tracker { get; }

	public ShipTrainingPlanViewerViewModel() : base("ShipTrainingPlanViewer", "ShipTrainingPlanViewer", ImageSourceIcons.GetIcon(IconContent.ItemActionReport))
	{
		Tracker = Ioc.Default.GetService<Tracker>()!;

		Title = ShipTrainingPlanner.ViewTitle;
		ShipTrainingPlanner.PropertyChanged += (_, _) => Title = ShipTrainingPlanner.ViewTitle;

		SubscribeToApi();
		StartJotTracking();
	}

	private void SubscribeToApi()
	{
		APIObserver o = APIObserver.Instance;

		o.ApiPort_Port.ResponseReceived += (_, _) => Update();
	}

	public void Update()
	{
		Plans.Clear();

		List<ShipTrainingPlanModel> models = DatabaseContext.ShipTrainingPlans.ToList();

		foreach (ShipTrainingPlanModel model in models)
		{
			ShipTrainingPlanViewModel viewModel = new(model);
			viewModel.OnSave += () => DatabaseContext.SaveChanges();
			Plans.Add(viewModel);
		}
	}

	private ShipTrainingPlanViewModel NewPlan(IShipData ship)
	{
		ShipTrainingPlanModel newPlan = new()
		{
			ShipId = ship.ID,
			TargetLevel = ship.Level,
			TargetLuck = ship.LuckTotal
		};

		DatabaseContext.ShipTrainingPlans.Add(newPlan);
		DatabaseContext.SaveChanges();

		ShipTrainingPlanViewModel newPlanViewModel = new(newPlan);

		newPlanViewModel.OnSave += () => DatabaseContext.SaveChanges();

		return newPlanViewModel;
	}

	[RelayCommand]
	public void RemovePlan(ShipTrainingPlanViewModel plan)
	{
		Plans.Remove(plan);
		DatabaseContext.ShipTrainingPlans.Remove(plan.Model);
		DatabaseContext.SaveChanges();
	}

	[RelayCommand]
	public void OpenShipPickerToAddNewPlan()
	{
		List<int> alreadyAddedIds = Plans.Select(s => s.Ship.ID).ToList();

		List<IShipData> ships = KCDatabase.Instance.Ships
			.Values
			.Cast<IShipData>()
			.Where(s => !alreadyAddedIds.Contains(s.ID))
			.ToList();

		ShipDataPickerViewModel pickerViewModel = new(ships);
		ShipDataPickerView pickerView = new(pickerViewModel);

		pickerView.ShowDialog();

		if (pickerViewModel.PickedShip is IShipData ship)
		{
			Plans.Add(NewPlan(ship));
		}
	}

	private void StartJotTracking()
	{
		Tracker.Track(this);
	}
}
