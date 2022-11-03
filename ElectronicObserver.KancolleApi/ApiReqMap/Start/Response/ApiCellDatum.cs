using System.Text.Json.Serialization;

namespace ElectronicObserver.KancolleApi.ApiReqMap.Start.Response;

public class ApiCellDatum
{

	[JsonPropertyName("api_color_no")]
	public int ApiColorNo { get; set; }

	[JsonPropertyName("api_distance")]
	public int ApiDistance { get; set; }

	[JsonPropertyName("api_id")]
	public int ApiId { get; set; }

	[JsonPropertyName("api_no")]
	public int ApiNo { get; set; }

	[JsonPropertyName("api_passed")]
	public int ApiPassed { get; set; }

}