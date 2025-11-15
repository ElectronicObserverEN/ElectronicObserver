using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Core.Types.Extensions;

namespace ElectronicObserver.Core.Types.Attacks.Specials;

public record RichelieuSpecialAttack : SpecialAttack
{
	public RichelieuSpecialAttack(IFleetData fleet) : base(fleet)
	{
	}

	public override string GetDisplay() => AttackResources.SpecialRichelieu;

	public override bool CanTrigger()
	{
		List<IShipData?> ships = Fleet.MembersInstance.ToList();

		if (ships.Count is 0) return false;

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

	/// <summary>
	/// https://github.com/madonoharu/fleethub/blob/b68f24247f44660ee1d6ef26d6d095cd41391436/crates/fleethub-core/src/attack/fleet_cutin.rs#L828
	/// </summary>
	/// <returns></returns>
	public override double GetTriggerRate()
	{
		List<IShipData?> ships = Fleet.MembersInstance.ToList();

		IShipData? flagship = ships.First();
		if (flagship is null) return 0;

		IShipData? helper = ships[1];
		if (helper is null) return 0;

		double rate = Math.Sqrt(flagship.Level) + Math.Sqrt(helper.Level) + 1.2 * (Math.Sqrt(flagship.LuckTotal) + Math.Sqrt(helper.Level));

		int deuxGunBonus = 0;

		if (flagship.HasDeuxGun())
		{
			deuxGunBonus += 5;
		}

		if (helper.HasDeuxGun())
		{
			deuxGunBonus += 5;
		}

		return (rate + 30 + deuxGunBonus) / 100;
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
