namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.SetOssCondition;

public class ApiReqMemberSetOssConditionResponse
{
	[JsonPropertyName("api_result")]
	public int ApiResult { get; set; } = default!;

	[JsonPropertyName("api_result_msg")]
	public string ApiResultMsg { get; set; } = default!;
}
