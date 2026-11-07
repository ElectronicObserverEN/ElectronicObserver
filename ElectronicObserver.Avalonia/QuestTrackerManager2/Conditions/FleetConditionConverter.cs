using System.Text.Json;
using System.Text.Json.Serialization;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.Group;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.Not;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.Reference;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipClass;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipCount;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipId;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipLevel;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipNationality;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipPosition;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipType;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions;

public sealed class FleetConditionConverter : JsonConverter<FleetCondition>
{
	private const string TypePropertyName = "type";

	public override FleetCondition Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		using JsonDocument document = JsonDocument.ParseValue(ref reader);
		JsonElement json = document.RootElement.Clone();

		if (json.ValueKind is not JsonValueKind.Object
			|| !json.TryGetProperty(TypePropertyName, out JsonElement discriminator)
			|| !discriminator.TryGetInt32(out int type))
		{
			return new UnknownFleetCondition(json);
		}

		return (ConditionType)type switch
		{
			ConditionType.Group => Deserialize<GroupCondition>(json, options),
			ConditionType.Not => Deserialize<NotCondition>(json, options),
			ConditionType.Reference => Deserialize<ReferenceCondition>(json, options),
			ConditionType.ShipId => Deserialize<ShipIdCondition>(json, options),
			ConditionType.ShipType => Deserialize<ShipTypeCondition>(json, options),
			ConditionType.ShipClass => Deserialize<ShipClassCondition>(json, options),
			ConditionType.ShipPosition => Deserialize<ShipPositionCondition>(json, options),
			ConditionType.ShipCount => Deserialize<ShipCountCondition>(json, options),
			ConditionType.ShipLevel => Deserialize<ShipLevelCondition>(json, options),
			ConditionType.ShipNationality => Deserialize<ShipNationalityCondition>(json, options),
			_ => new UnknownFleetCondition(json),
		};
	}

	public override void Write(Utf8JsonWriter writer, FleetCondition value, JsonSerializerOptions options)
	{
		if (value is UnknownFleetCondition unknown)
		{
			unknown.Json.WriteTo(writer);
			return;
		}

		(int type, JsonElement json) = value switch
		{
			GroupCondition condition => ((int)ConditionType.Group, JsonSerializer.SerializeToElement(condition, options)),
			NotCondition condition => ((int)ConditionType.Not, JsonSerializer.SerializeToElement(condition, options)),
			ReferenceCondition condition => ((int)ConditionType.Reference, JsonSerializer.SerializeToElement(condition, options)),
			ShipIdCondition condition => ((int)ConditionType.ShipId, JsonSerializer.SerializeToElement(condition, options)),
			ShipTypeCondition condition => ((int)ConditionType.ShipType, JsonSerializer.SerializeToElement(condition, options)),
			ShipClassCondition condition => ((int)ConditionType.ShipClass, JsonSerializer.SerializeToElement(condition, options)),
			ShipPositionCondition condition => ((int)ConditionType.ShipPosition, JsonSerializer.SerializeToElement(condition, options)),
			ShipCountCondition condition => ((int)ConditionType.ShipCount, JsonSerializer.SerializeToElement(condition, options)),
			ShipLevelCondition condition => ((int)ConditionType.ShipLevel, JsonSerializer.SerializeToElement(condition, options)),
			ShipNationalityCondition condition => ((int)ConditionType.ShipNationality, JsonSerializer.SerializeToElement(condition, options)),
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

	private static (int Type, JsonElement Json) SerializeUnknown(FleetCondition value, JsonSerializerOptions options)
	{
		JsonElement json = JsonSerializer.SerializeToElement(value, value.GetType(), options);

		if (json.ValueKind != JsonValueKind.Object
			|| !json.TryGetProperty(TypePropertyName, out JsonElement discriminator)
			|| !discriminator.TryGetInt32(out int type))
		{
			throw new JsonException($"Unsupported fleet condition type {value.GetType()} must serialize an integer 'type' property.");
		}

		return (type, json);
	}

	private static T Deserialize<T>(JsonElement json, JsonSerializerOptions options)
		where T : FleetCondition
		=> json.Deserialize<T>(options) ?? throw new JsonException($"Could not deserialize fleet condition as {typeof(T)}.");
}
