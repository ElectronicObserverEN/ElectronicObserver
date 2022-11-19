using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqAirCorps.Supply;

public class ApiReqAirCorpsSupplyResponse
{
	[JsonPropertyName("api_after_bauxite")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiAfterBauxite { get; set; } = default!;

	[JsonPropertyName("api_after_fuel")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiAfterFuel { get; set; } = default!;

	[JsonPropertyName("api_distance")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public ApiDistance ApiDistance { get; set; } = new();

	[JsonPropertyName("api_plane_info")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiPlaneInfo> ApiPlaneInfo { get; set; } = new();
}
