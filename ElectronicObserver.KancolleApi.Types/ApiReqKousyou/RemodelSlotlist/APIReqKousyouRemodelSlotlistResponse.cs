using System.Text.Json.Serialization;

namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.RemodelSlotlist;

public class APIReqKousyouRemodelSlotlistResponse
{
	[JsonPropertyName("api_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public int ApiId { get; set; }

	[JsonPropertyName("api_slot_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public int ApiSlotId { get; set; }

	[JsonPropertyName("api_sp_type")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public int ApiSpType { get; set; }

	[JsonPropertyName("api_req_fuel")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public int ApiReqFuel { get; set; }

	[JsonPropertyName("api_req_bull")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public int ApiReqBull { get; set; }

	[JsonPropertyName("api_req_steel")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public int ApiReqSteel { get; set; }

	[JsonPropertyName("api_req_bauxite")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public int ApiReqBauxite { get; set; }

	[JsonPropertyName("api_req_buildkit")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public int ApiReqBuildkit { get; set; }

	[JsonPropertyName("api_req_remodelkit")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public int ApiReqRemodelkit { get; set; }

	[JsonPropertyName("api_req_slot_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public int ApiReqSlotId { get; set; }

	[JsonPropertyName("api_req_slot_num")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public int ApiReqSlotNum { get; set; }
}
