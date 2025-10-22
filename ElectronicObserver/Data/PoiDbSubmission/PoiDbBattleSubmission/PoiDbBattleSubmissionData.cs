using System.Text.Json.Serialization;

namespace ElectronicObserver.Data.PoiDbSubmission.PoiDbBattleSubmission;

public class PoiDbBattleSubmissionData
{
	[JsonPropertyName("data")]
	public required Data Data { get; init; }
}
