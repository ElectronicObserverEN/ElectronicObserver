namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.RemodelSlotlistDetail;

public class ApiReqKousyouRemodelSlotlistDetailResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_certain_buildkit")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiCertainBuildkit { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_certain_remodelkit")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiCertainRemodelkit { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_change_flag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiChangeFlag { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_req_buildkit")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiReqBuildkit { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_req_remodelkit")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiReqRemodelkit { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_req_slot_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiReqSlotId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_req_slot_num")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiReqSlotNum { get; set; } = default!;
}
