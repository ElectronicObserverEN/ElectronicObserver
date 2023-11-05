namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;

public class ApiAirsearch
{
	[JsonPropertyName("api_plane_type")]
	public int ApiPlaneType { get; set; }

	[JsonPropertyName("api_result")]
	public int ApiResult { get; set; }
}
