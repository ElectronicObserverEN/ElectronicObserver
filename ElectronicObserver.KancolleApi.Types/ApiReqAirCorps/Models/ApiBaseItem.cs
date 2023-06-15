using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqAirCorps.Models;

public class ApiBaseItem
{
	[JsonPropertyName("api_rid")]
	public int ApiRid { get; set; }

	[JsonPropertyName("api_distance")]
	public ApiDistance? ApiDistance { get; set; }

	[JsonPropertyName("api_plane_info")]
	public List<ApiPlaneInfo> ApiPlaneInfo { get; set; } = new();
}
