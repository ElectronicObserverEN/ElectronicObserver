namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.ItemuseCond;

public class ApiReqMemberItemuseCondRequest
{
	[JsonPropertyName("api_deck_id")]
	public string ApiDeckId { get; set; } = default!;

	[JsonPropertyName("api_use_type")]
	public string ApiUseType { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = default!;
}
