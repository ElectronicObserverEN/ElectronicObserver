namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.SetFlagshipPosition;

public class ApiReqMemberSetFlagshipPositionRequest
{
	[JsonPropertyName("api_position_id")]
	public string ApiPositionId { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = default!;
}
