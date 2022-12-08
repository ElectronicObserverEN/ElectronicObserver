namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.Lock;

public class ApiReqKaisouLockRequest
{
	[JsonPropertyName("api_slotitem_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiSlotitemId { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
