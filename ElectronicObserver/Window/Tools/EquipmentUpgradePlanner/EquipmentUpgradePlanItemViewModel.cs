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

	public bool Finished { get; set; }

	public int Priority { get; set; }

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
		Priority = Plan.Priority;
		EquipmentId = Plan.EquipmentMasterId;
		EquipmentMasterDataId = Plan.EquipmentId;

		Update();
	}

	private void EquipmentUpgradePlanItemViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName is nameof(EquipmentId) or nameof(EquipmentMasterDataId)) Update();

		Save();
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

	}

	public void Save()
	{
		Plan.EquipmentId = EquipmentMasterDataId;
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
			EquipmentId = newEquip.MasterID;
		}
	}

}
