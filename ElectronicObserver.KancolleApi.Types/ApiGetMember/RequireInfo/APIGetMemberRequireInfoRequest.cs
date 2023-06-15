namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.RequireInfo;

public class ApiGetMemberRequireInfoRequest
{
	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
