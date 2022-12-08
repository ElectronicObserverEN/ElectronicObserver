namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Ndock;

public class ApiGetMemberNdockRequest
{
	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
