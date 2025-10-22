using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.Data.PoiDbSubmission.PoiDbQuestSubmission;

public class PoiDbQuestSubmissionData
{
	[JsonPropertyName("current")]
	public required int CompletedQuestId { get; init; }

	[JsonPropertyName("after")]
	public required List<int> NewQuestIds { get; init; }

	/// <summary>
	/// Should be <see cref="ApiListClass" /> but we're skipping deserialization to avoid data loss.
	/// </summary>
	[JsonPropertyName("detail")]
	public required List<JsonNode> NewQuestData { get; init; }

	[JsonPropertyName("version")]
	public required string Version { get; init; }
}
