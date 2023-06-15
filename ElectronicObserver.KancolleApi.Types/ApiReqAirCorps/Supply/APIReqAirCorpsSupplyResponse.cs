using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqAirCorps.Supply;

public class ApiReqAirCorpsSupplyResponse
{
	[JsonPropertyName("api_after_bauxite")]
	public int ApiAfterBauxite { get; set; } = default!;

	[JsonPropertyName("api_after_fuel")]
	public int ApiAfterFuel { get; set; } = default!;

	[JsonPropertyName("api_distance")]
	[Required]
	public ApiDistance ApiDistance { get; set; } = new();

	[JsonPropertyName("api_plane_info")]
	[Required]
	public List<ApiPlaneInfo> ApiPlaneInfo { get; set; } = new();
}
