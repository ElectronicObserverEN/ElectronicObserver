using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Window.Wpf.FleetPreset;

public class FleetPresetItemControlViewModel : ObservableObject
{
	public string? Text { get; set; }
	public string? ToolTip { get; set; }
}
