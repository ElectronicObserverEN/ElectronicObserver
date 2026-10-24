using System.Text.Json.Serialization;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2;

[JsonSerializable(typeof(TrackerModelV2))]
[JsonSerializable(typeof(List<TrackerModelV2>))]
[JsonSerializable(typeof(IEnumerable<TrackerModelV2>))]
[JsonSerializable(typeof(QuestProgressRecordV2))]
[JsonSerializable(typeof(QuestTrackerProgressRecordV2))]
public partial class TrackerJsonContext : JsonSerializerContext;
