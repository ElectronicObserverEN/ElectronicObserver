﻿using ElectronicObserver.Data;
using System;
using System.Globalization;
using System.Windows.Data;

namespace ElectronicObserver.Converters;

public class EquipmentRarityConverter : IValueConverter
{
	public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
		=> value switch
		{
			int i => Constants.GetEquipmentRarity(i),
			_ => null
		};

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}
}