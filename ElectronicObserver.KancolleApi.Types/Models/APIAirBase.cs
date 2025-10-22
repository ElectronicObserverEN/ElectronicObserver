using ElectronicObserver.Core.Types;

namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiAirBase
{
	[JsonPropertyName("api_action_kind")]
	public AirBaseActionKind ApiActionKind { get; set; }

	[JsonPropertyName("api_area_id")]
	public int ApiAreaId { get; set; }

	[JsonPropertyName("api_distance")]
	public ApiDistance ApiDistance { get; set; } = new();

	[JsonPropertyName("api_name")]
	public string ApiName { get; set; } = "";

	[JsonPropertyName("api_plane_info")]
	public List<ApiPlaneInfo> ApiPlaneInfo { get; set; } = new();

	[JsonPropertyName("api_rid")]
	public int ApiRid { get; set; }
}
