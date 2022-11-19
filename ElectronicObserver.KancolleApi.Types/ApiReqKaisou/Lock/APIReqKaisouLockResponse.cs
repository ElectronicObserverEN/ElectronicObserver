namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.Lock;

public class ApiReqKaisouLockResponse
{
	[JsonPropertyName("api_locked")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiLocked { get; set; } = default!;
}
