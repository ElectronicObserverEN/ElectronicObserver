﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Behaviors.PersistentColumns;
using ElectronicObserver.Common.Datagrid;
using ElectronicObserver.Data;
using ElectronicObserver.Database;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.ViewModels;
using ElectronicObserver.Window.Dialog.ShipDataPicker;
using ElectronicObserverTypes;
using Jot;

namespace ElectronicObserver.Window.Wpf.ShipTrainingPlanner;

public partial class ShipTrainingPlanViewerViewModel : AnchorableViewModel
{
	private ShipDataPickerViewModel PickerViewModel { get; } = new();

	public ObservableCollection<ShipTrainingPlanViewModel> Plans { get; } = new();
	public ICollectionView PlansFiltered { get; set; }

	private ElectronicObserverContext DatabaseContext { get; } = new();

	public DataGridViewModel DataGridViewModel { get; set; } = new();

	public bool DisplayFinished { get; set; } = false;

	public ShipTrainingPlannerTranslationViewModel ShipTrainingPlanner { get; }

	public ShipTrainingPlanViewModel? SelectedPlan { get; set; }

	private Tracker Tracker { get; }

	public ShipTrainingPlanViewerViewModel() : base("ShipTrainingPlanViewer", "ShipTrainingPlanViewer", ImageSourceIcons.GetIcon(IconContent.ItemActionReport))
	{
		Tracker = Ioc.Default.GetService<Tracker>()!;
		ShipTrainingPlanner = Ioc.Default.GetRequiredService<ShipTrainingPlannerTranslationViewModel>();

		PlansFiltered = CollectionViewSource.GetDefaultView(Plans);

		Title = ShipTrainingPlanner.ViewTitle;
		ShipTrainingPlanner.PropertyChanged += (_, _) => Title = ShipTrainingPlanner.ViewTitle;
		PropertyChanged += ShipTrainingPlanViewerViewModel_PropertyChanged;

		SubscribeToApi();
		StartJotTracking();
	}

	private void ShipTrainingPlanViewerViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName is nameof(DisplayFinished)) UpdatePlanList();
	}

	private void SubscribeToApi()
	{
		APIObserver o = APIObserver.Instance;

		o.ApiPort_Port.ResponseReceived += Initialize;
		o.ApiReqKousyou_DestroyShip.RequestReceived += OnShipScrap;
	}

	private void OnShipScrap(string apiname, dynamic data)
	{
		string idList = data["api_ship_id"].ToString();
		IEnumerable<int> shipId = idList.Split(',').Select(int.Parse);

		IEnumerable<ShipTrainingPlanViewModel> plansFound = Plans.Where(plan => shipId.Contains(plan.Model.ShipId));

		foreach (ShipTrainingPlanViewModel planFound in plansFound)
		{
			RemovePlan(planFound);
		}
	}

	private void Initialize(string apiname, dynamic data)
	{
		List<ShipTrainingPlanModel> models = DatabaseContext.ShipTrainingPlans.ToList();

		foreach (ShipTrainingPlanModel model in models)
		{
			ShipTrainingPlanViewModel viewModel = new(model);
			viewModel.PropertyChanged += OnPlanViewModelPropertyChanged;
			Plans.Add(viewModel);
		}

		APIObserver.Instance.ApiPort_Port.ResponseReceived -= Initialize;
		Plans.CollectionChanged += (_, _) => UpdatePlanList();

		UpdatePlanList();
	}

	private void OnPlanViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName is nameof(ShipTrainingPlanViewModel.PlanFinished)) UpdatePlanList();
	}

	private void UpdatePlanList()
	{
		CollectionViewSource.GetDefaultView(Plans.Where(plan => DisplayFinished || !plan.PlanFinished));
	}

	private ShipTrainingPlanViewModel NewPlan(IShipData ship)
	{
		ShipTrainingPlanModel newPlan = new()
		{
			ShipId = ship.ID,
			TargetLevel = ship.Level,
			TargetLuck = ship.LuckTotal
		};

		ShipTrainingPlanViewModel newPlanViewModel = new(newPlan);

		// init some values like level or target remodel
		IShipDataMaster? lastRemodel = newPlanViewModel.PossibleRemodels.LastOrDefault()?.Ship;

		if (lastRemodel is not null)
		{
			newPlan.TargetRemodel = lastRemodel.ShipId;

			if (lastRemodel.RemodelBeforeShip is IShipDataMaster shipBefore)
			{
				newPlan.TargetLevel = shipBefore.RemodelAfterLevel;
			}

			newPlanViewModel.UpdateFromModel();
		}

		return newPlanViewModel;
	}

	[RelayCommand]
	private void RemovePlan(ShipTrainingPlanViewModel plan)
	{
		Plans.Remove(plan);
		DatabaseContext.ShipTrainingPlans.Remove(plan.Model);
		DatabaseContext.SaveChanges();
	}

	[RelayCommand]
	private void RemoveSelectedPlan()
	{
		if (SelectedPlan is not null)
		{
			RemovePlan(SelectedPlan);
		}
	}

	[RelayCommand]
	private void OpenEditPopup(ShipTrainingPlanViewModel plan)
	{
		ShipTrainingPlanViewModel viewModel = new(plan.Model);
		ShipTrainingPlanView view = new(viewModel);

		if (view.ShowDialog() is true)
		{
			viewModel.Save();
			DatabaseContext.SaveChanges();
			plan.UpdateFromModel();
		}
	}

	[RelayCommand]
	private void OpenEditPopupSelectedPlan()
	{
		if (SelectedPlan is not null)
		{
			OpenEditPopup(SelectedPlan);
		}
	}

	[RelayCommand]
	private void OpenShipPickerToAddNewPlan()
	{
		List<int> alreadyAddedIds = Plans.Select(s => s.Ship.ID).ToList();

		PickerViewModel.LoadWithShips(KCDatabase.Instance.Ships
			.Values
			.Cast<IShipData>()
			.Where(s => !alreadyAddedIds.Contains(s.ID)));

		ShipDataPickerView pickerView = new(PickerViewModel);

		pickerView.ShowDialog();

		if (PickerViewModel.PickedShip is IShipData ship)
		{
			AddNewPlan(ship);
		}
	}

	[RelayCommand]
	private void RemoveAllFinishedPlans()
	{
		List<ShipTrainingPlanViewModel> plansToRemove = Plans.Where(p => p.PlanFinished).ToList();

		foreach (ShipTrainingPlanViewModel plan in plansToRemove)
		{
			RemovePlan(plan);
		}
	}

	public void AddNewPlan(IShipData ship)
	{
		ShipTrainingPlanViewModel newPlan = NewPlan(ship);
		ShipTrainingPlanView editForm = new(newPlan);

		if (editForm.ShowDialog() is true)
		{
			newPlan.Save();
			newPlan.UpdateFromModel();
			Plans.Add(newPlan);
			DatabaseContext.ShipTrainingPlans.Add(newPlan.Model);
			newPlan.PropertyChanged += OnPlanViewModelPropertyChanged;
			DatabaseContext.SaveChanges();
		}
	}

	private void StartJotTracking()
	{
		Tracker.Track(this);
	}
}
