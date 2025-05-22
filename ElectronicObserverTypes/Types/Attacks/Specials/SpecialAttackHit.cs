using ElectronicObserver.Core.Types.Attacks;

namespace ElectronicObserver.Core.Types.Attacks.Specials;

public record SpecialAttackHit : Attack
{
	/// <summary>
	/// Ship index in the fleet
	/// </summary>
	public int ShipIndex { get; set; }
}
