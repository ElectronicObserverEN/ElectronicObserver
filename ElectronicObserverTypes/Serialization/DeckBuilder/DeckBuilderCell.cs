using System.Text.Json.Serialization;

namespace ElectronicObserverTypes.Serialization.DeckBuilder;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
// todo: required
public class DeckBuilderCell
{
	[JsonPropertyName("c")] public int CellId { get; set; }
	[JsonPropertyName("pf")] public FormationType PlayerFormation { get; set; }
	[JsonPropertyName("ef")] public FormationType EnemyFormation { get; set; }
	[JsonPropertyName("f1")] public DeckBuilderEnemyFleet Fleet1 { get; set; }
	[JsonPropertyName("f2")] public DeckBuilderEnemyFleet? Fleet2 { get; set; }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
