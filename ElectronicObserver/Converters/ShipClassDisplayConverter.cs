using System;
using System.Globalization;
using System.Windows.Data;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Data;

namespace ElectronicObserver.Converters;

public class ShipClassDisplayConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		=> value switch
		{
			ShipClass.Unknown => "*",
			ShipClass id => Constants.GetShipClass(id),
			_ => throw new ArgumentOutOfRangeException()
		};

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
