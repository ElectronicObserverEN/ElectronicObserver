using ElectronicObserverTypes;

namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiGetShip
{
	[JsonPropertyName("api_ship_getmes")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiShipGetmes { get; set; } = default!;

	[JsonPropertyName("api_ship_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ShipId ApiShipId { get; set; } = default!;

	[JsonPropertyName("api_ship_name")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiShipName { get; set; } = default!;

	[JsonPropertyName("api_ship_type")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiShipType { get; set; } = default!;
}
