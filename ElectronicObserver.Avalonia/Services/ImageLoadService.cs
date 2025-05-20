using Avalonia.Media.Imaging;
using ElectronicObserverTypes;

namespace ElectronicObserver.Avalonia.Services;

public class ImageLoadService(GameResourceHelper gameResourceHelper, GameAssetDownloaderService gameAssetDownloader)
{
	private GameResourceHelper GameResourceHelper { get; } = gameResourceHelper;
	private GameAssetDownloaderService GameAssetDownloader { get; } = gameAssetDownloader;

	public async Task<Bitmap?> GetShipImage(ShipId shipId, string resourceType)
	{
		Bitmap? shipImage = GetShipImageFromDisk(shipId, resourceType);

		if (shipImage is not null) return shipImage;

		await GameAssetDownloader.DownloadImage(shipId, resourceType);

		return GetShipImageFromDisk(shipId, resourceType);
	}

	public Bitmap? GetShipImageFromDisk(ShipId shipId, string resourceType)
	{
		Bitmap? shipImage = GameResourceHelper.LoadShipImage(shipId, false, resourceType);

		return shipImage;
	}
}
