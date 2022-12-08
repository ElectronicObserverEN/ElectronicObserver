namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.Payitemuse;

public class ApiReqMemberPayitemuseRequest
{
	[JsonPropertyName("api_force_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiForceFlag { get; set; } = default!;

	[JsonPropertyName("api_payitem_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiPayitemId { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
