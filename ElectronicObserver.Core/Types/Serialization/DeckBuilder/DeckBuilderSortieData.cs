using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ElectronicObserver.Core.Types.Serialization.DeckBuilder;

public class DeckBuilderSortieData
{
	[JsonPropertyName("a")] public int MapAreaId { get; set; }
	[JsonPropertyName("i")] public int MapInfoId { get; set; }
	[JsonPropertyName("c")] public List<DeckBuilderCell> Cells { get; set; } = new();
}
