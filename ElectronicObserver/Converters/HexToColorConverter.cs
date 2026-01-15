using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ElectronicObserver.Converters;

public class HexToColorConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is string hex && !string.IsNullOrWhiteSpace(hex))
		{
			return (Color)ColorConverter.ConvertFromString(hex);
		}

		return Colors.Transparent;
	}

	public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is not Color color) return null;

		return $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
	}
}
