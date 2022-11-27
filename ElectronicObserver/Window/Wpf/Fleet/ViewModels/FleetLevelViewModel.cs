using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Services;
using ElectronicObserver.Utility.Storage;
using ElectronicObserver.ViewModels;
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Window.Tools.ExpChecker;

namespace ElectronicObserver.Window.Wpf.Fleet.ViewModels;

public partial class FleetLevelViewModel : ObservableObject
{
	private ToolService ToolService { get; }

	public string? TextNext { get; internal set; }
	public int Value { get; set; }
	public int MaximumValue { get; set; }
	public int ValueNext { get; set; }
	public int Tag { get; set; }
	public string? ToolTip { get; set; }
	public SerializableFont SubFont { get; set; }
	public System.Drawing.Color SubFontColor { get; set; }
	public bool NextVisible { get; set; }

	public FontFamily SubFontFamily => new(SubFont.FontData.FontFamily.Name);
	public double SubFontSize => SubFont.FontData.ToSize();
	public SolidColorBrush SubForeground => SubFontColor.ToBrush();

	public FleetLevelViewModel()
	{
		ToolService = Ioc.Default.GetRequiredService<ToolService>();
	}

	[RelayCommand]
	private void OpenExpChecker()
	{
		ToolService.ExpChecker(new ExpCheckerViewModel(Tag));
	}
}
