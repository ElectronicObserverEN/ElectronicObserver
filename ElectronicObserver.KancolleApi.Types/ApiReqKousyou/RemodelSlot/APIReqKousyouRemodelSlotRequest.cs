namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.RemodelSlot;

public class ApiReqKousyouRemodelSlotRequest
{
	[JsonPropertyName("api_certain_flag")]
	public string ApiCertainFlag { get; set; } = default!;

	[JsonPropertyName("api_id")]
	public string ApiId { get; set; } = default!;

	[JsonPropertyName("api_slot_id")]
	public string ApiSlotId { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = default!;
}
