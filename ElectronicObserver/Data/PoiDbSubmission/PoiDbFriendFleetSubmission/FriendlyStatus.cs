using System.Text.Json.Serialization;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.Data.PoiDbSubmission.PoiDbFriendFleetSubmission;

public class FriendlyStatus
{
	[JsonPropertyName("firenumBefore")]
	public required int FirenumBefore { get; init; }

	[JsonPropertyName("firenum")]
	public required int Firenum { get; init; }

	[JsonPropertyName("flag")]
	public required FriendFleetRequestFlag Flag { get; init; }

	[JsonPropertyName("type")]
	public required FriendFleetRequestType Type { get; init; }

	[JsonPropertyName("max_maphp")]
	public required int MaxMaphp { get; init; }

	[JsonPropertyName("now_maphp")]
	public required int NowMaphp { get; init; }

	[JsonPropertyName("version")]
	public required string Version { get; init; }
}
