using System.Text.Json.Serialization;

namespace ElectronicObserver.Data.PoiDbSubmission.PoiDbRouteSubmission;

public class PoiApiHappening
{
	[JsonPropertyName("api_count")]
	public required string ApiCount { get; set; }

	[JsonPropertyName("api_dentan")]
	public required string ApiDentan { get; set; }

	[JsonPropertyName("api_icon_id")]
	public required string ApiIconId { get; set; }

	[JsonPropertyName("api_mst_id")]
	public required string ApiMstId { get; set; }

	[JsonPropertyName("api_type")]
	public required string ApiType { get; set; }

	[JsonPropertyName("api_usemst")]
	public required string ApiUsemst { get; set; }
}
