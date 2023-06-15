namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Getship;

public class ApiReqKousyouGetshipRequest
{
	[JsonPropertyName("api_kdock_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiKdockId { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
