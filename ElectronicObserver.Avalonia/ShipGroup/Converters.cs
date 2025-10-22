using Avalonia.Data.Converters;
using Avalonia.Media;

namespace ElectronicObserver.Avalonia.ShipGroup;

public static class Converters
{
	public static FuncValueConverter<int, string> RemainingToTextConverter { get; } =
		new(r => r switch
		{
			0 => "MAX",
			_ => r.ToString(),
		});

	public static FuncValueConverter<TimeSpan, string> RepairTimeDisplayConverter { get; } =
		new(t => $"{(int)t.TotalHours:00}:{t.Minutes:00}:{t.Seconds:00}");

	public static FuncValueConverter<ShipGroupItemViewModel, string?> ShipToLockConverter { get; } =
		new(r => r switch
		{
			{ IsLocked: true } => "\u2764\ufe0f",
			{ IsLockedByEquipment: true } => "\u2b1b",
			_ => null,
		});

	private static SolidColorBrush HeartLockBrush { get; } = new(Colors.IndianRed);
	private static SolidColorBrush EquipmentLockBrush { get; } = new(Colors.Gray);
	private static SolidColorBrush NoLockBrush { get; } = new(Colors.Transparent);

	public static FuncValueConverter<ShipGroupItemViewModel, SolidColorBrush> ShipToLockForegroundConverter { get; } =
		new(r => r switch
		{
			{ IsLocked: true } => HeartLockBrush,
			{ IsLockedByEquipment: true } => EquipmentLockBrush,
			_ => NoLockBrush,
		});
}
