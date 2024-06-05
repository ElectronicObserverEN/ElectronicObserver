using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace ElectronicObserver.Avalonia.ShipGroup;

public class AircraftToBackgroundConverter : IMultiValueConverter
{
	public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
	{
		int current = (int)values[0]!;
		int max = (int)values[1]!;

		if (max is 0) return ShipGroupColors.TransparentBrush;

		IBrush brush = current switch
		{
			0 => ShipGroupColors.RedBrush,
			_ when current < max => ShipGroupColors.YellowBrush,
			_ => ShipGroupColors.TransparentBrush,
		};

		return brush;
	}
}
