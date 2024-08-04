using System.Collections.Generic;
using System.Linq;
using ElectronicObserverTypes.Extensions;

namespace ElectronicObserverTypes.Attacks.Specials;

public record RichelieuSpecialAttack : SpecialAttack
{
	public RichelieuSpecialAttack(IFleetData fleet) : base(fleet)
	{
	}

	public override string GetDisplay() => AttackResources.SpecialRichelieu;
	
	public override bool CanTrigger()
	{
		List<IShipData?> ships = Fleet.MembersInstance.ToList();

		if (!ships.Any()) return false;

		IShipData? flagship = ships.First();
		if (flagship is null) return false;
		if (flagship.MasterShip.ShipId is not ShipId.RichelieuKai and not ShipId.RichelieuDeux and not ShipId.JeanBartKai) return false;

		if (flagship.HPRate <= 0.5) return false;

		if (Fleet.NumberOfSurfaceShipNotRetreatedNotSunk() < 6) return false;

		IShipData? helper = ships[1];
		if (helper is null) return false;
		if (helper.MasterShip.ShipId is not ShipId.RichelieuKai and not ShipId.RichelieuDeux and not ShipId.JeanBartKai) return false;
		if (helper.HPRate <= 0.25) return false;

		return true;
	}

	public override List<SpecialAttackHit> GetAttacks()
		=> new()
		{
			new()
			{
				ShipIndex = 0,
				AccuracyModifier = 1,
				PowerModifier = GetFlagshipPowerModifier(),
			},
			new()
			{
				ShipIndex = 0,
				AccuracyModifier = 1,
				PowerModifier = GetFlagshipPowerModifier(),
			},
			new()
			{
				ShipIndex = 1,
				AccuracyModifier = 1,
				PowerModifier = GetHelperPowerModifier(),
			},
		};

	private double GetFlagshipPowerModifier()
	{
		List<IShipData?> ships = Fleet.MembersInstance.ToList();

		IShipData? flagship = ships.First();
		if (flagship is null) return 1;

		double baseRate = 1.24;

		if (flagship.MasterShip.ShipId is ShipId.RichelieuKai or ShipId.RichelieuDeux)
		{
			baseRate = 1.3;
		}
		
		return baseRate * GetEquipmentPowerModifier(flagship);
	}

	private double GetHelperPowerModifier()
	{
		List<IShipData?> ships = Fleet.MembersInstance.ToList();

		IShipData? helper = ships[1];
		if (helper is null) return 1;

		return 1.24 * GetEquipmentPowerModifier(helper);
	}

	private double GetEquipmentPowerModifier(IShipData ship)
	{
		double mod = 1;

		if (ship.HasApShell())
		{
			mod *= 1.35;
		}

		if (ship.HasSurfaceRadar())
		{
			mod *= 1.15;
		}

		return mod;
	}
}
