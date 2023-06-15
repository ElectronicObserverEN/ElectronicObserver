namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.SelectEventmapRank;

public class ApiReqMapSelectEventmapRankRequest
{
	[JsonPropertyName("api_map_no")]
	[Required(AllowEmptyStrings = true)]
	public string ApiMapNo { get; set; } = default!;

	[JsonPropertyName("api_maparea_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiMapareaId { get; set; } = default!;

	[JsonPropertyName("api_rank")]
	[Required(AllowEmptyStrings = true)]
	public string ApiRank { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
