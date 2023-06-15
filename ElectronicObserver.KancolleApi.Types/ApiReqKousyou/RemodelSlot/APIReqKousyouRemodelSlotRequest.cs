namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.RemodelSlot;

public class ApiReqKousyouRemodelSlotRequest
{
	[JsonPropertyName("api_certain_flag")]
	public string ApiCertainFlag { get; set; } = "";

	[JsonPropertyName("api_id")]
	public string ApiId { get; set; } = "";

	[JsonPropertyName("api_slot_id")]
	public string ApiSlotId { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";
}
