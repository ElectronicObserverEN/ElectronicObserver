using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqAirCorps.SetPlane;

public class ApiReqAirCorpsSetPlaneResponse
{
	[JsonPropertyName("api_after_bauxite")]
	public int? ApiAfterBauxite { get; set; } = default!;

	[JsonPropertyName("api_distance")]
	[Required]
	public ApiDistance ApiDistance { get; set; } = new();

	[JsonPropertyName("api_plane_info")]
	[Required]
	public List<ApiPlaneInfo> ApiPlaneInfo { get; set; } = new();
}
