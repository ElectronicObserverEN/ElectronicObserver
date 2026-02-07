using System;
using System.Text.Json.Serialization;

namespace ElectronicObserver.Core.Types.Serialization.Quests;

public record TimeLimitedQuestData
{
	[JsonPropertyName("id")]
	public required int ApiId { get; set; }

	[JsonPropertyName("endTime")]
	public DateTime? EndTime { get; set; }

	[JsonPropertyName("dailyReset")]
	public bool ProgressResetsDaily { get; set; }
}
