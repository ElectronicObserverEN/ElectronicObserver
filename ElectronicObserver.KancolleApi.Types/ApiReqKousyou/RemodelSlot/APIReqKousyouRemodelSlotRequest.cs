namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.RemodelSlot;

public class ApiReqKousyouRemodelSlotRequest
{
	[JsonPropertyName("api_certain_flag")]
	[Required(AllowEmptyStrings = true)]
	public string ApiCertainFlag { get; set; } = default!;

	[JsonPropertyName("api_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiId { get; set; } = default!;

	[JsonPropertyName("api_slot_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiSlotId { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
