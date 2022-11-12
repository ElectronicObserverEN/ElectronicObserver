using ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.SelectEventmapRank;

public class ApiReqMapSelectEventmapRankResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_maphp")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public ApiMaphp ApiMaphp { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_sally_flag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<int>? ApiSallyFlag { get; set; } = default!;
}
