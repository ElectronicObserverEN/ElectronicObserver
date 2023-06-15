namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.SetFriendlyRequest;

public class ApiReqMemberSetFriendlyRequestRequest
{
	[JsonPropertyName("api_token")]
	[Required(AllowEmptyStrings = true)]
	public string ApiToken { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;

	[JsonPropertyName("api_request_flag")]
	[Required(AllowEmptyStrings = true)]
	public string ApiRequestFlag { get; set; } = default!;

	[JsonPropertyName("api_request_type")]
	[Required(AllowEmptyStrings = true)]
	public string ApiRequestType { get; set; } = default!;
}
