namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.Powerup;

public class ApiReqKaisouPowerupRequest
{
	[JsonPropertyName("api_id")]
	public string ApiId { get; set; } = default!;

	[JsonPropertyName("api_id_items")]
	public string ApiIdItems { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = default!;
}
