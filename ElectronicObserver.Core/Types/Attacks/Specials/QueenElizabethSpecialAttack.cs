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
