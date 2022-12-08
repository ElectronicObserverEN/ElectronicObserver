namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Mission;

public class ApiGetMemberMissionRequest
{
	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
