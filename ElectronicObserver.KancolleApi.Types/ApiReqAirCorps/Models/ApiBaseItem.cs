using System.Text.Json.Serialization;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqAirCorps.Models;

public class ApiBaseItem
{
	[JsonPropertyName("api_rid")]
	[Required]
	public int ApiRid { get; set; }

	[JsonPropertyName("api_distance")]
	[Required]
	public ApiDistance ApiDistance { get; set; } = default!;

	[JsonPropertyName("api_plane_info")]
	[Required]
	public List<ApiPlaneInfo> ApiPlaneInfo { get; set; } = new();
}
