using ElectronicObserver.Core.Types;

namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;

public class EDeckInfo
{
	/// <summary>
	/// 0 = 3 ships or less <br />
	/// 1 = 4 ships <br />
	/// 2 = 5 ships or more <br />
	/// </summary>
	[JsonPropertyName("api_kind")]
	public int ApiKind { get; set; }

	/// <summary>
	/// Ids of the first 3 ships in fleet.
	/// If the enemy fleet has less than 3 ships, ids for all ships will be in the list.
	/// </summary>
	[JsonPropertyName("api_ship_ids")]
	public List<ShipId> ApiShipIds { get; set; } = new();
}
