using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Resource;
using ElectronicObserver.Window.Wpf;

namespace ElectronicObserver.Converters;

public class EquipmentToImageConverter : IValueConverter
{
	public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is not IEquipmentDataMaster equipment) return null;

		try
		{
			string? imageUri = KCResourceHelper.GetEquipmentImagePath(equipment.ID, KCResourceHelper.ResourceTypeEquipmentCard) ??
							   KCResourceHelper.GetEquipmentImagePath(equipment.ID, KCResourceHelper.ResourceTypeEquipmentCardSmall);

			return imageUri switch
			{
				null => null,
				_ => new BitmapImage(new Uri(imageUri, UriKind.RelativeOrAbsolute).ToAbsolute())
			};
		}
		catch
		{
			return null;
		}
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}
}
