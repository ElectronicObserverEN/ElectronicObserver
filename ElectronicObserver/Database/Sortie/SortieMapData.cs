using System.Text.Json.Serialization;

namespace ElectronicObserver.Database.Sortie;

public class SortieMapData
{
	[JsonPropertyName("RequiredDefeatedCount")]
	public int RequiredDefeatedCount { get; set; }

	[JsonPropertyName("MapHPCurrent")]
	public int MapHPCurrent { get; set; }

	[JsonPropertyName("MapHPMax")]
	public int MapHPMax { get; set; }
}
