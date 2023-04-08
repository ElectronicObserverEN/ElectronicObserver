namespace ElectronicObserverTypes.Attacks.Specials;

public record SpecialAttackHit
{
	/// <summary>
	/// Ship index in the fleet
	/// </summary>
	public int ShipIndex { get; set; }

	/// <summary>
	/// If it's the first hit of the special attack => 1<br></br>
	/// If it's the second hit of the special attack => 2<br></br>
	/// ...
	/// </summary>
	public int HitNumber { get; set; }

	public double PowerModifier { get; init; }

	public double AccuracyModifier { get; init; }
}
