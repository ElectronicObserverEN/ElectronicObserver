using System.Text.Json.Serialization;

namespace ElectronicObserver.Data.PoiDbSubmission.PoiDbFriendFleetSubmission;

public class PoiDbFriendFleetSubmission
{
	[JsonPropertyName("body")]
	public required Body Body { get; init; }
}
