using System.Text.Json.Serialization;

namespace ElectronicObserverTypes.Serialization.FleetAnalysis;

public class FleetAnalysisSpecialEffectItem
{
	[JsonPropertyName("api_kind")] public required SpEffectItemKind ApiKind { get; init; }
	[JsonPropertyName("api_houg")] public required int Firepower { get; init; }
	[JsonPropertyName("api_raig")] public required int Torpedo { get; init; }
	[JsonPropertyName("api_souk")] public required int Armor { get; init; }
	[JsonPropertyName("api_kaih")] public required int Evasion { get; init; }
}
