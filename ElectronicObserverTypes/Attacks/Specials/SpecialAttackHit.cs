namespace ElectronicObserverTypes.Attacks.Specials;

public record SpecialAttackHit
{
	/// <summary>
	/// Ship index in the fleet
	/// </summary>
	public int ShipIndex { get; set; }

	public double PowerModifier { get; init; }

	public double AccuracyModifier { get; init; }
}
