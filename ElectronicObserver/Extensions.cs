using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Avalonia.Media.Imaging;
using CroppedBitmap = System.Windows.Media.Imaging.CroppedBitmap;

namespace ElectronicObserver;

public static class Extensions
{
	public static BitmapImage ToBitmapImage(this Bitmap bitmap)
	{
		using MemoryStream memoryStream = new();
		bitmap.Save(memoryStream);
		memoryStream.Seek(0, SeekOrigin.Begin);

		BitmapImage bitmapImage = new();
		bitmapImage.BeginInit();
		bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
		bitmapImage.StreamSource = memoryStream;
		bitmapImage.EndInit();

		return bitmapImage;
	}

	/// <summary>
	/// Used for the album_status image to remove the "No." text and some useless whitespace.
	/// </summary>
	public static CroppedBitmap ToShipNameBitmapImage(this Bitmap bitmap)
	{
		using MemoryStream memoryStream = new();
		bitmap.Save(memoryStream);
		memoryStream.Seek(0, SeekOrigin.Begin);

		BitmapImage image = new();
		image.BeginInit();
		image.CacheOption = BitmapCacheOption.OnLoad;
		image.StreamSource = memoryStream;
		image.EndInit();

		const int leftOffset = 140;
		const int topOffset = 5;
		const int rightOffset = 60;
		const int bottomOffset = 25;

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
}
