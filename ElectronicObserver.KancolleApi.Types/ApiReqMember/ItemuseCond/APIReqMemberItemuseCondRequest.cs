namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.ItemuseCond;

public class ApiReqMemberItemuseCondRequest
{
	[JsonPropertyName("api_deck_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiDeckId { get; set; } = default!;

	[JsonPropertyName("api_use_type")]
	[Required(AllowEmptyStrings = true)]
	public string ApiUseType { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
