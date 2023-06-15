namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;

public class ApiHappening
{
	[JsonPropertyName("api_count")]
	public int ApiCount { get; set; } = default!;

	[JsonPropertyName("api_dentan")]
	public int ApiDentan { get; set; } = default!;

	[JsonPropertyName("api_icon_id")]
	public int ApiIconId { get; set; } = default!;

	[JsonPropertyName("api_mst_id")]
	public int ApiMstId { get; set; } = default!;

	[JsonPropertyName("api_type")]
	public int ApiType { get; set; } = default!;

	[JsonPropertyName("api_usemst")]
	public int ApiUsemst { get; set; } = default!;
}
