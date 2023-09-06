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
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.Helpers;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;
using ElectronicObserverTypes.Mocks;
using ElectronicObserverTypes.Serialization.EquipmentUpgrade;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;
public partial class EquipmentUpgradePlanItemViewModel : UserControlViewModelBase
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

	public List<EquipmentUpgradeHelpersViewModel> HelperViewModels { get; set; } = new();

	public EquipmentUpgradeDaysViewModel HelperViewModelCompact { get; set; } = new(new());

	public Dictionary<DayOfWeek, List<EquipmentUpgradeHelperViewModel>> HelpersPerDay => HelperViewModelCompact
		.Days
		.ToDictionary(helpers => helpers.DayValue, helpers => helpers.Helpers);

	public List<EquipmentUpgradeHelperViewModel> HelpersForCurrentDay => HelpersPerDay[TimeChangeService.CurrentDayOfWeekJST];

	public EquipmentUpgradePlanCostViewModel Cost { get; set; } = new(new());
	public EquipmentUpgradePlanCostViewModel NextUpgradeCost { get; set; } = new(new());

	public List<IShipDataMaster> PossibleHelpers => EquipmentUpgradeData.UpgradeList
		.Where(data => data.EquipmentId == (int?)Equipment?.EquipmentId)
		.SelectMany(data => data.Improvement)
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

	public EquipmentUpgradePlanItemViewModel(EquipmentUpgradePlanItemModel plan)
	{
		EquipmentUpgradeData = KCDatabase.Instance.Translation.EquipmentUpgrade;
		Plan = plan;

		EquipmentPicker = Ioc.Default.GetService<EquipmentPickerService>()!;
		TimeChangeService = Ioc.Default.GetService<TimeChangeService>()!;
		EquipmentUpgradePlanItem = Ioc.Default.GetRequiredService<EquipmentUpgradePlannerTranslationViewModel>();

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

		Cost = new(Equipment.CalculateUpgradeCost(EquipmentUpgradeData.UpgradeList, SelectedHelper, DesiredUpgradeLevel, SliderLevel));
		NextUpgradeCost = new(Equipment.CalculateNextUpgradeCost(EquipmentUpgradeData.UpgradeList, SelectedHelper, SliderLevel));
	}

	public void UpdateHelperDisplay()
	{
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
