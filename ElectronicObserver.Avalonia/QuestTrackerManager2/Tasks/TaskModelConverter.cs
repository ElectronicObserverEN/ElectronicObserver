using System.Text.Json;
using System.Text.Json.Serialization;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.BattleNodeId;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.BossKill;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.EquipmentCardTypeScrap;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.EquipmentCategoryScrap;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.EquipmentIconTypeScrap;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.EquipmentScrap;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.Exercise;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.Expedition;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.MapFirstClear;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.NodeReach;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks;

public sealed class TaskModelConverter : JsonConverter<ITaskModel>
{
	private const string TypePropertyName = "type";

	public override ITaskModel Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		using JsonDocument document = JsonDocument.ParseValue(ref reader);
		JsonElement json = document.RootElement.Clone();

		if (json.ValueKind is not JsonValueKind.Object
			|| !json.TryGetProperty(TypePropertyName, out JsonElement discriminator)
			|| !discriminator.TryGetInt32(out int type))
		{
			return new UnknownTaskModel(json);
		}

		return (TaskType)type switch
		{
			TaskType.BossKill => Deserialize<BossKillTaskModel>(json, options),
			TaskType.Expedition => Deserialize<ExpeditionTaskModel>(json, options),
			TaskType.BattleNodeId => Deserialize<BattleNodeIdTaskModel>(json, options),
			TaskType.NodeReach => Deserialize<NodeReachTaskModel>(json, options),
			TaskType.MapFirstClear => Deserialize<MapFirstClearTaskModel>(json, options),
			TaskType.Exercise => Deserialize<ExerciseTaskModel>(json, options),
			TaskType.EquipmentScrap => Deserialize<EquipmentScrapTaskModel>(json, options),
			TaskType.EquipmentCategoryScrap => Deserialize<EquipmentCategoryScrapTaskModel>(json, options),
			TaskType.EquipmentCardTypeScrap => Deserialize<EquipmentCardTypeScrapTaskModel>(json, options),
			TaskType.EquipmentIconTypeScrap => Deserialize<EquipmentIconTypeScrapTaskModel>(json, options),
			_ => new UnknownTaskModel(json),
		};
	}

	public override void Write(Utf8JsonWriter writer, ITaskModel value, JsonSerializerOptions options)
	{
		if (value is UnknownTaskModel unknown)
		{
			unknown.Json.WriteTo(writer);
			return;
		}

		(int type, JsonElement json) = value switch
		{
			BossKillTaskModel task => ((int)TaskType.BossKill, JsonSerializer.SerializeToElement(task, options)),
			ExpeditionTaskModel task => ((int)TaskType.Expedition, JsonSerializer.SerializeToElement(task, options)),
			BattleNodeIdTaskModel task => ((int)TaskType.BattleNodeId, JsonSerializer.SerializeToElement(task, options)),
			NodeReachTaskModel task => ((int)TaskType.NodeReach, JsonSerializer.SerializeToElement(task, options)),
			MapFirstClearTaskModel task => ((int)TaskType.MapFirstClear, JsonSerializer.SerializeToElement(task, options)),
			ExerciseTaskModel task => ((int)TaskType.Exercise, JsonSerializer.SerializeToElement(task, options)),
			EquipmentScrapTaskModel task => ((int)TaskType.EquipmentScrap, JsonSerializer.SerializeToElement(task, options)),
			EquipmentCategoryScrapTaskModel task => ((int)TaskType.EquipmentCategoryScrap, JsonSerializer.SerializeToElement(task, options)),
			EquipmentCardTypeScrapTaskModel task => ((int)TaskType.EquipmentCardTypeScrap, JsonSerializer.SerializeToElement(task, options)),
			EquipmentIconTypeScrapTaskModel task => ((int)TaskType.EquipmentIconTypeScrap, JsonSerializer.SerializeToElement(task, options)),
			_ => SerializeUnknown(value, options)
		};

		writer.WriteStartObject();
		writer.WriteNumber(TypePropertyName, type);

		foreach (JsonProperty property in json.EnumerateObject())
		{
			if (property.NameEquals(TypePropertyName)) continue;

			property.WriteTo(writer);
		}

		writer.WriteEndObject();
	}

	private static (int Type, JsonElement Json) SerializeUnknown(ITaskModel value, JsonSerializerOptions options)
	{
		JsonElement json = JsonSerializer.SerializeToElement(value, value.GetType(), options);

		if (json.ValueKind != JsonValueKind.Object
			|| !json.TryGetProperty(TypePropertyName, out JsonElement discriminator)
			|| !discriminator.TryGetInt32(out int type))
		{
			throw new JsonException($"Unsupported task type {value.GetType()} must serialize an integer '{TypePropertyName}' property.");
		}

		return (type, json);
	}

	private static T Deserialize<T>(JsonElement json, JsonSerializerOptions options)
		where T : ITaskModel
		=> json.Deserialize<T>(options) ?? throw new JsonException($"Could not deserialize task as {typeof(T)}.");
}
