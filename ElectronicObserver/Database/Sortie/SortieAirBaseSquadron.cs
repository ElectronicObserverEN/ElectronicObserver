namespace ElectronicObserver.Database.Sortie;

public class SortieAirBaseSquadron
{
	public int State { get; set; }
	public int Condition { get; set; }
	public SortieEquipmentSlot EquipmentSlot { get; set; } = new();
}
