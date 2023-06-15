namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstMapinfo
{
	[JsonPropertyName("api_id")]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_infotext")]
	public string ApiInfotext { get; set; } = default!;

	[JsonPropertyName("api_item")]
	public List<int> ApiItem { get; set; } = new();

	[JsonPropertyName("api_level")]
	public int ApiLevel { get; set; } = default!;

	[JsonPropertyName("api_maparea_id")]
	public int ApiMapareaId { get; set; } = default!;

	[JsonPropertyName("api_max_maphp")]
	public int? ApiMaxMaphp { get; set; } = default!;

	[JsonPropertyName("api_name")]
	public string ApiName { get; set; } = default!;

	[JsonPropertyName("api_no")]
	public int ApiNo { get; set; } = default!;

	[JsonPropertyName("api_opetext")]
	public string ApiOpetext { get; set; } = default!;

	[JsonPropertyName("api_required_defeat_count")]
	public int? ApiRequiredDefeatCount { get; set; } = default!;

	[JsonPropertyName("api_sally_flag")]
	public List<int> ApiSallyFlag { get; set; } = new();
}
