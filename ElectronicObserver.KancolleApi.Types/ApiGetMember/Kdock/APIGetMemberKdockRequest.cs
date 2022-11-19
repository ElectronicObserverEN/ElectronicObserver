namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Kdock;

public class ApiGetMemberKdockRequest
{
	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
