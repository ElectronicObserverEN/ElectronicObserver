﻿using System.Windows.Media;
using ElectronicObserverTypes;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Window.Wpf.Compass.ViewModels;

public class MasterShipSlotViewModel : ObservableObject
{
	public IEquipmentDataMaster? Equipment { get; set; }
	public int Size { get; set; }

	public string SizeString => Size switch
	{
		> 0 => $"{Size}",
		_ => ""
	};
	public ImageSource? EquipmentIcon =>
		ImageSourceIcons.GetEquipmentIcon(Equipment?.IconTypeTyped ?? EquipmentIconType.Nothing);
}
