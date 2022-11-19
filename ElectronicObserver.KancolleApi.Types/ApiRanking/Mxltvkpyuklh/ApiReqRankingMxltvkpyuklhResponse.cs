using System.Text.Json.Serialization;
using ElectronicObserver.KancolleApi.Types.ApiRanking.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiRanking.Mxltvkpyuklh;

public class ApiReqRankingMxltvkpyuklhResponse
{
	[JsonPropertyName("api_count")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public int ApiCount { get; set; }

	[JsonPropertyName("api_page_count")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public int ApiPageCount { get; set; }

	[JsonPropertyName("api_disp_page")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public int ApiDispPage { get; set; }

	[JsonPropertyName("api_list")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiList> ApiList { get; set; } = new();
}
