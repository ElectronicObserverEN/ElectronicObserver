﻿using System.Text.Json.Nodes;
using ElectronicObserverTypes;

namespace ElectronicObserver.Data.PoiDbSubmission;

public static class Extensions
{
	public static JsonNode MakeShip(this IShipData ship)
	{
		string rawData = ship.RawData.ToString();

		return JsonNode.Parse(rawData)!;
	}
}
