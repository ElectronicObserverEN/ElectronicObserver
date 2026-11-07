using System.Text.Json;
using ElectronicObserver.Avalonia.QuestTrackerManager2;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.Exercise;
using Xunit;

namespace ElectronicObserverCoreTests.QuestTrackerManager2;

public class TaskModelSerializationTests
{
	[Fact(DisplayName = "Unknown discriminator round-trips all data")]
	public void TaskModelSerializationTest1()
	{
		const string json = """
			{"type":9876,"Count":2,"futureData":{"name":"future","values":[1,2,3]}}
			""";

		ITaskModel task = JsonSerializer.Deserialize(json, TrackerJsonContext.Default.QuestTaskModel)!;

		Assert.IsType<UnknownTaskModel>(task);
		Assert.Equal(2, task.Count);

		using JsonDocument expected = JsonDocument.Parse(json);
		using JsonDocument actual = JsonDocument.Parse(JsonSerializer.Serialize(task, TrackerJsonContext.Default.QuestTaskModel));
		Assert.True(JsonElement.DeepEquals(expected.RootElement, actual.RootElement));
	}

	[Fact(DisplayName = "Known discriminator deserializes known task")]
	public void TaskModelSerializationTest2()
	{
		const string json = """
			{"type":6,"Rank":4,"Count":3}
			""";

		ExerciseTaskModel task = Assert.IsType<ExerciseTaskModel>(JsonSerializer.Deserialize(json, TrackerJsonContext.Default.QuestTaskModel));
		Assert.Equal(3, task.Count);
	}
}
