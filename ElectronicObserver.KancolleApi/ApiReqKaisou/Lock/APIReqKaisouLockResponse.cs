namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.Lock;

public class ApiReqKaisouLockResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_locked")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiLocked { get; set; } = default!;
}
