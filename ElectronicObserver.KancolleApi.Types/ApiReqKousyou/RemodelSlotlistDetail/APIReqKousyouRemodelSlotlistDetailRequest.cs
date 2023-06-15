namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.RemodelSlotlistDetail;

public class ApiReqKousyouRemodelSlotlistDetailRequest
{
	[JsonPropertyName("api_id")]
	public string ApiId { get; set; } = default!;

	[JsonPropertyName("api_slot_id")]
	public string ApiSlotId { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = default!;
}
