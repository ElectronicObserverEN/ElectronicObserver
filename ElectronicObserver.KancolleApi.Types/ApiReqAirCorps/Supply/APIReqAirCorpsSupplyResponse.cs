using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqAirCorps.Supply;

public class ApiReqAirCorpsSupplyResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_after_bauxite")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiAfterBauxite { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_after_fuel")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiAfterFuel { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_distance")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public ApiDistance ApiDistance { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_plane_info")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiPlaneInfo> ApiPlaneInfo { get; set; } = new();
}
