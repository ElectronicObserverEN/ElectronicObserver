namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Unsetslot;

public class ApiGetMemberUnsetslotRequest
{
	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
