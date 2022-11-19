namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiBasic
{
	[JsonPropertyName("api_firstflag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiFirstflag { get; set; } = default!;

	[JsonPropertyName("api_member_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiMemberId { get; set; } = default!;
}
