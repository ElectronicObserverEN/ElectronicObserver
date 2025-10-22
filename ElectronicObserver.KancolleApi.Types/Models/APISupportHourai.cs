using ElectronicObserver.Core.Types.Attacks;

namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiSupportHourai
{
	[JsonPropertyName("api_cl_list")]
	public List<HitType> ApiClList { get; set; } = new();

	[JsonPropertyName("api_damage")]
	public List<double> ApiDamage { get; set; } = new();

	[JsonPropertyName("api_deck_id")]
	public int ApiDeckId { get; set; }

	[JsonPropertyName("api_ship_id")]
	public List<int> ApiShipId { get; set; } = new();

	[JsonPropertyName("api_undressing_flag")]
	public List<int> ApiUndressingFlag { get; set; } = new();
}
