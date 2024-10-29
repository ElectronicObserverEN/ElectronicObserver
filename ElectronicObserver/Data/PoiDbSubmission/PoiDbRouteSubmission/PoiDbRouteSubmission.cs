using System.Text.Json.Serialization;

namespace ElectronicObserver.Data.PoiDbSubmission.PoiDbRouteSubmission;

public class PoiDbRouteSubmission
{
	[JsonPropertyName("form")]
	public required Form Form { get; init; }
}
