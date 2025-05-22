using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Resource;
using ElectronicObserver.Window.Wpf;

namespace ElectronicObserver.Converters;

public class ShipToCardImageConverter : IValueConverter
{
	public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		int? shipId = value switch
		{
			IShipDataMaster ship => ship.ID,
			ShipId i => (int)i,

			_ => null,
		};

		if (shipId is not int id) return null;

		try
		{
			string? imageUri = KCResourceHelper
				.GetShipImagePath(id, false, KCResourceHelper.ResourceTypeShipCard);

			return new BitmapImage(new Uri(imageUri, UriKind.RelativeOrAbsolute).ToAbsolute());
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
