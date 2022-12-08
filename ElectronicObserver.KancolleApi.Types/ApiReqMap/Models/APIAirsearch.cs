namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;

public class ApiAirsearch
{
	[JsonPropertyName("api_plane_type")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiPlaneType { get; set; } = default!;

	[JsonPropertyName("api_result")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiResult { get; set; } = default!;
}
