using System.Text.Json;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks;

public sealed class UnknownTaskModel(JsonElement json) : ITaskModel
{
	public JsonElement Json { get; } = json;
	public int Count { get; set; }
}
