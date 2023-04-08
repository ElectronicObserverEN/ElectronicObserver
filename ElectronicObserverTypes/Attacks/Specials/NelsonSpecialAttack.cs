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

	protected override double GetTriggerRate()
	{
		List<IShipData?> ships = Fleet.MembersInstance.ToList();

		if (!ships.Any()) return -1;

		IShipData? flagship = ships.First();
		if (flagship is null) return -1;
		if (flagship.MasterShip.ShipId is not ShipId.Nelson and not ShipId.NelsonKai) return -1;

		if (flagship.HPRate <= 0.5) return -1;

		int availableShipCount = ships.Count(ship => ship?.HPCurrent > 0) - Fleet.EscapedShipList.Count;
		if (availableShipCount < 6) return -1;

		IShipData? firstHelper = ships[2];
		if (firstHelper is null) return -1;
		if (firstHelper.IsAircraftCarrier()) return -1;
		if (!firstHelper.IsSurfaceShip()) return -1;

		IShipData? secondHelper = ships[4];
		if (secondHelper is null) return -1;
		if (secondHelper.IsAircraftCarrier()) return -1;
		if (!secondHelper.IsSurfaceShip()) return -1;

		// https://twitter.com/dewydrops/status/1181520911444271105?s=20
		return (Math.Sqrt(flagship.Level) + Math.Sqrt(firstHelper.Level) + Math.Sqrt(secondHelper.Level) + flagship.LuckTotal * 0.24 + 25) / 100;
	}

	protected override IEnumerable<SpecialAttackHit> GetAttacks()
		=> new List<SpecialAttackHit>()
		{
			new SpecialAttackHit()
			{
				ShipIndex = 0,
				AccuracyModifier = 1,
				PowerModifier = 2,
			},
			new SpecialAttackHit()
			{
				ShipIndex = 2,
				AccuracyModifier = 1,
				PowerModifier = 2,
			},
			new SpecialAttackHit()
			{
				ShipIndex = 4,
				AccuracyModifier = 1,
				PowerModifier = 2,
			},
		};

	public override double GetEngagmentModifier(EngagementType engagement) => engagement switch
	{
		EngagementType.TDisadvantage => 1.25,
		_ => 1
	};

	public override string GetDisplay() => AttackResources.SpecialNelson;
}
