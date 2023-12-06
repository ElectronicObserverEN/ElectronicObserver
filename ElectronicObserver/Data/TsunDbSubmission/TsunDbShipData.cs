using System;
using System.Linq;
using ElectronicObserverTypes;
using Newtonsoft.Json;

namespace ElectronicObserver.Data.TsunDbSubmission;

public class TsunDbShipData : TsunDbEntity
{
	protected override string Url => throw new NotImplementedException();

	#region Json properties
	[JsonProperty("id")]
	public int Id { get; private set; }

	[JsonProperty("name")]
	public string Name { get; private set; }

	[JsonProperty("shiplock")]
	public int Shiplock { get; private set; }

	[JsonProperty("level")]
	public int Level { get; private set; }

	[JsonProperty("type")]
	public int Type { get; private set; }

	[JsonProperty("speed")]
	public int Speed { get; private set; }

	[JsonProperty("flee")]
	public bool Flee { get; set; }

	[JsonProperty("equip")]
	public int[] Equip { get; private set; }

	[JsonProperty("stars")]
	public int[] Stars { get; private set; }

	[JsonProperty("ace")]
	public int[] Ace { get; private set; }

	[JsonProperty("exslot")]
	public int Exslot { get; private set; }

	public TsunDbShipData(IShipData ship)
	{
		this.Id = ship.ShipID;
		this.Name = ship.MasterShip.Name;
		this.Shiplock = ship.SallyArea;
		this.Level = ship.Level;
		this.Type = (int)ship.MasterShip.ShipType;
		this.Speed = ship.Speed;

		// --- Equips
		this.Equip = ship.SlotInstanceMaster.Select(eq => eq != null ? eq.EquipmentID : -1).ToArray();

		// --- Stars
		this.Stars = ship.SlotInstance.Select(eq => eq != null ? eq.Level : -1).ToArray();

		// --- Ace
		this.Ace = ship.SlotInstance.Select(eq => eq != null ? eq.AircraftLevel : -1).ToArray();

		// --- Expension slot
		this.Exslot = ship.IsExpansionSlotAvailable && ship.ExpansionSlotInstanceMaster != null ? ship.ExpansionSlotInstanceMaster.EquipmentID : -1;
	}
	#endregion
}
