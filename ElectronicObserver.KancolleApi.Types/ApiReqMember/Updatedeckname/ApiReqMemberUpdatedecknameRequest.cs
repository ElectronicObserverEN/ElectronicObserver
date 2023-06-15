namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.Updatedeckname;

public class ApiReqMemberUpdatedecknameRequest
{
	[JsonPropertyName("api_token")]
	public string ApiToken { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = default!;

	[JsonPropertyName("api_deck_id")]
	public string ApiDeckId { get; set; } = default!;

	[JsonPropertyName("api_name")]
	public string ApiName { get; set; } = default!;

	[JsonPropertyName("api_name_id")]
	public string ApiNameId { get; set; } = default!;
}
