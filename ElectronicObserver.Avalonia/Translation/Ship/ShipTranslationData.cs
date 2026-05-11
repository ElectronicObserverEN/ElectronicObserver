using System.Text.Json.Serialization;

namespace ElectronicObserver.Avalonia.Translation.Ship;

public sealed class ShipTranslationData
{
	[JsonPropertyName("version")]
	public required string Version { get; init; }

	[JsonPropertyName("ship")]
	public required Dictionary<string, string> Ship { get; init; }

	[JsonPropertyName("class")]
	public required Dictionary<string, string> Class { get; init; }

	[JsonPropertyName("suffix")]
	public required Dictionary<string, string> Suffix { get; init; }

	[JsonPropertyName("stype")]
	public required Dictionary<string, string> Stype { get; init; }
}
