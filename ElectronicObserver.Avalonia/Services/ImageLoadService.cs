using System.Collections.Concurrent;
using Avalonia.Media.Imaging;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Avalonia.Services;

public class ImageLoadService(GameResourceHelper gameResourceHelper, GameAssetDownloaderService gameAssetDownloader)
{
	private GameResourceHelper GameResourceHelper { get; } = gameResourceHelper;
	private GameAssetDownloaderService GameAssetDownloader { get; } = gameAssetDownloader;
	private static ConcurrentDictionary<(ShipId, string), Lazy<Task>> Cache { get; } = new();

	static ImageLoadService()
	{
		Task.Run(async () =>
		{
			while (true)
			{
				await Task.Delay(TimeSpan.FromMinutes(5));

				Cache.Clear();
			}
			// ReSharper disable once FunctionNeverReturns
		});
	}

	public async Task<Bitmap?> GetShipImage(ShipId shipId, string resourceType)
	{
		Bitmap? shipImage = GetShipImageFromDisk(shipId, resourceType);

		if (shipImage is not null) return shipImage;

		Lazy<Task> downloadImageTask = Cache.GetOrAdd((shipId, resourceType), _ =>
		{
			return new Lazy<Task>(() => GameAssetDownloader.DownloadImage(shipId, resourceType));
		});

		await downloadImageTask.Value;

		return GetShipImageFromDisk(shipId, resourceType);
	}

	public Bitmap? GetShipImageFromDisk(ShipId shipId, string resourceType)
	{
		Bitmap? shipImage = GameResourceHelper.LoadShipImage(shipId, false, resourceType);

		return shipImage;
	}
}
