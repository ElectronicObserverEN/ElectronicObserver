using System.Collections.Generic;
using System.Linq;
using ElectronicObserverTypes;
using Newtonsoft.Json;

namespace ElectronicObserver.Data.TsunDbSubmission.Battle;

/// <summary>
/// Contains information about the ship
/// </summary>
public class TsunDbBattleShipData
{
	/// <summary>
	/// Ship mst id
	/// </summary>
	[JsonProperty("id")]
	public int Id { get; private set; }

	/// <summary>
	/// Ship level
	/// </summary>
	[JsonProperty("lvl")]
	public int Level { get; private set; }

	/// <summary>
	/// Ship morale pre-battle
	/// </summary>
	[JsonProperty("morale")]
	public int Morale { get; private set; }

	/// <summary>
	/// Ship stats
	/// </summary>
	[JsonProperty("stats")]
	public TsunDbBattleShipStatData ShipStats { get; private set; }

	/// <summary>
	/// Array of mst ids of ship equipment, -1 if slot is empty, extra slot is included
	/// </summary>
	[JsonProperty("equips")]
	public List<int> EquipIds { get; private set; }

	/// <summary>
	/// Array of improvement level of ship equipment, -1 if slot is empty, extra slot is included
	/// </summary>
	[JsonProperty("improvements")]
	public List<int> EquipImprovements { get; private set; }

	/// <summary>
	/// Array of proficiency level of ship equipment, -1 if slot is empty, extra slot is included
	/// </summary>
	[JsonProperty("proficiency")]
	public List<int> EquipProfiency { get; private set; }

	/// <summary>
	/// Array of plane slot count, corresponds to api_slotnum
	/// </summary>
	[JsonProperty("slots")]
	public IList<int> PlaneSlots { get; private set; }

	/// <summary>
	/// Boolean to check if ship has been retreated by FCF
	/// </summary>
	[JsonProperty("flee")]
	public bool Flee { get; private set; }

	/// <summary>
	/// Fuel of the ship pre-battle
	/// </summary>
	[JsonProperty("fuel")]
	public int Fuel { get; private set; }

	/// <summary>
	/// Ammo of the ship pre-battle
	/// </summary>
	[JsonProperty("ammo")]
	public int Ammo { get; private set; }

	public TsunDbBattleShipData(IShipData ship, FleetData fleetData)
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
