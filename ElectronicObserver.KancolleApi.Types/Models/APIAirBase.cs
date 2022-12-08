namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiAirBase
{
	[JsonPropertyName("api_action_kind")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiActionKind { get; set; } = default!;

	[JsonPropertyName("api_area_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiAreaId { get; set; } = default!;

	[JsonPropertyName("api_distance")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public ApiDistance ApiDistance { get; set; } = new();

	[JsonPropertyName("api_name")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiName { get; set; } = default!;

	[JsonPropertyName("api_plane_info")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiPlaneInfo> ApiPlaneInfo { get; set; } = new();

	[JsonPropertyName("api_rid")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiRid { get; set; } = default!;
}
