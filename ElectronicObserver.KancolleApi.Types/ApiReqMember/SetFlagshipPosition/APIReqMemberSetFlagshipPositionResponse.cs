namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.SetFlagshipPosition;

public class ApiReqMemberSetFlagshipPositionResponse
{
	[JsonPropertyName("api_result")]
	public int ApiResult { get; set; }

	[JsonPropertyName("api_result_msg")]
	public string ApiResultMsg { get; set; } = "";
}
