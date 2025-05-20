using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Avalonia.Services;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer;

public class SortieRecordShipViewModel(IShipData ship, ImageLoadService imageLoadService) : ObservableObject
{
	private IShipData Ship { get; } = ship;
	private ImageLoadService ImageLoadService { get; } = imageLoadService;

	private bool TriedToLoadImage { get; set; }

	public BitmapSource? ShipImageSource
	{
		get
		{
			if (field is null)
			{
				Task.Run(LoadImage);
			}

			return field;
		}

		set;
	}

	private async Task LoadImage()
	{
		if (TriedToLoadImage) return;

		TriedToLoadImage = true;

		Bitmap? bitmap = await ImageLoadService.GetShipImage(Ship.MasterShip.ShipId, GameResourceHelper.ResourceTypeShipBanner);

		await App.Current!.Dispatcher.BeginInvoke(() =>
		{
			ShipImageSource = bitmap?.ToImageSource();
		});

		OnPropertyChanged(nameof(ShipImageSource));
	}
}
