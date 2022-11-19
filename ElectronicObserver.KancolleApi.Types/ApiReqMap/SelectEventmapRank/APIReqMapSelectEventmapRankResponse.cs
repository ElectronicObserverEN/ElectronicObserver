using ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.SelectEventmapRank;

public class ApiReqMapSelectEventmapRankResponse
{
	[JsonPropertyName("api_maphp")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public ApiMaphp ApiMaphp { get; set; } = new();

	[JsonPropertyName("api_sally_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiSallyFlag { get; set; } = default!;
}
