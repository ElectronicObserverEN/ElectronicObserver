using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ElectronicObserver.Converters;

public class HexToBrushConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is string hex && !string.IsNullOrWhiteSpace(hex))
		{
			Color color = (Color)ColorConverter.ConvertFromString(hex);
			return new SolidColorBrush(color);
		}

		return Brushes.Transparent;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
