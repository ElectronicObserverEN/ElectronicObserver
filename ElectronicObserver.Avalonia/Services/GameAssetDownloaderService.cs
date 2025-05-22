using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Avalonia.Services;

public class GameAssetDownloaderService(IConfigurationConnection connection)
{
	private IConfigurationConnection Connection { get; } = connection;
	private static string ImageSourceUrl => "https://raw.githubusercontent.com/ElectronicObserverEN/GameAssets/main";

	public async Task DownloadImage(ShipId shipId, string resourceType)
	{
		string imagePath = GetShipImagePath(shipId, false, resourceType);

		using HttpClient client = new();

		HttpResponseMessage response = await client
			.GetAsync(UrlHelpers.Join(ImageSourceUrl, imagePath));

		if (!response.IsSuccessStatusCode) return;

		string fileSavePath = Connection.SaveDataPath;
		string saveLocation = Path.Combine(imagePath.Split("/").Prepend(fileSavePath).ToArray());

		string directoryName = Path.GetDirectoryName(saveLocation)!;
		Directory.CreateDirectory(directoryName);
		await using FileStream fs = new(saveLocation, FileMode.OpenOrCreate, FileAccess.Write);

		await response.Content.CopyToAsync(fs, null, CancellationToken.None);
	}

	// code down from here is a slightly adjusted version of:
	// https://gist.github.com/sanaehirotaka/c177c39c37ba09b3642705a0be5e2465
	private static int CreateKey(string? t) => t?.Sum(c => c) ?? 0;

	private static string CreateHash(int shipId, string imageType)
	{
		List<int> resource =
		[
			6657, 5699, 3371, 8909, 7719, 6229, 5449, 8561,
			2987, 5501, 3127, 9319, 4365, 9811, 9927, 2423, 3439, 1865, 5925,
			4409, 5509, 1517, 9695, 9255, 5325, 3691, 5519, 6949, 5607, 9539,
			4133, 7795, 5465, 2659, 6381, 6875, 4019, 9195, 5645, 2887, 1213,
			1815, 8671, 3015, 3147, 2991, 7977, 7045, 1619, 7909, 4451, 6573,
			4545, 8251, 5983, 2849, 7249, 7449, 9477, 5963, 2711, 9019, 7375,
			2201, 5631, 4893, 7653, 3719, 8819, 5839, 1853, 9843, 9119, 7023,
			5681, 2345, 9873, 6349, 9315, 3795, 9737, 4633, 4173, 7549, 7171,
			6147, 4723, 5039, 2723, 7815, 6201, 5999, 5339, 4431, 2911, 4435,
			3611, 4423, 9517, 3243
		];

		List<int> voice =
		[
			2475, 6547, 1471, 8691, 7847, 3595, 1767, 3311, 2507, 9651, 5321,
			4473, 7117, 5947, 9489, 2669, 8741, 6149, 1301, 7297, 2975, 6413,
			8391, 9705, 2243, 2091, 4231, 3107, 9499, 4205, 6013, 3393, 6401,
			6985, 3683, 9447, 3287, 5181, 7587, 9353, 2135, 4947, 5405, 5223,
			9457, 5767, 9265, 8191, 3927, 3061, 2805, 3273, 7331
		];

		int s = CreateKey(imageType);
		int a = imageType.Length;

		return (17 * (shipId + 7) * resource[(s + shipId * a) % 100] % 8973 + 1e3).ToString();
	}

	/*
		GetShip(476, false, "banner");
		Returns: kcs2/resources/ship/banner/0476_4522.png
		Full link: http://<KC server IP>/kcs2/resources/ship/banner/0476_4522.png

		GetShip(476, true, "remodel");
		Returns: kcs2/resources/ship/remodel_dmg/0476_8203.png
		Full link: http://<KC server IP>/kcs2/resources/ship/remodel_dmg/0476_8203.png
	*/
	private static string GetShipImagePath(ShipId shipId, bool damaged, string imageType)
	{
		int id = (int)shipId;
		string folderName = imageType + (damaged ? "_dmg" : "");
		string fullImageType = "ship_" + folderName;
		string imageHash = CreateHash(id, fullImageType);

		string paddedShipId = id.ToString().PadLeft(4, '0');

		return $"kcs2/resources/ship/{folderName}/{paddedShipId}_{imageHash}.png";
	}
}
