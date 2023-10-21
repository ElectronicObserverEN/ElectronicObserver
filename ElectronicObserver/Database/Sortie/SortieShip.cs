using System.Collections.Generic;
using System.Text.Json.Serialization;
using ElectronicObserverTypes;

namespace ElectronicObserver.Database.Sortie;

public class SortieShip
{
	[JsonPropertyName("Id")]
	public ShipId Id { get; set; }

	/// <summary>
	/// null for older data
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

	/// <summary>
	/// null for older data
	/// </summary>
	[JsonPropertyName("Hp")]
	public int? Hp { get; set; }

	/// <summary>
	/// null for older data
	/// </summary>
	[JsonPropertyName("Armor")]
	public int? Armor { get; set; }

	/// <summary>
	/// null for older data
	/// </summary>
	[JsonPropertyName("Evasion")]
	public int? Evasion { get; set; }

	/// <summary>
	/// null for older data
	/// </summary>
	[JsonPropertyName("Aircraft")]
	public List<int>? Aircraft { get; set; }

	[JsonPropertyName("Range")]
	public int Range { get; set; }

	[JsonPropertyName("Speed")]
	public int Speed { get; set; }

	/// <summary>
	/// null for older data
	/// </summary>
	[JsonPropertyName("Firepower")]
	public int? Firepower { get; set; }

	/// <summary>
	/// null for older data
	/// </summary>
	[JsonPropertyName("Torpedo")]
	public int? Torpedo { get; set; }

	/// <summary>
	/// null for older data
	/// </summary>
	[JsonPropertyName("Aa")]
	public int? Aa { get; set; }

	/// <summary>
	/// null for older data
	/// </summary>
	[JsonPropertyName("Asw")]
	public int? Asw { get; set; }

	/// <summary>
	/// null for older data
	/// </summary>
	[JsonPropertyName("Search")]
	public int? Search { get; set; }

	/// <summary>
	/// null for older data
	/// </summary>
	[JsonPropertyName("Luck")]
	public int? Luck { get; set; }

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
