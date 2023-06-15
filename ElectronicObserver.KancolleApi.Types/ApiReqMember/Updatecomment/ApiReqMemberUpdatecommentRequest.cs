namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.Updatecomment;

public class ApiReqMemberUpdatecommentRequest
{
	[JsonPropertyName("api_token")]
	public string ApiToken { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = default!;

	[JsonPropertyName("api_cmt")]
	public string ApiCmt { get; set; } = default!;

	[JsonPropertyName("api_cmt_id")]
	public string ApiCmtId { get; set; } = default!;
}
