using System.Text.Json.Serialization;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqAirCorps.Models;

public class ApiBaseItem
{
	[JsonPropertyName("api_rid")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public int ApiRid { get; set; }

	[JsonPropertyName("api_distance")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public ApiDistance ApiDistance { get; set; } = default!;

	[JsonPropertyName("api_plane_info")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiPlaneInfo> ApiPlaneInfo { get; set; } = new();
}
