using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Data;
using ElectronicObserver.Data.Translation;
using ElectronicObserver.Services;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.CostCalculation;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.EquipmentAssignment;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.Helpers;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.UpgradeTree;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;
using ElectronicObserverTypes.Mocks;
using ElectronicObserverTypes.Serialization.EquipmentUpgrade;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;

public partial class EquipmentUpgradePlanItemViewModel : WindowViewModelBase, IEquipmentPlanItemViewModel
{
	public EquipmentUpgradeData EquipmentUpgradeData { get; set; }

	public int? EquipmentId { get; set; }

	public EquipmentId EquipmentMasterDataId { get; set; }

	public IEquipmentData? Equipment => EquipmentId switch
	{
		int id => KCDatabase.Instance.Equipments.ContainsKey(id) switch
		{
			true => KCDatabase.Instance.Equipments[id]!,
			_ => KCDatabase.Instance.MasterEquipments.ContainsKey((int)EquipmentMasterDataId) switch
			{
				true => new EquipmentDataMock(KCDatabase.Instance.MasterEquipments[(int)EquipmentMasterDataId]),
				_ => null,
			}
		},
		_ => KCDatabase.Instance.MasterEquipments.ContainsKey((int)EquipmentMasterDataId) switch
		{
			true => new EquipmentDataMock(KCDatabase.Instance.MasterEquipments[(int)EquipmentMasterDataId]),
			_ => null,
		}
	};

	public UpgradeLevel DesiredUpgradeLevel { get; set; }

	public string EquipmentName { get; set; } = "";
	public string CurrentLevelDisplay { get; set; } = "";

	public List<UpgradeLevel> PossibleUpgradeLevels { get; set; } =
		Enum.GetValues<UpgradeLevel>()
			.Where(e => e != UpgradeLevel.Zero)
			.ToList();

	public List<SliderUpgradeLevel> PossibleSliderLevels { get; set; } =
		Enum.GetValues<SliderUpgradeLevel>()
			.ToList();

	public bool Finished { get; set; }

	public int Priority { get; set; }

	public SliderUpgradeLevel SliderLevel { get; set; }

	public IShipDataMaster? SelectedHelper { get; set; }

	public List<EquipmentUpgradeHelpersViewModel> HelperViewModels { get; private set; } = new();

	public EquipmentUpgradeDaysViewModel HelperViewModelCompact { get; private set; } = new(new());

	public Dictionary<DayOfWeek, List<EquipmentUpgradeHelperViewModel>> HelpersPerDay => HelperViewModelCompact
		.Days
		.ToDictionary(helpers => helpers.DayValue, helpers => helpers.Helpers);

	public List<EquipmentUpgradeHelperViewModel> HelpersForCurrentDay => HelpersPerDay[TimeChangeService.CurrentDayOfWeekJST];

	public EquipmentUpgradePlanCostViewModel Cost { get; private set; } = new(new());
	public EquipmentUpgradePlanCostViewModel NextUpgradeCost { get; private set; } = new(new());

	public List<IShipDataMaster> PossibleHelpers => EquipmentUpgradeData.UpgradeList
		.Where(data => data.EquipmentId == (int?)Equipment?.EquipmentId)
		.SelectMany(data => data.Improvement)
		.Where(upgrade => ShouldBeConvertedInto is null || (upgrade.ConversionData is not null && (int)ShouldBeConvertedInto == upgrade.ConversionData.IdEquipmentAfter))
		.SelectMany(improvement => improvement.Helpers)
		.SelectMany(helpers => helpers.ShipIds)
		.Distinct()
		.Select(id => KCDatabase.Instance.MasterShips[id])
		.ToList();

	public string EquipmentAfterConversionDisplay { get; set; } = "";
	public Visibility EquipmentAfterConversionVisible => string.IsNullOrEmpty(EquipmentAfterConversionDisplay) ? Visibility.Collapsed : Visibility.Visible;

	private EquipmentPickerService EquipmentPicker { get; }
	private TimeChangeService TimeChangeService { get; }
	public EquipmentUpgradePlanItemModel Plan { get; }

	public bool HasHelperForToday => HelpersForCurrentDay.Any(helpers => helpers.PlayerHasAtleastOne);

	public bool HasHelperForAtleastOneDayOfWeek => HelpersPerDay
		.SelectMany(day => day.Value)
		.Any(helper => helper.PlayerHasAtleastOne);

	public EquipmentUpgradePlannerTranslationViewModel EquipmentUpgradePlanItem { get; }

	public EquipmentUpgradePlanItemModel? Parent { get; set; }
	public EquipmentId? ShouldBeConvertedInto { get; set; }

	public bool AllowToChangeDesiredUpgradeLevel => Parent is null;
	public bool AllowToChangeEquipment => Parent is null;

	public EquipmentUpgradePlanManager EquipmentUpgradePlanManager { get; }

	public EquipmentUpgradePlanItemViewModel(EquipmentUpgradePlanItemModel plan)
	{
		EquipmentUpgradeData = KCDatabase.Instance.Translation.EquipmentUpgrade;
		Plan = plan;

		EquipmentPicker = Ioc.Default.GetRequiredService<EquipmentPickerService>();
		TimeChangeService = Ioc.Default.GetRequiredService<TimeChangeService>();
		EquipmentUpgradePlanItem = Ioc.Default.GetRequiredService<EquipmentUpgradePlannerTranslationViewModel>();
		EquipmentUpgradePlanManager = Ioc.Default.GetRequiredService<EquipmentUpgradePlanManager>();

		LoadModel();

		PropertyChanged += EquipmentUpgradePlanItemViewModel_PropertyChanged;

		TimeChangeService.DayChanged += UpdateHasHelperProperties;
	}

	private void LoadModel()
	{
		DesiredUpgradeLevel = Plan.DesiredUpgradeLevel;
		Finished = Plan.Finished;
		SliderLevel = Plan.SliderLevel;
		SelectedHelper = KCDatabase.Instance.MasterShips[(int)Plan.SelectedHelper];
		Priority = Plan.Priority;
		EquipmentId = Plan.EquipmentMasterId;
		EquipmentMasterDataId = Plan.EquipmentId;
		Parent = Plan.Parent;
		ShouldBeConvertedInto = Plan.ShouldBeConvertedInto;

		Update();
		UpdateHelperDisplay();
	}

	private void EquipmentUpgradePlanItemViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName is nameof(EquipmentId) or nameof(EquipmentMasterDataId) or nameof(SelectedHelper) or nameof(SliderLevel) or nameof(DesiredUpgradeLevel)) Update();
		if (e.PropertyName is nameof(Equipment)) UpdateHelperDisplay();

		Save();
	}

	public void UpdateCosts()
	{
		if (Equipment is null) return;

		Cost.UnsubscribeFromApis();
		Cost = new(Equipment.CalculateUpgradeCost(EquipmentUpgradeData.UpgradeList, SelectedHelper, DesiredUpgradeLevel, SliderLevel));

		NextUpgradeCost.UnsubscribeFromApis();
		NextUpgradeCost = new(Equipment.CalculateNextUpgradeCost(EquipmentUpgradeData.UpgradeList, SelectedHelper, SliderLevel));
	}

	public void UpdateHelperDisplay()
	{
		HelperViewModels.ForEach(helper => helper.UnsubscribeFromApis());
		HelperViewModels = Equipment switch
		{
			IEquipmentData equipment => EquipmentUpgradeData.UpgradeList
				.Where(data => data.EquipmentId == equipment.EquipmentID)
				.SelectMany(data => data.Improvement)
				.SelectMany(data => data.Helpers)
				.Select(helpers => new EquipmentUpgradeHelpersViewModel(helpers))
				.ToList(),
			_ => new()
		};

		HelperViewModelCompact.UnsubscribeFromApis();
		HelperViewModelCompact = Equipment switch
		{
			IEquipmentData equipment => new EquipmentUpgradeDaysViewModel(EquipmentUpgradeData.UpgradeList
				.Where(data => data.EquipmentId == equipment.EquipmentID)
				.SelectMany(data => data.Improvement)
				.SelectMany(data => data.Helpers)
				.ToList()),
			_ => new(new())
		};

		foreach (EquipmentUpgradeHelperViewModel helpers in HelpersPerDay.SelectMany(day => day.Value))
		{
			helpers.PropertyChanged += (_, args) =>
			{
				if (args.PropertyName is not nameof(EquipmentUpgradeHelperViewModel.PlayerHasAtleastOne)) return;

				UpdateHasHelperProperties();
			};
		}

		UpdateHasHelperProperties();
	}

	public void UpdateHasHelperProperties()
	{
		OnPropertyChanged(nameof(HasHelperForToday));
		OnPropertyChanged(nameof(HasHelperForAtleastOneDayOfWeek));
	}

	public void Update()
	{
		if (Equipment != null) EquipmentMasterDataId = Equipment.MasterEquipment.EquipmentId;

		// Update the equipment display
		if (Equipment is null)
		{
			EquipmentName = "";
			CurrentLevelDisplay = "";
			return;
		}

		EquipmentName = Equipment.MasterEquipment.NameEN;

		if (Equipment.MasterID > 0)
			CurrentLevelDisplay = Equipment.UpgradeLevel.Display();
		else
			CurrentLevelDisplay = EquipmentUpgradePlannerResources.Unassigned;

		UpdateCosts();
		UpdatePostConversionEquipmentDisplay();
	}

	public void UpdatePostConversionEquipmentDisplay()
	{
		if (DesiredUpgradeLevel != UpgradeLevel.Conversion)
		{
			EquipmentAfterConversionDisplay = "";
			return;
		}

		EquipmentUpgradeConversionModel? equipmentAfter = EquipmentUpgradeData.UpgradeList
			.Where(data => data.EquipmentId == (int?)Equipment?.EquipmentId)
			.SelectMany(data => data.Improvement)
			.Where(improvement => SelectedHelper is null || improvement.Helpers.Where(helper => helper.ShipIds.Contains(SelectedHelper.ShipID)).Any())
			.FirstOrDefault()?.ConversionData;

		EquipmentAfterConversionDisplay = equipmentAfter switch
		{
			EquipmentUpgradeConversionModel data => 
				$"{KCDatabase.Instance.MasterEquipments[data.IdEquipmentAfter].NameEN}{(equipmentAfter.EquipmentLevelAfter > 0 ? $" +{equipmentAfter.EquipmentLevelAfter}" : "")}",
			_ => "",
		};
	}

	public void Save()
	{
		Plan.EquipmentId = EquipmentMasterDataId;
		Plan.EquipmentMasterId = Equipment?.MasterID > 0 ? Equipment.MasterID : null;
		Plan.DesiredUpgradeLevel = DesiredUpgradeLevel;
		Plan.Finished = Finished;
		Plan.Priority = Priority;
		Plan.SliderLevel = SliderLevel;
		Plan.SelectedHelper = SelectedHelper?.ShipId ?? ShipId.Unknown;
		Plan.Parent = Parent;
		Plan.ShouldBeConvertedInto = ShouldBeConvertedInto;
	}

	public bool OpenPlanDialog()
	{
		EquipmentUpgradePlanItemViewModel editVm = new(Plan);
		EquipmentUpgradePlanItemWindow editView = new(editVm);

		if (editView.ShowDialog() is not true)
		{
			return false;
		}

		DesiredUpgradeLevel = editVm.DesiredUpgradeLevel;
		Finished = editVm.Finished;
		SliderLevel = editVm.SliderLevel;
		SelectedHelper = editVm.SelectedHelper;
		Priority = editVm.Priority;
		EquipmentMasterDataId = editVm.EquipmentMasterDataId;
		EquipmentId = editVm.EquipmentId;
		Parent = editVm.Parent;
		ShouldBeConvertedInto = editVm.ShouldBeConvertedInto;

		editVm.UnsubscribeFromApis();

		Save();

		return true;
	}

	public void UnsubscribeFromApis()
	{
		HelperViewModels.ForEach(viewModel => viewModel.UnsubscribeFromApis());
		HelperViewModelCompact.UnsubscribeFromApis();

		Cost.UnsubscribeFromApis();
		NextUpgradeCost.UnsubscribeFromApis();
	}

	public List<IEquipmentPlanItemViewModel> GetPlanChildren()
	{
		List<IEquipmentPlanItemViewModel> children = new();

		// Equipments needed
		Dictionary<EquipmentId, int> equipmentsRequired = Cost.RequiredEquipments
			.GroupBy(eq => eq.Equipment.EquipmentId)
			.ToDictionary(group => group.Key, group => group.Sum(eq => eq.Required));

		// Equipment itself (if plan is unassigned)
		if (EquipmentId is null)
		{
			if (!equipmentsRequired.TryAdd(EquipmentMasterDataId, 1))
			{
				equipmentsRequired[EquipmentMasterDataId]++;
			}
		}

		// The order by is here so that the first child is the equipment itself if needed
		foreach (KeyValuePair<EquipmentId, int> equipmentRequired in equipmentsRequired.OrderBy(eq => eq.Key == EquipmentMasterDataId ? 0 : eq.Key))
		{
			children.AddRange(GetUpgradeChildrenForRequiredEquipments(equipmentRequired.Key, equipmentRequired.Value));
		}

		// TODO Consumable needed

		return children;
	}

	private List<IEquipmentPlanItemViewModel> GetUpgradeChildrenForRequiredEquipments(EquipmentId id, int required)
	{
		// if fodder = upgraded equipment, return a craft plan to avoid infinite loops
		if (EquipmentMasterDataId == id && Plan.ShouldBeConvertedInto == id)
		{
			return new List<IEquipmentPlanItemViewModel>
			{
				new EquipmentCraftPlanItemViewModel(id, this)
				{
					RequiredCount = required
				}
			};
		}

		// Look for assigned plans
		List<IEquipmentPlanItemViewModel> plans =
			EquipmentUpgradePlanManager.PlannedUpgrades
				.Where(plan => plan.Parent == Plan && plan.ShouldBeConvertedInto == id)
				.Cast<IEquipmentPlanItemViewModel>()
				.ToList();

		// Look for assigned equipments
		plans.AddRange(EquipmentUpgradePlanManager
			.GetAssignments(Plan)
			.Where(plan => plan.EquipmentMasterDataId == id)
			.Select(model => new EquipmentAssignmentItemViewModel(model))
		);

		EquipmentUpgradeDataModel? planToMakeRequiredEquipment = GetPlanToMakeEquipmentFromUpgrade(id);

		if (planToMakeRequiredEquipment is not null)
		{
			List<EquipmentUpgradePlanItemViewModel> newPlans = new();

			while (newPlans.Count + plans.Count < required)
			{
				EquipmentUpgradePlanItemViewModel newChild = EquipmentUpgradePlanManager.MakePlanViewModel(new());
				newChild.EquipmentMasterDataId = (EquipmentId)planToMakeRequiredEquipment.EquipmentId;
				newChild.DesiredUpgradeLevel = UpgradeLevel.Conversion;
				newChild.Parent = Plan;
				newChild.ShouldBeConvertedInto = id;
				newPlans.Add(newChild);
			}

			if (newPlans.Count > 0)
			{
				plans.Add(new EquipmentConversionPlanItemViewModel(this, newPlans));
			}
		}
		else if (plans.Count < required)
		{
			plans.Add(new EquipmentCraftPlanItemViewModel(id, this)
			{
				RequiredCount = required - plans.Count
			});
		}

		return plans;
	}

	private EquipmentUpgradeDataModel? GetPlanToMakeEquipmentFromUpgrade(EquipmentId equipment)
	{
		EquipmentUpgradeDataModel? upgradePlan = EquipmentUpgradeData.UpgradeList
			.Find(equipmentData => equipmentData.ConvertTo
				.Exists(equipmentAfterConvertion =>
					equipmentAfterConvertion.IdEquipmentAfter == (int)equipment));

		return upgradePlan;
	}

	[RelayCommand]
	public void OpenEquipmentPicker()
	{
		IEquipmentData? newEquip = EquipmentPicker.OpenEquipmentPicker();
		if (newEquip != null)
		{
			EquipmentId = newEquip.MasterID;
		}
	}
}
