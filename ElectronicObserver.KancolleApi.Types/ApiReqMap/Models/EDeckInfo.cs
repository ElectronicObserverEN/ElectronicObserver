using ElectronicObserverTypes;

namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;

public class EDeckInfo
{
	/// <summary>
	/// 0 = 3 ships or less <br />
	/// 1 = 4 ships <br />
	/// 2 = 5 ships or more <br />
	/// Not 100% confirmed yet.
	/// </summary>
	[JsonPropertyName("api_kind")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiKind { get; set; } = default!;

	/// <summary>
	/// Ids of the first 3 ships in fleet.
	/// If the enemy fleet has less than 3 ships, ids for all ships will be in the list.
	/// </summary>
	[JsonPropertyName("api_ship_ids")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<ShipId> ApiShipIds { get; set; } = default!;
}
