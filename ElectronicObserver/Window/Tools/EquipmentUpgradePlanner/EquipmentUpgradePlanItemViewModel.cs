using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Data;
using ElectronicObserver.Services;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;
using ElectronicObserverTypes.Mocks;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;
public partial class EquipmentUpgradePlanItemViewModel : ObservableObject
{
	public int? Id { get; set; }

	[ObservableProperty]
	private IEquipmentData? equipment;

	public UpgradeLevel DesiredUpgradeLevel { get; set; }

	public string EquipmentName { get; set; } = "";
	public string CurrentLevelDisplay { get; set; } = "";

	public List<UpgradeLevel> PossibleUpgradeLevels { get; set; } =
		Enum.GetValues<UpgradeLevel>()
			.Where(e => e != UpgradeLevel.Zero)
			.ToList();

	public bool Finished { get; set; }

	public int Priority { get; set; }

	private readonly EquipmentPickerService EquipmentPicker;
	public EquipmentUpgradePlanItemModel Plan { get; private set; }

	public EquipmentUpgradePlanItemViewModel(EquipmentUpgradePlanItemModel plan)
	{
		Plan = plan;

		EquipmentPicker = Ioc.Default.GetService<EquipmentPickerService>()!;

		LoadModel();

		PropertyChanged += EquipmentUpgradePlanItemViewModel_PropertyChanged;
	}

	private void LoadModel()
	{
		Id = Plan.Id;
		DesiredUpgradeLevel = Plan.DesiredUpgradeLevel;
		Finished = Plan.Finished;
		Priority = Plan.Priority;

		if (!KCDatabase.Instance.MasterEquipments.ContainsKey((int)Plan.EquipmentId)) return;

		IEquipmentDataMaster masterEquipment = KCDatabase.Instance.MasterEquipments[(int)Plan.EquipmentId];

		// Try to load the owned equipment
		// not found => scrapped ? lost ? logged on another acc ? (what to do ?)
		// not found => Set to null  
		IEquipmentData equipmentData = Plan.EquipmentMasterId switch
		{
			int => KCDatabase.Instance.Equipments.ContainsKey((int)Plan.EquipmentMasterId) switch
			{
				true => KCDatabase.Instance.Equipments[(int)Plan.EquipmentMasterId]!,
				_ => new EquipmentDataMock(masterEquipment)
			},
			_ => new EquipmentDataMock(masterEquipment)
		};

		Equipment = equipmentData;
		Update();
	}

	private void EquipmentUpgradePlanItemViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(Equipment)) Update();

		Save();
	}

	public void Update()
	{
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
			CurrentLevelDisplay = EquipmentUpgradePlanner.NotOwned;

	}

	public void Save()
	{
		Plan.EquipmentId = Equipment?.EquipmentId ?? EquipmentId.Unknown;
		Plan.EquipmentMasterId = Equipment?.MasterID > 0 ? Equipment.MasterID : null;
		Plan.DesiredUpgradeLevel = DesiredUpgradeLevel;
		Plan.Finished = Finished;
		Plan.Priority = Priority;
	}

	[ICommand]
	public void OpenEquipmentPicker()
	{
		IEquipmentData? newEquip = EquipmentPicker.OpenEquipmentPicker();
		if (newEquip != null)
		{
			Equipment = newEquip;
		}
	}

}
