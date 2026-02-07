using System;
using System.Text.Json.Serialization;
using ElectronicObserver.Core.Types.Quests;

namespace ElectronicObserver.Core.Types.Serialization.Quests;

public record QuestMetadata
{
	[JsonPropertyName("id")]
	public required int ApiId { get; set; }

	[JsonPropertyName("endTime")]
	public DateTime? EndTime { get; set; }

	[JsonPropertyName("resetType")]
	public QuestProgressResetType? QuestProgressResetType { get; set; }
}
