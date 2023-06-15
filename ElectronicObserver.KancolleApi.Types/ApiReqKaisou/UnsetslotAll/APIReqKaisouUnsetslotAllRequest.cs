namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.UnsetslotAll;

public class ApiReqKaisouUnsetslotAllRequest
{
	[JsonPropertyName("api_id")]
	public string ApiId { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = default!;
}
