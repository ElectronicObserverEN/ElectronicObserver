using ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.RemodelSlot;

public class ApiReqKousyouRemodelSlotResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_after_material")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiAfterMaterial { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_after_slot")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public ApiAfterSlot? ApiAfterSlot { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_remodel_flag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiRemodelFlag { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_remodel_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiRemodelId { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_use_slot_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<int>? ApiUseSlotId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_voice_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiVoiceId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_voice_ship_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiVoiceShipId { get; set; } = default!;
}
