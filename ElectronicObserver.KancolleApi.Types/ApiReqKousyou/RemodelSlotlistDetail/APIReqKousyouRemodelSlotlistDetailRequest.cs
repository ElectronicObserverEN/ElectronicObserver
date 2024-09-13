namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.RemodelSlotlistDetail;

public class ApiReqKousyouRemodelSlotlistDetailRequest
{
	[JsonPropertyName("api_id")]
	public int ApiId { get; set; }

	[JsonPropertyName("api_slot_id")]
	public int ApiSlotId { get; set; }

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";
}
