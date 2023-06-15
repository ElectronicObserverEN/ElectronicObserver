namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.SetFriendlyRequest;

public class ApiReqMemberSetFriendlyRequestRequest
{
	[JsonPropertyName("api_token")]
	public string ApiToken { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";

	[JsonPropertyName("api_request_flag")]
	public string ApiRequestFlag { get; set; } = "";

	[JsonPropertyName("api_request_type")]
	public string ApiRequestType { get; set; } = "";
}
