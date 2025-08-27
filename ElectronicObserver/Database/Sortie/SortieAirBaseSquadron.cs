using System.Text.Json.Serialization;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Database.Sortie;

public class SortieAirBaseSquadron
{
	[JsonPropertyName("AircraftCurrent")]
	public int? AircraftCurrent { get; set; }

	[JsonPropertyName("State")]
	public int State { get; set; }

	[JsonPropertyName("Condition")]
	public AirBaseCondition Condition { get; set; }

	[JsonPropertyName("EquipmentSlot")]
	public SortieEquipmentSlot EquipmentSlot { get; set; } = new();
}
