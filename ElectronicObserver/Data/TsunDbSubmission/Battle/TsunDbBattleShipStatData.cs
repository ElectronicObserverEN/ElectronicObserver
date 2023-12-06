using ElectronicObserverTypes;
using Newtonsoft.Json;

namespace ElectronicObserver.Data.TsunDbSubmission.Battle;

/// <summary>
/// Contains information about the ship
/// </summary>
public class TsunDbBattleShipStatData
{
	/// <summary>
	/// Ship visible firepower
	/// </summary>
	[JsonProperty("fp")]
	public int Firepower { get; private set; }

	/// <summary>
	/// Ship visible torpedo power
	/// </summary>
	[JsonProperty("tp")]
	public int Torpedo { get; private set; }

	/// <summary>
	/// Ship visible anti-air
	/// </summary>
	[JsonProperty("aa")]
	public int AntiAir { get; private set; }

	/// <summary>
	/// Ship visible armor
	/// </summary>
	[JsonProperty("ar")]
	public int Armor { get; private set; }

	/// <summary>
	/// Ship visible evasion
	/// </summary>
	[JsonProperty("ev")]
	public int Evasion { get; private set; }

	/// <summary>
	/// Ship visible Anti-Submarine
	/// </summary>
	[JsonProperty("as")]
	public int ASW { get; private set; }

	/// <summary>
	/// Ship visible line-of-sight
	/// </summary>
	[JsonProperty("ls")]
	public int LOS { get; private set; }

	/// <summary>
	/// Ship visible luck
	/// </summary>
	[JsonProperty("lk")]
	public int Luck { get; private set; }

	public TsunDbBattleShipStatData(IShipData ship)
	{
		Firepower = ship.FirepowerTotal;
		Torpedo = ship.TorpedoTotal;
		AntiAir = ship.AATotal;
		Armor = ship.ArmorTotal;
		Evasion = ship.EvasionTotal;
		ASW = ship.ASWTotal;
		LOS = ship.LOSTotal;
		Luck = ship.LuckTotal;
	}
}
