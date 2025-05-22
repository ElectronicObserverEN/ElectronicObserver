using System.Text.Json.Serialization;

namespace ElectronicObserver.Core.Types.Serialization.EventLockPlanner;

public class EventLockPlannerLock
{
	[JsonPropertyName("Id")] public int Id { get; set; }
	[JsonPropertyName("A")] public byte A { get; set; }
	[JsonPropertyName("R")] public byte R { get; set; }
	[JsonPropertyName("G")] public byte G { get; set; }
	[JsonPropertyName("B")] public byte B { get; set; }
	[JsonPropertyName("Name")] public string Name { get; set; } = "";
}
