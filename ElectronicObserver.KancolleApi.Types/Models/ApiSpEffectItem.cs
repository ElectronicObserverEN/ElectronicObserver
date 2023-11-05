using ElectronicObserverTypes;

namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiSpEffectItem
{
	[JsonPropertyName("api_kind")]
	public SpEffectItemKind ApiKind { get; set; }

	/// <summary>
	/// Firepower.
	/// </summary>
	[JsonPropertyName("api_houg")]
	public int ApiHoug { get; set; }

	/// <summary>
	/// Torpedo.
	/// </summary>
	[JsonPropertyName("api_raig")]
	public int ApiRaig { get; set; }

	/// <summary>
	/// Armor.
	/// </summary>
	[JsonPropertyName("api_souk")]
	public int ApiSouk { get; set; }

	/// <summary>
	/// Evasion.
	/// </summary>
	[JsonPropertyName("api_kaih")]
	public int ApiKaih { get; set; }
}
