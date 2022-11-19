using System.Text.Json.Serialization;

namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.RemodelSlotlist;

public class APIReqKousyouRemodelSlotlistResponse
{
	[JsonPropertyName("api_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public int ApiId { get; set; }

	[JsonPropertyName("api_slot_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public int ApiSlotId { get; set; }

	[JsonPropertyName("api_sp_type")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public int ApiSpType { get; set; }

	[JsonPropertyName("api_req_fuel")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public int ApiReqFuel { get; set; }

	[JsonPropertyName("api_req_bull")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public int ApiReqBull { get; set; }

	[JsonPropertyName("api_req_steel")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public int ApiReqSteel { get; set; }

	[JsonPropertyName("api_req_bauxite")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public int ApiReqBauxite { get; set; }

	[JsonPropertyName("api_req_buildkit")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public int ApiReqBuildkit { get; set; }

	[JsonPropertyName("api_req_remodelkit")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public int ApiReqRemodelkit { get; set; }

	[JsonPropertyName("api_req_slot_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public int ApiReqSlotId { get; set; }

	[JsonPropertyName("api_req_slot_num")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public int ApiReqSlotNum { get; set; }
}
