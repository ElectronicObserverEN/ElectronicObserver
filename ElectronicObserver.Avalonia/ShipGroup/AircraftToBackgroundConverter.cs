using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace ElectronicObserver.Avalonia.ShipGroup;

public class AircraftToBackgroundConverter : IValueConverter
{
	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is not Fraction { Max: > 0 } fraction) return ShipGroupColors.TransparentBrush;

		IBrush brush = fraction switch
		{
			{ Current: 0 } => ShipGroupColors.RedBrush,
			{ Rate: < 1 } => ShipGroupColors.YellowBrush,
			_ => ShipGroupColors.TransparentBrush,
		};

		return brush;
	}

	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
