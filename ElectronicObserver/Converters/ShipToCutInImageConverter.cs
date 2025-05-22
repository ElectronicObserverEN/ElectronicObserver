using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Window.Wpf;

namespace ElectronicObserver.Converters;

public class ShipToCutInImageConverter : IValueConverter
{
	public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		int? shipId = value switch
		{
			IShipDataMaster ship => ship.ID,
			ShipId.Unknown => null,
			ShipId i => (int)i,

			_ => null,
		};

		if (shipId is not int id) return null;

		try
		{
			string? imageUri = KCResourceHelper
				.GetShipImagePath(id, false, KCResourceHelper.ResourceTypeShipCutin);

			return imageUri switch
			{
				null => CutInImageFromFullImage(KCDatabase.Instance.MasterShips[id]),
				_ => new BitmapImage(new Uri(imageUri, UriKind.RelativeOrAbsolute).ToAbsolute()),
			};
		}
		catch
		{
			return null;
		}
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}

	private static readonly Size ShipCutinSize = new(665, 121);

	private BitmapImage? CutInImageFromFullImage(IShipDataMaster ship)
	{
		using Bitmap? shipImageOriginal = KCResourceHelper.LoadShipImage(ship.ShipID, false, KCResourceHelper.ResourceTypeShipFull);
		
		if (shipImageOriginal == null)
		{
			return null;
		}

		using Bitmap shipImage = new(ShipCutinSize.Width, ShipCutinSize.Height, PixelFormat.Format32bppArgb);
		using Graphics shipg = Graphics.FromImage(shipImage);

		shipg.InterpolationMode = InterpolationMode.HighQualityBicubic;
		shipg.PixelOffsetMode = PixelOffsetMode.HighQuality;

		Rectangle face = ship.GraphicData.FaceArea;
		PointF faceCenter = new(face.X + face.Width / 2f, face.Y + face.Height / 2f);

		PointF zone = new(ShipCutinSize.Width * 0.2f, ShipCutinSize.Height * 0.2f);
		float rate = face.Height == 0 ? 1 : (ShipCutinSize.Height * 4f / 3f / face.Height);

		shipg.DrawImage(shipImageOriginal, new RectangleF(
			-faceCenter.X * rate + zone.X,
			-faceCenter.Y * rate + zone.Y,
			shipImageOriginal.Width * rate,
			shipImageOriginal.Height * rate
		));

		using MemoryStream memory = new();

		shipImage.Save(memory, ImageFormat.Bmp);
		memory.Position = 0;
		BitmapImage bitmapimage = new();
		bitmapimage.BeginInit();
		bitmapimage.StreamSource = memory;
		bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
		bitmapimage.EndInit();

		return bitmapimage;
	}
}
