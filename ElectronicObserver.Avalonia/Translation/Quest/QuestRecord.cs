using System.Text.Json.Serialization;

namespace ElectronicObserver.Avalonia.Translation.Quest;

public class QuestRecord
{
	[JsonPropertyName("code")]
	public required string Code { get; init; }

	[JsonPropertyName("name_jp")]
	public required string NameJp { get; init; }

	[JsonPropertyName("name")]
	public required string Name { get; init; }

	[JsonPropertyName("desc_jp")]
	public required string DescriptionJp { get; init; }

	[JsonPropertyName("desc")]
	public required string Description { get; init; }
}
