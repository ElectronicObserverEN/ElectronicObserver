using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Data;
using ElectronicObserver.Data.Translation;
using ElectronicObserver.Services;
using ElectronicObserver.Utility.Data;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;
using ElectronicObserverTypes.Mocks;
using ElectronicObserverTypes.Serialization.EquipmentUpgrade;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;
public partial class EquipmentUpgradePlanItemViewModel : ObservableObject
{
	public EquipmentUpgradeData EquipmentUpgradeData { get; set; } = new();

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

	public EquipmentUpgradePlanCostViewModel Cost { get; set; } = new(new());

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
	public EquipmentUpgradePlanItemModel Plan { get; }

	public EquipmentUpgradePlanItemViewModel(EquipmentUpgradePlanItemModel plan)
	{
		Plan = plan;

		EquipmentPicker = Ioc.Default.GetService<EquipmentPickerService>()!;

		LoadModel();

		PropertyChanged += EquipmentUpgradePlanItemViewModel_PropertyChanged;
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
	}

	private void EquipmentUpgradePlanItemViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName is nameof(EquipmentId) or nameof(EquipmentMasterDataId) or nameof(SelectedHelper) or nameof(SliderLevel) or nameof(DesiredUpgradeLevel)) Update();

		Save();
	}

	public EquipmentUpgradePlanCostModel CalculateCost()
	{
		if (Equipment is null) return new EquipmentUpgradePlanCostModel();

		return Equipment.CalculateUpgradeCost(EquipmentUpgradeData.UpgradeList, SelectedHelper, DesiredUpgradeLevel, SliderLevel);
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
			CurrentLevelDisplay = EquipmentUpgradePlanner.Unassigned;

		Cost = new(CalculateCost());
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
