namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;

public class ApiCellDatum
{
	[JsonPropertyName("api_color_no")]
	public int ApiColorNo { get; set; } = default!;

	[JsonPropertyName("api_distance")]
	public int? ApiDistance { get; set; } = default!;

	[JsonPropertyName("api_id")]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_no")]
	public int ApiNo { get; set; } = default!;

	[JsonPropertyName("api_passed")]
	public int ApiPassed { get; set; } = default!;
}
