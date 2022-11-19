namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Useitem;

public class ApiGetMemberUseitemResponse
{
	[JsonPropertyName("api_count")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiCount { get; set; } = default!;

	[JsonPropertyName("api_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;
}
