using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Core.Types.Extensions;

namespace ElectronicObserver.Core.Types.Attacks.Specials;

public record QueenElizabethSpecialAttack : SpecialAttack
{
	public QueenElizabethSpecialAttack(IFleetData fleet) : base(fleet)
	{
	}

	public override string GetDisplay() => AttackResources.SpecialQueenElizabeth;

	public override bool CanTrigger()
	{
		List<IShipData?> ships = Fleet.MembersInstance.ToList();

		if (ships.Count is 0) return false;

		IShipData? flagship = ships.First();
		if (flagship is null) return false;
		if (flagship.MasterShip.ShipId is not ShipId.WarspiteKai and not ShipId.ValiantKai) return false;

		if (flagship.HPRate <= 0.5) return false;

		if (Fleet.NumberOfSurfaceShipNotRetreatedNotSunk() < 6) return false;

		IShipData? helper = ships[1];
		if (helper is null) return false;
		if (helper.MasterShip.ShipId is not ShipId.WarspiteKai and not ShipId.ValiantKai) return false;
		if (helper.HPRate <= 0.25) return false;

		return true;
	}

	/// <summary>
	/// https://github.com/madonoharu/fleethub/blob/b68f24247f44660ee1d6ef26d6d095cd41391436/crates/fleethub-core/src/attack/fleet_cutin.rs#L814
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

		return (rate + 30) / 100;
	}

	public override List<SpecialAttackHit> GetAttacks()
		=> new()
		{
			new()
			{
				ShipIndex = 0,
				AccuracyModifier = 1,
				PowerModifier = GetPowerModifier(0),
			},
			new()
			{
				ShipIndex = 0,
				AccuracyModifier = 1,
				PowerModifier = GetPowerModifier(0),
			},
			new()
			{
				ShipIndex = 1,
				AccuracyModifier = 1,
				PowerModifier = GetPowerModifier(1),
			},
		};

	/// <summary>
	/// todo : find source for the mods
	/// </summary>
	/// <returns></returns>
	private double GetPowerModifier(int shipIndex)
	{
		List<IShipData?> ships = Fleet.MembersInstance.ToList();

		IShipData? ship = ships[shipIndex];
		if (ship is null) return 1;

		return 1.2 * GetEquipmentPowerModifier(ship);
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
