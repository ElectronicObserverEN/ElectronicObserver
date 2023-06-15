namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.RemodelSlotlistDetail;

public class ApiReqKousyouRemodelSlotlistDetailResponse
{
	[JsonPropertyName("api_certain_buildkit")]
	public int ApiCertainBuildkit { get; set; } = default!;

	[JsonPropertyName("api_certain_remodelkit")]
	public int ApiCertainRemodelkit { get; set; } = default!;

	[JsonPropertyName("api_change_flag")]
	public int ApiChangeFlag { get; set; } = default!;

	[JsonPropertyName("api_req_buildkit")]
	public int ApiReqBuildkit { get; set; } = default!;

	[JsonPropertyName("api_req_remodelkit")]
	public int ApiReqRemodelkit { get; set; } = default!;

	[JsonPropertyName("api_req_slot_id")]
	public int ApiReqSlotId { get; set; } = default!;

	[JsonPropertyName("api_req_slot_num")]
	public int ApiReqSlotNum { get; set; } = default!;
}
