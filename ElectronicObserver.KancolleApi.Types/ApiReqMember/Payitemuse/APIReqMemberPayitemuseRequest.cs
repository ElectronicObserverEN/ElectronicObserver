namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.Payitemuse;

public class ApiReqMemberPayitemuseRequest
{
	[JsonPropertyName("api_force_flag")]
	public string ApiForceFlag { get; set; } = "";

	[JsonPropertyName("api_payitem_id")]
	public string ApiPayitemId { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";
}
