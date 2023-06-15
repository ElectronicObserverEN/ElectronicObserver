namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.Remodeling;

public class ApiReqKaisouRemodelingRequest
{
	[JsonPropertyName("api_id")]
	public string ApiId { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = default!;
}
