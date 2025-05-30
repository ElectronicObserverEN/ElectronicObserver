﻿using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ElectronicObserver.Core.Types.Serialization.DeckBuilder;

public class DeckBuilderEnemyFleet
{
	[JsonPropertyName("name")] public string? Name { get; set; }
	[JsonPropertyName("s")] public List<DeckBuilderEnemyShip> Ships { get; set; } = new();
}
