using System.Text.Json.Serialization;

namespace ElectronicObserver.Data.PoiDbSubmission.PoiDbRouteSubmission;

public class PoiApiItemget
{
	[JsonPropertyName("api_getcount")]
	public required string ApiGetcount { get; set; }

	[JsonPropertyName("api_icon_id")]
	public required string ApiIconId { get; set; }

	[JsonPropertyName("api_id")]
	public required string ApiId { get; set; }

	[JsonPropertyName("api_name")]
	public required string ApiName { get; set; }

	[JsonPropertyName("api_usemst")]
	public required string ApiUsemst { get; set; }
}
