namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Practice;

public class ApiGetMemberPracticeRequest
{
	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
