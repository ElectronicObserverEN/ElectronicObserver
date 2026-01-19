namespace ElectronicObserver.Window.Dialog.UiBlocker;

public class UiBlockerConfiguration
{
	public bool IsEnabled { get; set; }
	public double DesiredTop { get; set; }
	public double DesiredLeft { get; set; }
	public double DesiredHeight { get; set; }
	public double DesiredWidth { get; set; }
	public string? ImagePath { get; set; }
	public string? BackgroundColor { get; set; } = "#80000000";
}
