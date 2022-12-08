namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Getship;

public class ApiReqKousyouGetshipRequest
{
	[JsonPropertyName("api_kdock_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiKdockId { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
