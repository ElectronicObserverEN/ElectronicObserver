namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;

public class ApiAirsearch
{
	[JsonPropertyName("api_plane_type")]
	public int ApiPlaneType { get; set; } = default!;

	[JsonPropertyName("api_result")]
	public int ApiResult { get; set; } = default!;
}
