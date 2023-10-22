using System;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Resource;

namespace ElectronicObserver.Window.Wpf.FleetOverview;

public class FleetOverviewItemViewModel : ObservableObject
{
	public string? Text { get; set; }
	public string? ToolTip { get; set; }
	public IconContent? Icon { get; set; }
	public bool Visible { get; set; }
	public DateTime? Tag { get; set; }
}
