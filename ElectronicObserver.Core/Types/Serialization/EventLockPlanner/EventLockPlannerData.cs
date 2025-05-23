using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ElectronicObserver.Core.Types.Serialization.EventLockPlanner;

public class EventLockPlannerData
{
	[JsonPropertyName("Locks")] public List<EventLockPlannerLock> Locks { get; set; } = new();
	[JsonPropertyName("Phases")] public List<EventLockPlannerPhase> Phases { get; set; } = new();
}
