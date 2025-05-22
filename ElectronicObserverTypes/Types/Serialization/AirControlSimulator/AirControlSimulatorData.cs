using System.Collections.Generic;
using System.Text.Json.Serialization;
using ElectronicObserver.Core.Types.Serialization.DeckBuilder;
using ElectronicObserver.Core.Types.Serialization.FleetAnalysis;

namespace ElectronicObserver.Core.Types.Serialization.AirControlSimulator;

public class AirControlSimulatorData
{
	[JsonPropertyName("predeck")] public DeckBuilderData? Fleet { get; set; }
	[JsonPropertyName("ships")] public IEnumerable<FleetAnalysisShip>? Ships { get; set; }
	[JsonPropertyName("items")] public IEnumerable<FleetAnalysisEquipment>? Equipment { get; set; }
}
