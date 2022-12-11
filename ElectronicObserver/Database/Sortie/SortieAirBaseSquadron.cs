using System.Text.Json.Serialization;

namespace ElectronicObserver.Database.Sortie;

public class SortieAirBaseSquadron
{
	[JsonPropertyName("State")]
	public int State { get; set; }

	[JsonPropertyName("Condition")]
	public int Condition { get; set; }

	[JsonPropertyName("EquipmentSlot")]
	public SortieEquipmentSlot EquipmentSlot { get; set; } = new();
}
