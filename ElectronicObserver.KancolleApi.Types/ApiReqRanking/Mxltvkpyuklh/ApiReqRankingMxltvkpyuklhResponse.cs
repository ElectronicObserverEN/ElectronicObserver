using ElectronicObserver.KancolleApi.Types.ApiReqRanking.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqRanking.Mxltvkpyuklh;

public class ApiReqRankingMxltvkpyuklhResponse
{
	[JsonPropertyName("api_count")]
	public int ApiCount { get; set; }

	[JsonPropertyName("api_page_count")]
	public int ApiPageCount { get; set; }

	[JsonPropertyName("api_disp_page")]
	public int ApiDispPage { get; set; }

	[JsonPropertyName("api_list")]
	public List<ApiList> ApiList { get; set; } = new();
}
