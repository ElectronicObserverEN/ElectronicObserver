namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.Updatecomment;

public class ApiReqMemberUpdatecommentRequest
{
	[JsonPropertyName("api_token")]
	[Required(AllowEmptyStrings = true)]
	public string ApiToken { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;

	[JsonPropertyName("api_cmt")]
	[Required(AllowEmptyStrings = true)]
	public string ApiCmt { get; set; } = default!;

	[JsonPropertyName("api_cmt_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiCmtId { get; set; } = default!;
}
