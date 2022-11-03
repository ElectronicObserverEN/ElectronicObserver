using System;
using System.Globalization;
using System.Windows.Data;

namespace ElectronicObserver.Converters;

public class DateTimeDisplayConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		=> value switch
		{
			DateTime d => d.ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss"),
			_ => throw new NotImplementedException(),
		};

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}
}
