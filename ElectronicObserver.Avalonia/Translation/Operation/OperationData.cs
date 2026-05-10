using System.Text.Json.Serialization;

namespace ElectronicObserver.Avalonia.Translation.Operation;

public class OperationData
{
	[JsonPropertyName("version")]
	public required string Version { get; init; }
	
	[JsonPropertyName("map")]
	public required Dictionary<string, string> Map { get; init; }
	
	[JsonPropertyName("fleet")]
	public required Dictionary<string, string> Fleet { get; init; }
}
