namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Payitem;

public class ApiGetMemberPayitemRequest
{
	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;

}
