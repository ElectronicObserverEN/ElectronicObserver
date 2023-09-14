using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserverTypes.Extensions;

namespace ElectronicObserverTypes.Attacks.Specials;

public record NelsonSpecialAttack : SpecialAttack
{
	public NelsonSpecialAttack(IFleetData fleet) : base(fleet)
	{
	}

	public override bool CanTrigger()
	{
		List<IShipData?> ships = Fleet.MembersInstance.ToList();

		if (!ships.Any()) return false;

		IShipData? flagship = ships.First();
		if (flagship is null) return false;
		if (flagship.MasterShip.ShipClassTyped is not ShipClass.Nelson) return false;

		if (flagship.HPRate <= 0.5) return false;

		if (Fleet.NumberOfSurfaceShipNotRetreatedNotSunk() < 6) return false;

		return MeetsHelperRequirement(ships[2]) && MeetsHelperRequirement(ships[4]); 
	}

	public override double GetTriggerRate()
	{
		List<IShipData?> ships = Fleet.MembersInstance.ToList();

		IShipData? flagship = ships.First();
		if (flagship is null) return 0;

		IShipData? firstHelper = ships[2];
		if (firstHelper is null) return 0;

		IShipData? secondHelper = ships[4];
		if (secondHelper is null) return 0;

		// https://twitter.com/dewydrops/status/1181520911444271105?s=20
		return (Math.Sqrt(flagship.Level) + Math.Sqrt(firstHelper.Level) + Math.Sqrt(secondHelper.Level) + flagship.LuckTotal * 0.24 + 25) / 100;
	}

	public override List<SpecialAttackHit> GetAttacks()
	{
		return new()
		{
			new()
			{
				ShipIndex = 0,
				AccuracyModifier = 1.05,
				PowerModifier = GetFlagshipPowerModifier(),
			},
			new()
			{
				ShipIndex = 2,
				AccuracyModifier = 1.05,
				PowerModifier = GetHelperPowerModifier(2),
			},
			new()
			{
				ShipIndex = 4,
				AccuracyModifier = 1.05,
				PowerModifier = GetHelperPowerModifier(4),
			},
		};
	}

	private double GetFlagshipPowerModifier()
	{
		return GetNumberOfNelsonClass() switch
		{
			2 => 2.3,
			_ => 2,
		};
	}

	private double GetHelperPowerModifier(int shipIndex)
	{
		List<IShipData?> ships = Fleet.MembersInstance.ToList();

		if (ships[shipIndex]?.MasterShip.ShipClassTyped is ShipClass.Nelson)
		{
			return 2.4;
		}

		return 2;
	}

	private int GetNumberOfNelsonClass()
	{
		List<IShipData?> ships = Fleet.MembersInstance.ToList();

		List<IShipData?> touchShips = new() { ships[0], ships[2], ships[4] };

		return touchShips.Count(ship => ship?.MasterShip.ShipClassTyped is ShipClass.Nelson);
	}

	public override double GetEngagmentModifier(EngagementType engagement) => engagement switch
	{
		EngagementType.TDisadvantage => 1.25,
		_ => 1
	};

	public override string GetDisplay() => AttackResources.SpecialNelson;

	private bool MeetsHelperRequirement(IShipData? helper)
	{
		if (helper is null) return false;
		if (helper.IsAircraftCarrier()) return false;
		if (!helper.IsSurfaceShip()) return false;

		return true;
	}
}
