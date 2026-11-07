using System.Text.Json.Serialization;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks;

[JsonConverter(typeof(TaskModelConverter))]
public interface ITaskModel
{
	int Count { get; set; }
}
