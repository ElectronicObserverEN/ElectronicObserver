using ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.RemodelSlot;

public class ApiReqKousyouRemodelSlotResponse
{
	[JsonPropertyName("api_after_material")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiAfterMaterial { get; set; } = new();

	[JsonPropertyName("api_after_slot")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiAfterSlot? ApiAfterSlot { get; set; } = default!;

	[JsonPropertyName("api_remodel_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiRemodelFlag { get; set; } = default!;

	[JsonPropertyName("api_remodel_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiRemodelId { get; set; } = new();

	[JsonPropertyName("api_use_slot_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiUseSlotId { get; set; } = default!;

	[JsonPropertyName("api_voice_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiVoiceId { get; set; } = default!;

	[JsonPropertyName("api_voice_ship_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiVoiceShipId { get; set; } = default!;
}
