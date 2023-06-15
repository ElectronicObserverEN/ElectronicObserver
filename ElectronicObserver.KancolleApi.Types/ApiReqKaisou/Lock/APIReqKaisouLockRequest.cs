namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.Lock;

public class ApiReqKaisouLockRequest
{
	[JsonPropertyName("api_slotitem_id")]
	public string ApiSlotitemId { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";
}
