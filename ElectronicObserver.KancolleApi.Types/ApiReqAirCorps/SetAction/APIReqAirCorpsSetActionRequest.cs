namespace ElectronicObserver.KancolleApi.Types.ApiReqAirCorps.SetAction;

public class ApiReqAirCorpsSetActionRequest
{
	[JsonPropertyName("api_action_kind")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiActionKind { get; set; } = default!;

	[JsonPropertyName("api_area_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiAreaId { get; set; } = default!;

	[JsonPropertyName("api_base_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiBaseId { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
