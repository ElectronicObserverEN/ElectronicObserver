namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Basic;

public class ApiGetMemberBasicRequest
{
	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
