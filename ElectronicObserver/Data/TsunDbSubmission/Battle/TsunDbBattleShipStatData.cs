using System.Text.Json.Serialization;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Data.TsunDbSubmission.Battle;

/// <summary>
/// Contains information about the ship
/// </summary>
public class TsunDbBattleShipStatData(IShipData ship)
{
	/// <summary>
	/// Ship visible firepower
	/// </summary>
	[JsonPropertyName("fp")]
	public int Firepower => ship.FirepowerTotal;

	/// <summary>
	/// Ship visible torpedo power
	/// </summary>
	[JsonPropertyName("tp")]
	public int Torpedo => ship.TorpedoTotal;

	/// <summary>
	/// Ship visible anti-air
	/// </summary>
	[JsonPropertyName("aa")]
	public int AntiAir => ship.AATotal;

	/// <summary>
	/// Ship visible armor
	/// </summary>
	[JsonPropertyName("ar")]
	public int Armor => ship.ArmorTotal;

	/// <summary>
	/// Ship visible evasion
	/// </summary>
	[JsonPropertyName("ev")]
	public int Evasion => ship.EvasionTotal;

	/// <summary>
	/// Ship visible Anti-Submarine
	/// </summary>
	[JsonPropertyName("as")]
	public int ASW => ship.ASWTotal;

	/// <summary>
	/// Ship visible line-of-sight
	/// </summary>
	[JsonPropertyName("ls")]
	public int LOS => ship.LOSTotal;

	/// <summary>
	/// Ship visible luck
	/// </summary>
	[JsonPropertyName("lk")]
	public int Luck => ship.LuckTotal;
}
