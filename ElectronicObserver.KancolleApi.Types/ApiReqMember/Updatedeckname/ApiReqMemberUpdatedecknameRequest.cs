namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.Updatedeckname;

public class ApiReqMemberUpdatedecknameRequest
{
	[JsonPropertyName("api_token")]
	public string ApiToken { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";

	[JsonPropertyName("api_deck_id")]
	public string ApiDeckId { get; set; } = "";

	[JsonPropertyName("api_name")]
	public string ApiName { get; set; } = "";

	[JsonPropertyName("api_name_id")]
	public string ApiNameId { get; set; } = "";
}
