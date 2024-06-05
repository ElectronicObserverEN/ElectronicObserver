using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace ElectronicObserver.Avalonia.ShipGroup;

public class ConditionToBackgroundConverter : IMultiValueConverter
{
	public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
	{
		object? condition = values[0];
		object? conditionBorder = values[1];

		IBrush? brush = (condition, conditionBorder) switch
		{
			( <= 20, _) => ShipGroupColors.RedBrush,
			( <= 30, _) => ShipGroupColors.OrangeBrush,
			(int c, int b) when c < b => ShipGroupColors.YellowBrush,
			( < 50, _) => null,
			_ => ShipGroupColors.GreenBrush,
		};

		return brush;
	}
}
