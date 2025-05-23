using System.Text.Json.Serialization;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Data.PoiDbSubmission.PoiDbBattleSubmission;

public class PoiPlaneInfoSlot
{
	[JsonPropertyName("api_alv")]
	public required int ApiAlv { get; init; }

	[JsonPropertyName("api_level")]
	public required int ApiLevel { get; init; }

	[JsonPropertyName("api_locked")]
	public required int ApiLocked { get; init; }

	[JsonPropertyName("api_slotitem_id")]
	public required EquipmentId ApiSlotitemId { get; init; }

	[JsonPropertyName("api_id")]
	public required int ApiId { get; init; }
}
