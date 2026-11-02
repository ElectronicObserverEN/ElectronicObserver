using System.Text.Json.Serialization;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions;

[JsonConverter(typeof(FleetConditionConverter))]
public abstract class FleetCondition;
