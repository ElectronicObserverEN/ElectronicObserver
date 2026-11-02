using System.Text.Json;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks;

public sealed class UnknownTaskModel(JsonElement json) : ITaskModel
{
	public JsonElement Json { get; } = json;
	public int Count { get; set; } = GetCount(json);

	private static int GetCount(JsonElement json) => json.TryGetProperty(nameof(Count), out JsonElement count) switch
	{
		true when count.ValueKind is JsonValueKind.Number && count.TryGetInt32(out int value) => value,
		_ => 0,
	};
}
