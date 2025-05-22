using System;
using System.IO;
using System.Linq;

namespace ElectronicObserver.Resource;

public static class KCResourceHelper
{
	public static readonly string ResourceTypeShipName = "album_status";
	public static readonly string ResourceTypeShipBanner = "banner";
	public static readonly string ResourceTypeShipCard = "card";
	public static readonly string ResourceTypeShipAlbumFull = "character_full";
	public static readonly string ResourceTypeShipAlbumZoom = "character_up";
	public static readonly string ResourceTypeShipFull = "full";
	public static readonly string ResourceTypeShipCutin = "remodel";
	public static readonly string ResourceTypeShipSupply = "supply_character";

	public static readonly string ResourceTypeEquipmentText = "btxt_flat";
	public static readonly string ResourceTypeEquipmentCard = "card";
	public static readonly string ResourceTypeEquipmentCardSmall = "card_t";
	public static readonly string ResourceTypeEquipmentFairy = "item_character";
	public static readonly string ResourceTypeEquipmentAlbum = "item_on";
	public static readonly string ResourceTypeEquipmentWeapon = "item_up";
	public static readonly string ResourceTypeEquipmentSpec = "remodel";
	public static readonly string ResourceTypeEquipmentName = "statustop_item";



	/// <summary>
	/// 艦船画像リソースへのパスを取得します。
	/// </summary>
	/// <param name="shipID">艦船ID。</param>
	/// <param name="isDamaged">中破グラフィックかどうか。</param>
	/// <param name="resourceType">画像種別。["album_status", "banner", "card", "character_full", "character_up", "full", "remodel", "supply_character"]</param>
	/// <returns></returns>
	private static string GetShipResourcePath(int shipID, bool isDamaged, string resourceType)
	{
		if (resourceType == "album_status")
			isDamaged = false;

		if (isDamaged)
			resourceType += "_dmg";

		return $@"kcs2\resources\ship\{resourceType}\{shipID:d4}_";
	}

	/// <summary>
	/// 装備画像リソースへのパスを取得します。
	/// </summary>
	/// <param name="equipmentID">装備ID。</param>
	/// <param name="resourceType">画像種別。["btxt_flat", "card", "card_t", "item_character", "item_on", "item_up", "remodel", "statustop_item"]</param>
	/// <param name="useOldFormat">Old format used 3 digits for equipment id, new one uses 4 digits.</param>
	/// <returns></returns>
	private static string GetEquipmentResourcePath(int equipmentID, string resourceType, bool useOldFormat = false)
	{
		return useOldFormat switch
		{
			true => $@"kcs2\resources\slot\{resourceType}\{equipmentID:d3}_",
			_ => $@"kcs2\resources\slot\{resourceType}\{equipmentID:d4}_",
		};
	}




	/// <summary>
	/// 最新のリソースへのパスを取得します。
	/// </summary>
	public static string? GetLatestResourcePath(string resourcePath)
	{
		string root = Utility.Configuration.Config.Connection.SaveDataPath + "\\" + Path.GetDirectoryName(resourcePath);

		try
		{
			if (!Directory.Exists(root))
				return null;

			return
				Directory.EnumerateFiles(root, Path.GetFileNameWithoutExtension(resourcePath) + "*.png", SearchOption.TopDirectoryOnly)
					.OrderBy(path => File.GetLastWriteTime(path))
					.LastOrDefault();
		}
		catch (Exception)
		{
			return null;
		}
	}


	public static string? GetShipImagePath(int shipID, bool isDamaged, string resourceType)
		=> GetLatestResourcePath(GetShipResourcePath(shipID, isDamaged, resourceType));

	public static string? GetEquipmentImagePath(int equipmentID, string resourceType)
		=> GetLatestResourcePath(GetEquipmentResourcePath(equipmentID, resourceType))
		?? GetLatestResourcePath(GetEquipmentResourcePath(equipmentID, resourceType, true));
}
