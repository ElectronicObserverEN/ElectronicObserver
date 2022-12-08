namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiFriendlyInfo
{
	[JsonPropertyName("api_Param")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<List<int>> ApiParam { get; set; } = new();

	[JsonPropertyName("api_Slot")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<List<int>> ApiSlot { get; set; } = new();

	[JsonPropertyName("api_maxhps")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiMaxhps { get; set; } = new();

	[JsonPropertyName("api_nowhps")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiNowhps { get; set; } = new();

	[JsonPropertyName("api_production_type")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiProductionType { get; set; } = default!;

	[JsonPropertyName("api_ship_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiShipId { get; set; } = new();

	[JsonPropertyName("api_ship_lv")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiShipLv { get; set; } = new();

	[JsonPropertyName("api_voice_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiVoiceId { get; set; } = new();

	[JsonPropertyName("api_voice_p_no")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiVoicePNo { get; set; } = new();
}
