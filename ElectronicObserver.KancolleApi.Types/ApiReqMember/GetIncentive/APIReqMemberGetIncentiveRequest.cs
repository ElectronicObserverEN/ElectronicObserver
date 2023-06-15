namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.GetIncentive;

public class ApiReqMemberGetIncentiveRequest
{
	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
