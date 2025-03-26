using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using ElectronicObserver.Data.PoiDbSubmission.PoiDbRouteSubmission;
using ElectronicObserverTypes;

namespace ElectronicObserver.Data.PoiDbSubmission;

public static class Extensions
{
	private static DateTimeOffset UnixEpoch { get; } = new(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

	public static long ToUnixTimeMicroseconds(this DateTimeOffset timestamp)
	{
		TimeSpan duration = timestamp - UnixEpoch;
		// There are 10 ticks per microsecond.
		return duration.Ticks / 10;
	}

	public static JsonNode MakeShip(this IShipData ship)
	{
		string rawData = ship.RawData.ToString();

		List<PoiDbRouteEquipment?> poiSlot = ship.SlotInstance
			.Select(MakePoiEquipment)
			.ToList();
		PoiDbRouteEquipment? poiSlotEx = ship.ExpansionSlotInstance.MakePoiEquipment();

		JsonNode shipJson = JsonNode.Parse(rawData)!;
		shipJson["poi_slot"] = JsonSerializer.SerializeToNode(poiSlot);
		shipJson["poi_slot_ex"] = JsonSerializer.SerializeToNode(poiSlotEx);

		return shipJson;
	}

	[return: NotNullIfNotNull(nameof(equipment))]
	public static PoiDbRouteEquipment? MakePoiEquipment(this IEquipmentData? equipment) => equipment switch
	{
		null => null,
		_ => new()
		{
			ApiId = equipment.MasterID.ToString(),
			ApiSlotitemId = equipment.EquipmentID.ToString(),
			ApiLocked = equipment.IsLocked switch
			{
				true => "1",
				false => "0",
			},
			ApiLevel = equipment.Level.ToString(),
			ApiAlv = equipment.AircraftLevel.ToString(),
		},
	};

	public static JsonNode MakeEquipment(this IEquipmentData equipment)
	{
		string rawData = equipment.RawData.ToString();

		return JsonNode.Parse(rawData)!;
	}
}
