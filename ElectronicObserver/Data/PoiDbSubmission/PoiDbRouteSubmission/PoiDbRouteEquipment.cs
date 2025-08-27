using System.Text.Json.Serialization;

namespace ElectronicObserver.Data.PoiDbSubmission.PoiDbRouteSubmission;

public class PoiDbRouteEquipment
{
	[JsonPropertyName("api_id")]
	public required string ApiId { get; init; }

	[JsonPropertyName("api_slotitem_id")]
	public required string ApiSlotitemId { get; init; }

	[JsonPropertyName("api_locked")]
	public required string ApiLocked { get; init; }

	[JsonPropertyName("api_level")]
	public required string ApiLevel { get; init; }

	[JsonPropertyName("api_alv")]
	public required string ApiAlv { get; init; }
}
