using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Core.Types.Extensions;

namespace ElectronicObserver.Core.Types.Attacks.Specials;

public record ColoradoSpecialAttack : SpecialAttack
{
	public ColoradoSpecialAttack(IFleetData fleet) : base(fleet)
	{
	}

	public override string GetDisplay() => AttackResources.SpecialColorado;

	public override bool CanTrigger()
	{
		List<IShipData?> ships = Fleet.MembersInstance.ToList();

		if (!ships.Any()) return false;

		IShipData? flagship = ships.First();
		if (flagship is null) return false;
		if (flagship.MasterShip.ShipClassTyped is not ShipClass.Colorado) return false;

		if (flagship.HPRate <= 0.5) return false;

		if (Fleet.NumberOfSurfaceShipNotRetreatedNotSunk() < 6) return false;

		return MeetsHelperRequirement(ships[1]) && MeetsHelperRequirement(ships[2]);
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
				ShipIndex = 1,
				AccuracyModifier = 1,
				PowerModifier = GetFirstHelperPowerModifier(),
			},
			new()
			{
				ShipIndex = 2,
				AccuracyModifier = 1,
				PowerModifier = GetSecondHelperPowerModifier(),
			},
		};

	private double GetFlagshipPowerModifier()
	{
		List<IShipData?> ships = Fleet.MembersInstance.ToList();

		IShipData? flagship = ships[0];
		if (flagship is null) return 1;

		double mod = 1.5;

		mod *= GetEquipmentPowerModifier(flagship);

		return mod;
	}

	private double GetFirstHelperPowerModifier()
	{
		List<IShipData?> ships = Fleet.MembersInstance.ToList();

		IShipData? firstHelper = ships[1];
		if (firstHelper is null) return 1;

		double mod = 1.3;

		if (firstHelper.IsBigSeven()) mod *= 1.15;
		mod *= GetEquipmentPowerModifier(firstHelper);

		return mod;
	}

	private double GetSecondHelperPowerModifier()
	{
		List<IShipData?> ships = Fleet.MembersInstance.ToList();

		IShipData? secondHelper = ships[2];
		if (secondHelper is null) return 1;

		double mod = 1.3;

		if (secondHelper.IsBigSeven()) mod *= 1.17;
		mod *= GetEquipmentPowerModifier(secondHelper);

		return mod;
	}

	private double GetEquipmentPowerModifier(IShipData ship)
	{
		double mod = 1;

		if (ship.HasApShell()) mod *= 1.35;
		if (ship.HasRadar()) mod *= 1.15;
		if (ship.SlotInstance.Any(item => item?.EquipmentId is EquipmentId.RadarSmall_SGRadar_LateModel)) mod *= 1.15;

		return mod;
	}

	private bool MeetsHelperRequirement(IShipData? helper)
	{
		if (helper is null) return false;
		if (helper.MasterShip.ShipType is not ShipTypes.Battleship and not ShipTypes.Battlecruiser and not ShipTypes.AviationBattleship) return false;
		if (helper.HPRate <= 0.25) return false;

		return true;
	}
}
