using Avalonia.Data.Converters;

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
}
