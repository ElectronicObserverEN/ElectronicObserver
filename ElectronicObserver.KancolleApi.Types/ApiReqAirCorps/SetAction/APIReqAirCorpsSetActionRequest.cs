namespace ElectronicObserver.KancolleApi.Types.ApiReqAirCorps.SetAction;

public class ApiReqAirCorpsSetActionRequest
{
	[JsonPropertyName("api_action_kind")]
	[Required(AllowEmptyStrings = true)]
	public string ApiActionKind { get; set; } = default!;

	[JsonPropertyName("api_area_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiAreaId { get; set; } = default!;

	[JsonPropertyName("api_base_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiBaseId { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
