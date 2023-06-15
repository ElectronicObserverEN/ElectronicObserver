namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.SetFlagshipPosition;

public class ApiReqMemberSetFlagshipPositionRequest
{
	[JsonPropertyName("api_position_id")]
	public string ApiPositionId { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";
}
