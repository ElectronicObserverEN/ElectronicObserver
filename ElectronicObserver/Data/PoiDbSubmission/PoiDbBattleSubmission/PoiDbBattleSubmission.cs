using System.Text.Json.Serialization;

namespace ElectronicObserver.Data.PoiDbSubmission.PoiDbBattleSubmission;

public class PoiDbBattleSubmission
{
	[JsonPropertyName("body")]
	public required Body Body { get; init; }
}
