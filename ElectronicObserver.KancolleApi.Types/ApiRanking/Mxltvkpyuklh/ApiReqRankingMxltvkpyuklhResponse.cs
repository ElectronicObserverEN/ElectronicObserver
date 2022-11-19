using System.Text.Json.Serialization;
using ElectronicObserver.KancolleApi.Types.ApiRanking.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiRanking.Mxltvkpyuklh;

public class ApiReqRankingMxltvkpyuklhResponse
{
	[JsonPropertyName("api_count")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public int ApiCount { get; set; }

	[JsonPropertyName("api_page_count")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public int ApiPageCount { get; set; }

	[JsonPropertyName("api_disp_page")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public int ApiDispPage { get; set; }

	[JsonPropertyName("api_list")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiList> ApiList { get; set; } = new();
}
