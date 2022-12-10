using System.Collections.Generic;
using ElectronicObserverTypes;

namespace ElectronicObserver.Database.Sortie;

public class SortieShip
{
	public ShipId Id { get; set; }
	public int Level { get; set; }
	public int Condition { get; set; }
	public List<int> Kyouka { get; set; } = new();
	public int Fuel { get; set; }
	public int Ammo { get; set; }
	public int Range { get; set; }
	public int Speed { get; set; }
	public List<SortieEquipmentSlot> EquipmentSlots { get; set; } = new();

	/// <summary>
	/// null = expansion slot not available
	/// </summary>
	public SortieEquipmentSlot? ExpansionSlot { get; set; } = new();
}
