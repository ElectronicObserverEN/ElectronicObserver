using System.Text.Json.Serialization;

namespace ElectronicObserver.Data.PoiDbSubmission.PoiDbQuestSubmission;

public class PoiDbQuestSubmissionData
{
	[JsonPropertyName("form")]
	public required Form Form { get; init; }
}
