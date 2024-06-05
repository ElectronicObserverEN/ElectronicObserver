using Avalonia.Media;

namespace ElectronicObserver.Avalonia.ShipGroup;

public static class ShipGroupColors
{
	// foreground and background for fluent theme
	public static Color White { get; } = Colors.White;
	public static Color Black { get; } = Colors.Black;

	private static Color Red { get; } = Color.FromArgb(0xFF, 0xFF, 0xBB, 0xBB);
	private static Color Orange { get; } = Color.FromArgb(0xFF, 0xFF, 0xDD, 0xBB);
	private static Color Yellow { get; } = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xBB);
	private static Color Green { get; } = Color.FromArgb(0xFF, 0xBB, 0xFF, 0xBB);
	private static Color Gray { get; } = Color.FromArgb(0xFF, 0xBB, 0xBB, 0xBB);
	private static Color Cherry { get; } = Color.FromArgb(0xFF, 0xFF, 0xDD, 0xDD);
	private static Color Blue { get; } = Color.FromArgb(0xFF, 173, 216, 230);
	private static Color Purple { get; } = Color.FromArgb(0xFF, 156, 143, 238);
	private static Color Cyan { get; } = Color.FromArgb(0xFF, 224, 255, 255);
	public static Color Transparent { get; } = Colors.Transparent;

	public static SolidColorBrush WhiteBrush { get; } = new(White);
	public static SolidColorBrush BlackBrush { get; } = new(Black);

	public static SolidColorBrush RedBrush { get; } = new(Red);
	public static SolidColorBrush OrangeBrush { get; } = new(Orange);
	public static SolidColorBrush YellowBrush { get; } = new(Yellow);
	public static SolidColorBrush GreenBrush { get; } = new(Green);
}
