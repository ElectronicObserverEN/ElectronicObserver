namespace ElectronicObserver.Avalonia.UiBlocker;

public sealed class UiElementModel
{
	public PositionPreset Preset { get; set; }
	public double DesiredTop { get; set; }
	public double DesiredLeft { get; set; }
	public double DesiredWidth { get; set; } = 100;
	public double DesiredHeight { get; set; } = 100;
	public string BackgroundColor { get; set; } = "#80000000";
	public string? ImagePath { get; set; }
	public List<UiBlockerTrigger> ShowTriggers { get; set; } = [];
	public List<UiBlockerTrigger> HideTriggers { get; set; } = [];
	public List<ConditionModel> Conditions { get; set; } = [];

	public UiElementModel Clone() => new()
	{
		Preset = Preset,
		DesiredTop = DesiredTop,
		DesiredLeft = DesiredLeft,
		DesiredWidth = DesiredWidth,
		DesiredHeight = DesiredHeight,
		BackgroundColor = BackgroundColor,
		ImagePath = ImagePath,
		ShowTriggers = [.. ShowTriggers],
		HideTriggers = [.. HideTriggers],
		Conditions = [.. Conditions.Select(c => c.Clone())],
	};
}
