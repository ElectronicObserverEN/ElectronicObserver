namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.Updatecomment;

public class ApiReqMemberUpdatecommentRequest
{
	[JsonPropertyName("api_token")]
	public string ApiToken { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";

	[JsonPropertyName("api_cmt")]
	public string ApiCmt { get; set; } = "";

	[JsonPropertyName("api_cmt_id")]
	public string ApiCmtId { get; set; } = "";
}
