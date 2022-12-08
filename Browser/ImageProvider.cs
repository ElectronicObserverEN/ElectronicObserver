using System.Windows.Media;

namespace Browser;

public class ImageProvider
{
	public ImageSource? Screenshot { get; }
	public ImageSource? Zoom { get; }
	public ImageSource? ZoomIn { get; }
	public ImageSource? ZoomOut { get; }
	public ImageSource? Unmute { get; }
	public ImageSource? Mute { get; }
	public ImageSource? Refresh { get; }
	public ImageSource? Navigate { get; }
	public ImageSource? Other { get; }

	public ImageProvider(byte[][] images)
	{
		Screenshot = (ImageSource?)new ImageSourceConverter().ConvertFrom(images[0]);
		Zoom = (ImageSource?)new ImageSourceConverter().ConvertFrom(images[1]);
		ZoomIn = (ImageSource?)new ImageSourceConverter().ConvertFrom(images[2]);
		ZoomOut = (ImageSource?)new ImageSourceConverter().ConvertFrom(images[3]);
		Unmute = (ImageSource?)new ImageSourceConverter().ConvertFrom(images[4]);
		Mute = (ImageSource?)new ImageSourceConverter().ConvertFrom(images[5]);
		Refresh = (ImageSource?)new ImageSourceConverter().ConvertFrom(images[6]);
		Navigate = (ImageSource?)new ImageSourceConverter().ConvertFrom(images[7]);
		Other = (ImageSource?)new ImageSourceConverter().ConvertFrom(images[8]);
	}
}
