using System.Text.Json;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions;

public sealed class UnknownFleetCondition(JsonElement json) : FleetCondition
{
	public JsonElement Json { get; } = json;
}
