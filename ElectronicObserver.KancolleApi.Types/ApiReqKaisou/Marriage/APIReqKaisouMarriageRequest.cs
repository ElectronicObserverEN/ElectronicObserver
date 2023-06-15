namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.Marriage;

public class ApiReqKaisouMarriageRequest
{
	[JsonPropertyName("api_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiId { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
