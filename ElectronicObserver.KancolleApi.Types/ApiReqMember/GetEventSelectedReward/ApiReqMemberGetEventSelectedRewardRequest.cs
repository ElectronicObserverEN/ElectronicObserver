namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.GetEventSelectedReward;

public class ApiReqMemberGetEventSelectedRewardRequest
{
	[JsonPropertyName("api_token")]
	public string ApiToken { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = default!;

	[JsonPropertyName("api_selected_dict[21]")]
	public string ApiSelectedDict21 { get; set; } = default!;
}
