namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.ItemuseCond;

public class ApiReqMemberItemuseCondResponse
{
	[JsonPropertyName("api_result")]
	public int ApiResult { get; set; }

	[JsonPropertyName("api_result_msg")]
	public string ApiResultMsg { get; set; } = "";
}
