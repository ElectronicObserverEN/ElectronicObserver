namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Destroyitem2;

public class ApiReqKousyouDestroyitem2Request
{
	[JsonPropertyName("api_slotitem_ids")]
	[Required(AllowEmptyStrings = true)]
	public string ApiSlotitemIds { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
