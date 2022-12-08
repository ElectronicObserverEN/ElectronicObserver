namespace ElectronicObserver.KancolleApi.Types.ApiReqHensei.Lock;

public class ApiReqHenseiLockResponse
{
	[JsonPropertyName("api_locked")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiLocked { get; set; } = default!;
}
