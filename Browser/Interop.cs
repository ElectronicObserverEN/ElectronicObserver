using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace Browser;

public static class Interop
{
	public static BitmapSource ToBitmapSource(this Bitmap bitmap, ImageFormat format)
	{
		using MemoryStream memory = new();

		bitmap.Save(memory, format);
		memory.Position = 0;

		BitmapImage bitmapimage = new();
		bitmapimage.BeginInit();
		bitmapimage.StreamSource = memory;
		bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
		bitmapimage.EndInit();

		return bitmapimage;
	}
}
