using System.Text.Json.Serialization;

namespace ElectronicObserver.Window.Wpf.SenkaLeaderboard;

public record SenkaEntryModel
{
	[JsonPropertyName("position")]
	public required int Position { get; set; }

	[JsonPropertyName("admiral")]
	public required string AdmiralName { get; set; }

	[JsonPropertyName("senka")]
	public required int Points { get; set; }

	[JsonPropertyName("medal")]
	public required int MedalCount { get; set; }

	[JsonPropertyName("comment")]
	public required string Comment { get; set; }

	[JsonIgnore]
	public required bool IsKnown { get; set; }
}
