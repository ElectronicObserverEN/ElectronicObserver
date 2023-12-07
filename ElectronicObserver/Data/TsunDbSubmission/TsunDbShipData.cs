using System;
using System.Linq;
using System.Text.Json.Serialization;
using ElectronicObserverTypes;

namespace ElectronicObserver.Data.TsunDbSubmission;

public class TsunDbShipData : TsunDbEntity
{
	protected override string Url => throw new NotImplementedException();

	#region Json properties
	[JsonPropertyName("id")]
	public int Id { get; private set; }

	[JsonPropertyName("name")]
	public string Name { get; private set; }

	[JsonPropertyName("shiplock")]
	public int Shiplock { get; private set; }

	[JsonPropertyName("level")]
	public int Level { get; private set; }

	[JsonPropertyName("type")]
	public int Type { get; private set; }

	[JsonPropertyName("speed")]
	public int Speed { get; private set; }

	[JsonPropertyName("flee")]
	public bool Flee { get; set; }

	[JsonPropertyName("equip")]
	public int[] Equip { get; private set; }

	[JsonPropertyName("stars")]
	public int[] Stars { get; private set; }

	[JsonPropertyName("ace")]
	public int[] Ace { get; private set; }

	[JsonPropertyName("exslot")]
	public int Exslot { get; private set; }

	public TsunDbShipData(IShipData ship)
	{
		Id = ship.ShipID;
		Name = ship.MasterShip.Name;
		Shiplock = ship.SallyArea;
		Level = ship.Level;
		Type = (int)ship.MasterShip.ShipType;
		Speed = ship.Speed;

		// Equips
		Equip = ship.SlotInstanceMaster.Select(eq => eq?.EquipmentID ?? -1).ToArray();

		// Stars
		Stars = ship.SlotInstance.Select(eq => eq?.Level ?? -1).ToArray();

		// Ace
		Ace = ship.SlotInstance.Select(eq => eq?.AircraftLevel ?? -1).ToArray();

		// Expension slot
		Exslot = ship is { IsExpansionSlotAvailable: true, ExpansionSlotInstanceMaster: not null } ? ship.ExpansionSlotInstanceMaster.EquipmentID : -1;
	}
	#endregion
}
