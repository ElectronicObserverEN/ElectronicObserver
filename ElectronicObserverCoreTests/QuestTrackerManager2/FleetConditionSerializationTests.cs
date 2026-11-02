using System.Text.Json;
using ElectronicObserver.Avalonia.QuestTrackerManager2;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.Group;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipLevel;
using Xunit;

namespace ElectronicObserverCoreTests.QuestTrackerManager2;

public class FleetConditionSerializationTests
{
	[Fact(DisplayName = "Unknown discriminator round-trips all data")]
	public void FleetConditionSerializationTest1()
	{
		const string json = """
			{"type":9876,"futureData":{"name":"future","values":[1,2,3]}}
			""";

		FleetCondition condition = JsonSerializer.Deserialize(json, TrackerJsonContext.Default.QuestFleetCondition)!;

		Assert.IsType<UnknownFleetCondition>(condition);

		using JsonDocument expected = JsonDocument.Parse(json);
		using JsonDocument actual = JsonDocument.Parse(JsonSerializer.Serialize(condition, TrackerJsonContext.Default.QuestFleetCondition));
		Assert.True(JsonElement.DeepEquals(expected.RootElement, actual.RootElement));
	}

	[Fact(DisplayName = "Unknown nested discriminator round-trips with known condition")]
	public void FleetConditionSerializationTest2()
	{
		const string json = """
			{"type":0,"Operator":0,"Conditions":[{"type":9876,"futureData":{"name":"future"}}]}
			""";

		FleetCondition condition = JsonSerializer.Deserialize(json, TrackerJsonContext.Default.QuestFleetCondition)!;

		GroupCondition group = Assert.IsType<GroupCondition>(condition);
		Assert.IsType<UnknownFleetCondition>(Assert.Single(group.Conditions));

		using JsonDocument expected = JsonDocument.Parse(json);
		using JsonDocument actual = JsonDocument.Parse(JsonSerializer.Serialize(condition, TrackerJsonContext.Default.QuestFleetCondition));
		Assert.True(JsonElement.DeepEquals(expected.RootElement, actual.RootElement));
	}

	[Fact(DisplayName = "Known discriminator uses JSON derived type mapping")]
	public void FleetConditionSerializationTest3()
	{
		const string json = """
			{"type":8,"Level":44}
			""";

		ShipLevelCondition condition = Assert.IsType<ShipLevelCondition>(JsonSerializer.Deserialize(json, TrackerJsonContext.Default.QuestFleetCondition));
		Assert.Equal(44, condition.Level);
	}
}
