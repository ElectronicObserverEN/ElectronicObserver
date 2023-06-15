namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.Remodeling;

public class ApiReqKaisouRemodelingRequest
{
	[JsonPropertyName("api_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiId { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
