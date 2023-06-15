namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.GetPracticeEnemyinfo;

public class ApiReqMemberGetPracticeEnemyinfoRequest
{
	[JsonPropertyName("api_member_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiMemberId { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
