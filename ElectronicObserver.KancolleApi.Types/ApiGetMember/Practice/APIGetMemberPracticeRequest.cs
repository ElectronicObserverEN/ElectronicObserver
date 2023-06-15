namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Practice;

public class ApiGetMemberPracticeRequest
{
	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
