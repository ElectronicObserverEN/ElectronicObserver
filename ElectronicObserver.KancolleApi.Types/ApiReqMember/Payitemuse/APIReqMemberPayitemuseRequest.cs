namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.Payitemuse;

public class ApiReqMemberPayitemuseRequest
{
	[JsonPropertyName("api_force_flag")]
	[Required(AllowEmptyStrings = true)]
	public string ApiForceFlag { get; set; } = default!;

	[JsonPropertyName("api_payitem_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiPayitemId { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
