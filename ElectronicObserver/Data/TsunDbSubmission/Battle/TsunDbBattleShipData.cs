using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using ElectronicObserverTypes;

namespace ElectronicObserver.Data.TsunDbSubmission.Battle;

/// <summary>
/// Contains information about the ship
/// </summary>
public class TsunDbBattleShipData
{
	/// <summary>
	/// Ship mst id
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; }

	/// <summary>
	/// Ship level
	/// </summary>
	[JsonPropertyName("lvl")]
	public int Level { get; }

	/// <summary>
	/// Ship morale pre-battle
	/// </summary>
	[JsonPropertyName("morale")]
	public int Morale { get; }

	/// <summary>
	/// Ship stats
	/// </summary>
	[JsonPropertyName("stats")]
	public TsunDbBattleShipStatData ShipStats { get; }

	/// <summary>
	/// Array of mst ids of ship equipment, -1 if slot is empty, extra slot is included
	/// </summary>
	[JsonPropertyName("equips")]
	public List<int> EquipIds { get; }

	/// <summary>
	/// Array of improvement level of ship equipment, -1 if slot is empty, extra slot is included
	/// </summary>
	[JsonPropertyName("improvements")]
	public List<int> EquipImprovements { get; }

	/// <summary>
	/// Array of proficiency level of ship equipment, -1 if slot is empty, extra slot is included
	/// </summary>
	[JsonPropertyName("proficiency")]
	public List<int> EquipProfiency { get; }

	/// <summary>
	/// Array of plane slot count, corresponds to api_slotnum
	/// </summary>
	[JsonPropertyName("slots")]
	public IList<int> PlaneSlots { get; }

	/// <summary>
	/// Boolean to check if ship has been retreated by FCF
	/// </summary>
	[JsonPropertyName("flee")]
	public bool Flee { get; }

	/// <summary>
	/// Fuel of the ship pre-battle
	/// </summary>
	[JsonPropertyName("fuel")]
	public int Fuel { get; }

	/// <summary>
	/// Ammo of the ship pre-battle
	/// </summary>
	[JsonPropertyName("ammo")]
	public int Ammo { get; }

	public TsunDbBattleShipData(IShipData ship, IFleetData fleetData)
	{
		Id = ship.ShipID;
		Level = ship.Level;
		Morale = ship.Condition;

		ShipStats = new TsunDbBattleShipStatData(ship);

		EquipIds = ship.AllSlotInstanceMaster.Select(eq => eq != null ? eq.EquipmentID : -1).ToList();
		EquipImprovements = ship.AllSlotInstance.Select(eq => eq != null ? eq.Level : -1).ToList();
		EquipProfiency = ship.AllSlotInstance.Select(eq => eq != null ? eq.AircraftLevel : -1).ToList();
		PlaneSlots = ship.Aircraft;
		Flee = fleetData.EscapedShipList.Contains(ship.ShipID);

		Fuel = ship.Fuel;
		Ammo = ship.Ammo;
	}
}
