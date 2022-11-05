﻿using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Converters;
using ElectronicObserver.Services;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;

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

	public EquipmentUpgradePlanItemViewModel(IEquipmentData equipment)
	{
		PropertyChanged += EquipmentUpgradePlanItemViewModel_PropertyChanged;

		EquipmentPicker = Ioc.Default.GetService<EquipmentPickerService>()!;
		Equipment = equipment;
	}

	private void EquipmentUpgradePlanItemViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName != nameof(Equipment)) return;

		UpdateEquipmentDisplay();
	}

	public void UpdateEquipmentDisplay()
	{
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
