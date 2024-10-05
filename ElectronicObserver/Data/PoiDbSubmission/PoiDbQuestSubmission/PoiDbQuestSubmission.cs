using System.Text.Json.Serialization;

namespace ElectronicObserver.Data.PoiDbSubmission.PoiDbQuestSubmission;

public class PoiDbQuestSubmission
{
	[JsonPropertyName("form")]
	public required Form Form { get; init; }
}
