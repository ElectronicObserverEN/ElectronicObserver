namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiSupportHourai
{
	[JsonPropertyName("api_cl_list")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiClList { get; set; } = new();

	[JsonPropertyName("api_damage")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<double> ApiDamage { get; set; } = new();

	[JsonPropertyName("api_deck_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiDeckId { get; set; } = default!;

	[JsonPropertyName("api_ship_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiShipId { get; set; } = new();

	[JsonPropertyName("api_undressing_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiUndressingFlag { get; set; } = new();
}
