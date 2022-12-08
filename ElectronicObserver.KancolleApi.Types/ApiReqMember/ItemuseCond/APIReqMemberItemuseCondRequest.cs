namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.ItemuseCond;

public class ApiReqMemberItemuseCondRequest
{
	[JsonPropertyName("api_deck_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiDeckId { get; set; } = default!;

	[JsonPropertyName("api_use_type")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiUseType { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
