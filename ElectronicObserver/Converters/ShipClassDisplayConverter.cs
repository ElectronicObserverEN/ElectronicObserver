﻿using System;
using System.Globalization;
using System.Windows.Data;
using ElectronicObserver.Data;
using ElectronicObserverTypes;

namespace ElectronicObserver.Converters;

public class ShipClassDisplayConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		=> value switch
		{
			0 => "*",
			111 => Constants.GetShipClass(111, ShipId.Souya645),
			int id => Constants.GetShipClass(id),
			_ => throw new ArgumentOutOfRangeException()
		};

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
