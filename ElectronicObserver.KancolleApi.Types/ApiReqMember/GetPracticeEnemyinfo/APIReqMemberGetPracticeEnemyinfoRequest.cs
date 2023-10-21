namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.GetPracticeEnemyinfo;

public class ApiReqMemberGetPracticeEnemyinfoRequest
{
	[JsonPropertyName("api_member_id")]
	public string ApiMemberId { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";
}
