using System.Text.Json.Serialization;

namespace ElectronicObserver.Data.PoiDbSubmission.PoiDbFriendFleetSubmission;

public class PoiDbFriendFleetSubmissionData
{
	[JsonPropertyName("body")]
	public required Body Body { get; init; }
}
