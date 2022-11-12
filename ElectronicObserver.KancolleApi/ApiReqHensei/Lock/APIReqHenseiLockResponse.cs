namespace ElectronicObserver.KancolleApi.Types.ApiReqHensei.Lock;

public class ApiReqHenseiLockResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_locked")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiLocked { get; set; } = default!;
}
