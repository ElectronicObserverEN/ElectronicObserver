namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.RemodelSlotlistDetail;

public class ApiReqKousyouRemodelSlotlistDetailResponse
{
	[JsonPropertyName("api_certain_buildkit")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiCertainBuildkit { get; set; } = default!;

	[JsonPropertyName("api_certain_remodelkit")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiCertainRemodelkit { get; set; } = default!;

	[JsonPropertyName("api_change_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiChangeFlag { get; set; } = default!;

	[JsonPropertyName("api_req_buildkit")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiReqBuildkit { get; set; } = default!;

	[JsonPropertyName("api_req_remodelkit")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiReqRemodelkit { get; set; } = default!;

	[JsonPropertyName("api_req_slot_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiReqSlotId { get; set; } = default!;

	[JsonPropertyName("api_req_slot_num")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiReqSlotNum { get; set; } = default!;
}
