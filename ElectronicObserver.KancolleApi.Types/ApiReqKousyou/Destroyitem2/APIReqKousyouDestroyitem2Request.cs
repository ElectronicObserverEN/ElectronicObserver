namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Destroyitem2;

public class ApiReqKousyouDestroyitem2Request
{
	[JsonPropertyName("api_slotitem_ids")]
	public string ApiSlotitemIds { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = default!;
}
