using System.Text.Json.Serialization;

namespace ElectronicObserver.Data.PoiDbSubmission.PoiDbRouteSubmission;

public class PoiDbRouteSubmissionData
{
	[JsonPropertyName("form")]
	public required Form Form { get; init; }
}
