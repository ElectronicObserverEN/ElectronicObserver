using System.Text.Json.Serialization;

namespace ElectronicObserver.Avalonia.Translation.Equipment;

public class EquipmentData
{
	[JsonPropertyName("version")]
	public required string Version { get; init; }

	[JsonPropertyName("equiptype")]
	public required Dictionary<string, string> EquipmentType { get; init; } = new();

	[JsonPropertyName("equipment")]
	public required Dictionary<string, string> Equipment { get; init; } = new();
}
