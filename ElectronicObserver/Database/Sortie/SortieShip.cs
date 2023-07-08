using System.Collections.Generic;
using System.Text.Json.Serialization;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserverTypes;

namespace ElectronicObserver.Database.Sortie;

public class SortieShip
{
	[JsonPropertyName("Id")]
	public ShipId Id { get; set; }

	/// <summary>
	/// null for some older data
	/// </summary>
	[JsonPropertyName("DropId")]
	public int? DropId { get; set; }

	[JsonPropertyName("Level")]
	public int Level { get; set; }

	[JsonPropertyName("Condition")]
	public int Condition { get; set; }

	[JsonPropertyName("Kyouka")]
	public List<int> Kyouka { get; set; } = new();

	[JsonPropertyName("Fuel")]
	public int Fuel { get; set; }

	[JsonPropertyName("Ammo")]
	public int Ammo { get; set; }

	[JsonPropertyName("Range")]
	public int Range { get; set; }

	[JsonPropertyName("Speed")]
	public int Speed { get; set; }

	[JsonPropertyName("EquipmentSlots")]
	public List<SortieEquipmentSlot> EquipmentSlots { get; set; } = new();

	/// <summary>
	/// null = expansion slot not available
	/// </summary>
	[JsonPropertyName("ExpansionSlot")]
	public SortieEquipmentSlot? ExpansionSlot { get; set; } = new();

	/// <summary>
	/// null for old data
	/// </summary>
	[JsonPropertyName("SpecialEffectItems")]
	public List<SpecialEffectItem>? SpecialEffectItems { get; set; }
}
