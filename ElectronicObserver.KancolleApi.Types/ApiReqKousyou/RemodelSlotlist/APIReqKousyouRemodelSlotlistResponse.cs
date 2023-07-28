namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.RemodelSlotlist;

public class APIReqKousyouRemodelSlotlistResponse
{
	[JsonPropertyName("api_id")]
	public int ApiId { get; set; }

	[JsonPropertyName("api_slot_id")]
	public int ApiSlotId { get; set; }

	[JsonPropertyName("api_sp_type")]
	public int ApiSpType { get; set; }

	[JsonPropertyName("api_req_fuel")]
	public int ApiReqFuel { get; set; }

	[JsonPropertyName("api_req_bull")]
	public int ApiReqBull { get; set; }

	[JsonPropertyName("api_req_steel")]
	public int ApiReqSteel { get; set; }

	[JsonPropertyName("api_req_bauxite")]
	public int ApiReqBauxite { get; set; }

	[JsonPropertyName("api_req_buildkit")]
	public int ApiReqBuildkit { get; set; }

	[JsonPropertyName("api_req_remodelkit")]
	public int ApiReqRemodelkit { get; set; }

	[JsonPropertyName("api_req_slot_id")]
	public int ApiReqSlotId { get; set; }

	[JsonPropertyName("api_req_slot_num")]
	public int ApiReqSlotNum { get; set; }
}
