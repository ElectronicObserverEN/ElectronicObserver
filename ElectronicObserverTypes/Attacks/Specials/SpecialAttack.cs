using System.Collections.Generic;
using System.Linq;

namespace ElectronicObserverTypes.Attacks.Specials;

public abstract record SpecialAttack
{
	protected IFleetData Fleet { get; set; }

	public virtual bool CanTriggerOnDay => true;
	public virtual bool CanTriggerOnNight => true;

	protected SpecialAttack(IFleetData fleet)
	{
		Fleet = fleet;
	}

	public virtual double GetTriggerRate() => 0;

	public abstract bool CanTrigger();

	public abstract List<SpecialAttackHit> GetAttacks();

	public List<SpecialAttackHit> GetHitsPerShip(int shipIndex)
	{
		return GetAttacks().Where(hit => hit.ShipIndex == shipIndex).ToList();
	}

	public virtual double GetEngagmentModifier(EngagementType engagement) => 1;

	public virtual string GetDisplay() => "???";
}
