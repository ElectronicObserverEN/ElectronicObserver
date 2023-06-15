using ElectronicObserver.KancolleApi.Types.ApiReqRanking.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqRanking.Mxltvkpyuklh;

public class ApiReqRankingMxltvkpyuklhResponse
{
	[JsonPropertyName("api_count")]
	[Required]
	public int ApiCount { get; set; }

	[JsonPropertyName("api_page_count")]
	[Required]
	public int ApiPageCount { get; set; }

	[JsonPropertyName("api_disp_page")]
	[Required]
	public int ApiDispPage { get; set; }

	[JsonPropertyName("api_list")]
	[Required]
	public List<ApiList> ApiList { get; set; } = new();
}
