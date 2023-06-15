namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.SetFlagshipPosition;

public class ApiReqMemberSetFlagshipPositionRequest
{
	[JsonPropertyName("api_position_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiPositionId { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
