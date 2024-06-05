using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace ElectronicObserver.Avalonia.ShipGroup;

public class HpToBackgroundConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		IBrush? brush = value switch
		{
			<= 0.25 => ShipGroupColors.RedBrush,
			<= 0.50 => ShipGroupColors.OrangeBrush,
			<= 0.75 => ShipGroupColors.YellowBrush,
			< 1.00 => ShipGroupColors.GreenBrush,
			_ => null,
		};

		return brush;
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
