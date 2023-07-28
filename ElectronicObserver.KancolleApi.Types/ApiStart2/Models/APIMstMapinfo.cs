namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstMapinfo
{
	[JsonPropertyName("api_id")]
	public int ApiId { get; set; }

	[JsonPropertyName("api_infotext")]
	public string ApiInfotext { get; set; } = "";

	[JsonPropertyName("api_item")]
	public List<int> ApiItem { get; set; } = new();

	[JsonPropertyName("api_level")]
	public int ApiLevel { get; set; }

	[JsonPropertyName("api_maparea_id")]
	public int ApiMapareaId { get; set; }

	[JsonPropertyName("api_max_maphp")]
	public int? ApiMaxMaphp { get; set; }

	[JsonPropertyName("api_name")]
	public string ApiName { get; set; } = "";

	[JsonPropertyName("api_no")]
	public int ApiNo { get; set; }

	[JsonPropertyName("api_opetext")]
	public string ApiOpetext { get; set; } = "";

	[JsonPropertyName("api_required_defeat_count")]
	public int? ApiRequiredDefeatCount { get; set; }

	[JsonPropertyName("api_sally_flag")]
	public List<int> ApiSallyFlag { get; set; } = new();
}
