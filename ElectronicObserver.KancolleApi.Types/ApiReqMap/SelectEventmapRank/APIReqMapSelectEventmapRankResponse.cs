using ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.SelectEventmapRank;

public class ApiReqMapSelectEventmapRankResponse
{
	[JsonPropertyName("api_maphp")]
	public ApiMaphp ApiMaphp { get; set; } = new();

	[JsonPropertyName("api_sally_flag")]
	public List<int>? ApiSallyFlag { get; set; }
}
