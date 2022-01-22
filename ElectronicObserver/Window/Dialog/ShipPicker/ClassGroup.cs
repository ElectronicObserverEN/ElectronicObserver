﻿using System.Collections.Generic;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Dialog.ShipPicker;

public class ClassGroup
{
	public int Id { get; set; }
	public string Name { get; set; }
	public List<IShipDataMaster> Ships { get; set; } = new();
}