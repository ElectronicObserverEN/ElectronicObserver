namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiAirBase
{
	[System.Text.Json.Serialization.JsonPropertyName("api_action_kind")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiActionKind { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_area_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiAreaId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_distance")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public ApiDistance ApiDistance { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_name")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiName { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_plane_info")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiPlaneInfo> ApiPlaneInfo { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_rid")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiRid { get; set; } = default!;
}
