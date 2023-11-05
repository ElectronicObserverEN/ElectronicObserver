using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Window.Wpf.Headquarters;

public class HeadquarterItemViewModel : ObservableObject
{
	public string? Text { get; set; }
	public string? ToolTip { get; set; }
	public bool Visible { get; set; } = true;
	public System.Drawing.Color BackColor { get; set; }
	public System.Drawing.Color ForeColor { get; set; }
	public bool Tag { get; set; }

	public SolidColorBrush Foreground => ForeColor.ToBrush();
	public SolidColorBrush Background => BackColor.ToBrush();
}
