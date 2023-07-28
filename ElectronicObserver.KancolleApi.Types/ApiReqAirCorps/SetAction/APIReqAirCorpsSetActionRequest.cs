namespace ElectronicObserver.KancolleApi.Types.ApiReqAirCorps.SetAction;

public class ApiReqAirCorpsSetActionRequest
{
	[JsonPropertyName("api_action_kind")]
	public string ApiActionKind { get; set; } = "";

	[JsonPropertyName("api_area_id")]
	public string ApiAreaId { get; set; } = "";

	[JsonPropertyName("api_base_id")]
	public string ApiBaseId { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";
}
