using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Core.Types.Extensions;
using ElectronicObserver.Core.Types.Attacks;

namespace ElectronicObserver.Core.Types.Attacks.Specials;

public record QueenElizabethSpecialAttack : SpecialAttack
{
	public QueenElizabethSpecialAttack(IFleetData fleet) : base(fleet)
	{
	}

	public override string GetDisplay() => AttackResources.SpecialQueenElizabeth;

	public override bool CanTrigger()
	{
		var ships = Fleet.MembersInstance.ToList();

		if (ships.Count is 0) return false;

		var flagship = ships.First();
		if (flagship is null) return false;
		if (flagship.MasterShip.ShipId is not ShipId.WarspiteKai and not ShipId.ValiantKai) return false;

		if (flagship.HPRate <= 0.5) return false;

		if (Fleet.NumberOfSurfaceShipNotRetreatedNotSunk() < 6) return false;

		var helper = ships[1];
		if (helper is null) return false;
		if (helper.MasterShip.ShipId is not ShipId.WarspiteKai and not ShipId.ValiantKai) return false;
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
				PowerModifier = GetPowerModifier(),
			},
			new()
			{
				ShipIndex = 0,
				AccuracyModifier = 1,
				PowerModifier = GetPowerModifier(),
			},
			new()
			{
				ShipIndex = 1,
				AccuracyModifier = 1,
				PowerModifier = GetPowerModifier(),
			},
		};

	/// <summary>
	/// todo : find source for the mods
	/// </summary>
	/// <returns></returns>
	private double GetPowerModifier()
	{
		var ships = Fleet.MembersInstance.ToList();

		var helper = ships[1];
		if (helper is null) return 1;

		return 1.2 * GetEquipmentPowerModifier(helper);
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
