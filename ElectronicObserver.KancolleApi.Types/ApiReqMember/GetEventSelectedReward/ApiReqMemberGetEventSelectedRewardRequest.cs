namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.GetEventSelectedReward;

public class ApiReqMemberGetEventSelectedRewardRequest
{
	[JsonPropertyName("api_token")]
	[Required(AllowEmptyStrings = true)]
	public string ApiToken { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;

	[JsonPropertyName("api_selected_dict[21]")]
	[Required(AllowEmptyStrings = true)]
	public string ApiSelectedDict21 { get; set; } = default!;
}
