using ElectronicObserverTypes;

namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiGetShip
{
	[JsonPropertyName("api_ship_getmes")]
	[Required(AllowEmptyStrings = true)]
	public string ApiShipGetmes { get; set; } = default!;

	[JsonPropertyName("api_ship_id")]
	public ShipId ApiShipId { get; set; } = default!;

	[JsonPropertyName("api_ship_name")]
	[Required(AllowEmptyStrings = true)]
	public string ApiShipName { get; set; } = default!;

	[JsonPropertyName("api_ship_type")]
	[Required(AllowEmptyStrings = true)]
	public string ApiShipType { get; set; } = default!;
}
