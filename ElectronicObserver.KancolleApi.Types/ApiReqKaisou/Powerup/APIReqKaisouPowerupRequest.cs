namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.Powerup;

public class ApiReqKaisouPowerupRequest
{
	[JsonPropertyName("api_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiId { get; set; } = default!;

	[JsonPropertyName("api_id_items")]
	[Required(AllowEmptyStrings = true)]
	public string ApiIdItems { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
