using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace ElectronicObserver.Avalonia.ShipGroup;

public class RepairTimeToBackgroundConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		IBrush? brush = value switch
		{
			TimeSpan { TotalMilliseconds: < 1 } => null,
			TimeSpan { TotalHours: < 1 } => ShipGroupColors.YellowBrush,
			TimeSpan { TotalHours: < 6 } => ShipGroupColors.OrangeBrush,
			_ => ShipGroupColors.RedBrush,
		};

		return brush;
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
