using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqAirCorps.SetPlane;

public class ApiReqAirCorpsSetPlaneResponse
{
	[JsonPropertyName("api_after_bauxite")]
	public int? ApiAfterBauxite { get; set; }

	[JsonPropertyName("api_distance")]
	public ApiDistance ApiDistance { get; set; } = new();

	[JsonPropertyName("api_plane_info")]
	public List<ApiPlaneInfo> ApiPlaneInfo { get; set; } = new();
}
