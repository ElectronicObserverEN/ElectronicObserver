using System.Text.Json.Serialization;

namespace ElectronicObserver.Data.PoiDbSubmission.PoiDbBattleSubmission;

public class PoiDbBattleSubmissionData
{
	[JsonPropertyName("body")]
	public required Body Body { get; init; }
}
