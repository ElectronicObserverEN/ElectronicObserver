namespace ElectronicObserver.Database.Sortie;

public class SortieEquipmentSlot
{
	public int AircraftCurrent { get; set; }
	public int AircraftMax { get; set; }
	public SortieEquipment? Equipment { get; set; }
}