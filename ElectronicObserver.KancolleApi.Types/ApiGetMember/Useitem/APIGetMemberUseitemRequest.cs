namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Useitem;

public class ApiGetMemberUseitemRequest
{
	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
