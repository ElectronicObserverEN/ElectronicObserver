using ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.RemodelSlot;

public class ApiReqKousyouRemodelSlotResponse
{
	[JsonPropertyName("api_after_material")]
	public List<int> ApiAfterMaterial { get; set; } = new();

	[JsonPropertyName("api_after_slot")]
	public ApiAfterSlot? ApiAfterSlot { get; set; }

	[JsonPropertyName("api_remodel_flag")]
	public int ApiRemodelFlag { get; set; }

	[JsonPropertyName("api_remodel_id")]
	public List<int> ApiRemodelId { get; set; } = new();

	[JsonPropertyName("api_use_slot_id")]
	public List<int>? ApiUseSlotId { get; set; }

	[JsonPropertyName("api_voice_id")]
	public int ApiVoiceId { get; set; }

	[JsonPropertyName("api_voice_ship_id")]
	public int ApiVoiceShipId { get; set; }
}
