using System.Text.Json.Serialization;

namespace ElectronicObserver.Data.PoiDbSubmission.PoiDbBattleSubmission;

public class Body
{
	[JsonPropertyName("data")]
	public required Data Data { get; init; }
}
