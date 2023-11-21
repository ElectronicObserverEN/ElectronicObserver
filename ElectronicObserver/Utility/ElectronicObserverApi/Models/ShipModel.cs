using System.Text.Json.Serialization;
using ElectronicObserverTypes;

namespace ElectronicObserver.Utility.ElectronicObserverApi.Models;

public class ShipModel
{
	[JsonPropertyName("shipId")] public ShipId ShipId { get; set; }

	[JsonPropertyName("level")] public int Level { get; set; }

	[JsonPropertyName("firepower")] public int Firepower { get; set; }

	[JsonPropertyName("torpedo")] public int Torpedo { get; set; }

	[JsonPropertyName("antiAir")] public int AntiAir { get; set; }

	[JsonPropertyName("armor")] public int Armor { get; set; }

	[JsonPropertyName("evasion")] public int Evasion { get; set; }

	[JsonPropertyName("asw")] public int ASW { get; set; }

	[JsonPropertyName("los")] public int LOS { get; set; }

	[JsonPropertyName("accuracy")] public int Accuracy { get; set; }

	[JsonPropertyName("range")] public int Range { get; set; }
}
