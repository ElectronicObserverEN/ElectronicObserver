using System;
using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Resource;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Wpf.BaseAirCorps;

public class BaseAirCorpsItemControlViewModel : ObservableObject
{
	public string? Text { get; set; }
	public bool Visible { get; set; }
	public Enum? ImageIndex { get; set; }
	public int Tag { get; set; }
	public string? ToolTip { get; set; }

	public Visibility Visibility => Visible.ToVisibility();
	public ImageSource? Icon => ImageIndex switch
	{
		ResourceManager.EquipmentContent e => ImageSourceIcons.GetEquipmentIcon((EquipmentIconType)e),
		IconContent i => ImageSourceIcons.GetIcon(i),
		_ => null,
	};


}
