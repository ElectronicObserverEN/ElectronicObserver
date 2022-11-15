namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiFriendlyInfo
{
	[System.Text.Json.Serialization.JsonPropertyName("api_Param")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<List<int>> ApiParam { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_Slot")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<List<int>> ApiSlot { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_maxhps")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiMaxhps { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_nowhps")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiNowhps { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_production_type")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiProductionType { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_ship_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiShipId { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_ship_lv")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiShipLv { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_voice_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiVoiceId { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_voice_p_no")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiVoicePNo { get; set; } = new();
}
