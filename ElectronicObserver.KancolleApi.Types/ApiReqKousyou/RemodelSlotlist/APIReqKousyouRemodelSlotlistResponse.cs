using System.Text.Json.Serialization;

namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.RemodelSlotlist;

public class APIReqKousyouRemodelSlotlistResponse
{
	[JsonPropertyName("api_id")]
	[Required]
	public int ApiId { get; set; }

	[JsonPropertyName("api_slot_id")]
	[Required]
	public int ApiSlotId { get; set; }

	[JsonPropertyName("api_sp_type")]
	[Required]
	public int ApiSpType { get; set; }

	[JsonPropertyName("api_req_fuel")]
	[Required]
	public int ApiReqFuel { get; set; }

	[JsonPropertyName("api_req_bull")]
	[Required]
	public int ApiReqBull { get; set; }

	[JsonPropertyName("api_req_steel")]
	[Required]
	public int ApiReqSteel { get; set; }

	[JsonPropertyName("api_req_bauxite")]
	[Required]
	public int ApiReqBauxite { get; set; }

	[JsonPropertyName("api_req_buildkit")]
	[Required]
	public int ApiReqBuildkit { get; set; }

	[JsonPropertyName("api_req_remodelkit")]
	[Required]
	public int ApiReqRemodelkit { get; set; }

	[JsonPropertyName("api_req_slot_id")]
	[Required]
	public int ApiReqSlotId { get; set; }

	[JsonPropertyName("api_req_slot_num")]
	[Required]
	public int ApiReqSlotNum { get; set; }
}
