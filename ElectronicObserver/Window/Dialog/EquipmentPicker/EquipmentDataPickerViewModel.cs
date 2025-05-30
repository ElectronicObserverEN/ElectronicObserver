﻿using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Data;

namespace ElectronicObserver.Window.Dialog.EquipmentPicker;

public class EquipmentDataPickerViewModel : EquipmentPickerViewModel
{
	protected override List<IEquipmentData> AllEquipments { get; }

	public EquipmentDataPickerViewModel()
	{
		AllEquipments = KCDatabase.Instance.Equipments
			.Values
			.Cast<IEquipmentData>()
			.OrderBy(s => s.MasterEquipment.CategoryType)
			.ThenBy(s => s.MasterID)
			.ToList();

		Initialize();
	}
}
