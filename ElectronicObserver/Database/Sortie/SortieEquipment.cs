using System.Text.Json.Serialization;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Database.Sortie;

public class SortieEquipment
{
	[JsonPropertyName("Id")]
	public EquipmentId Id { get; set; }

	[JsonPropertyName("Level")]
	public int Level { get; set; }

	[JsonPropertyName("AircraftLevel")]
	public int AircraftLevel { get; set; }
}
