using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Resource;
using ElectronicObserver.Window.Wpf;

namespace ElectronicObserver.Converters;

public class ShipToAlbumStatusConverter : IValueConverter
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
				.GetShipImagePath(id, false, KCResourceHelper.ResourceTypeShipName);

			BitmapImage image = new(new Uri(imageUri, UriKind.RelativeOrAbsolute).ToAbsolute());

			int leftOffset = 140;
			int topOffset = 5;
			int rightOffset = 60;
			int bottomOffset = 25;
			Int32Rect region = new
			(
				leftOffset, 
				topOffset, 
				(int)image.Width - leftOffset - rightOffset, 
				(int)image.Height - topOffset - bottomOffset
			);
			
			CroppedBitmap cropped = new(image, region);

			return cropped;
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
