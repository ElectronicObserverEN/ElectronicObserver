using System.Text.Json.Serialization;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqAirCorps.Models;

public class ApiBaseItem
{
	[JsonPropertyName("api_rid")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public int ApiRid { get; set; }

	[JsonPropertyName("api_distance")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public ApiDistance ApiDistance { get; set; } = default!;

	[JsonPropertyName("api_plane_info")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiPlaneInfo> ApiPlaneInfo { get; set; } = new();
}
