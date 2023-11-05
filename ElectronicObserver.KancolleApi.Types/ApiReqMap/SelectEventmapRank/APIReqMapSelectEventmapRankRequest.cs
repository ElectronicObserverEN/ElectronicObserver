namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.SelectEventmapRank;

public class ApiReqMapSelectEventmapRankRequest
{
	[JsonPropertyName("api_map_no")]
	public string ApiMapNo { get; set; } = "";

	[JsonPropertyName("api_maparea_id")]
	public string ApiMapareaId { get; set; } = "";

	[JsonPropertyName("api_rank")]
	public string ApiRank { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";
}
