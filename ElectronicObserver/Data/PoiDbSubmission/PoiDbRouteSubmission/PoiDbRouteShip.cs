using System.Text.Json.Serialization;

namespace ElectronicObserver.Data.PoiDbSubmission.PoiDbRouteSubmission;

public class PoiDbRouteShip
{
	[JsonPropertyName("api_ship_id")]
	public required string ApiShipId { get; init; }

	[JsonPropertyName("api_lv")]
	public required string ApiLv { get; init; }

	[JsonPropertyName("api_sally_area")]
	public required string? ApiSallyArea { get; init; }

	[JsonPropertyName("api_soku")]
	public required string ApiSoku { get; init; }

	[JsonPropertyName("api_slotitem_ex")]
	public required string ApiSlotitemEx { get; init; }

	[JsonPropertyName("api_slotitem_level")]
	public required string ApiSlotitemLevel { get; init; }
}
