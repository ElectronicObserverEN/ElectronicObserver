namespace ElectronicObserverTypes.Attacks.Specials;

public record SpecialAttackHit : Attack
{
	/// <summary>
	/// Ship index in the fleet
	/// </summary>
	public int ShipIndex { get; set; }
}
