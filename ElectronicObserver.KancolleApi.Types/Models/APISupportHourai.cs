using ElectronicObserverTypes.Attacks;

namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiSupportHourai
{
	[JsonPropertyName("api_cl_list")]
	[Required]
	public List<HitType> ApiClList { get; set; } = new();

	[JsonPropertyName("api_damage")]
	[Required]
	public List<double> ApiDamage { get; set; } = new();

	[JsonPropertyName("api_deck_id")]
	public int ApiDeckId { get; set; } = default!;

	[JsonPropertyName("api_ship_id")]
	[Required]
	public List<int> ApiShipId { get; set; } = new();

	[JsonPropertyName("api_undressing_flag")]
	[Required]
	public List<int> ApiUndressingFlag { get; set; } = new();
}
