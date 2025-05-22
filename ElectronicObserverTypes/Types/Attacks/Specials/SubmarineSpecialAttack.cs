using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Core.Types.Attacks;

namespace ElectronicObserver.Core.Types.Attacks.Specials;

public record SubmarineSpecialAttack : SpecialAttack
{
	public SubmarineSpecialAttack(IFleetData fleet) : base(fleet)
	{
	}

	public override string GetDisplay() => GetHelperSubmarineIndexes() switch
	{
	[1, 3] => AttackResources.SpecialSubmarineTender24,
	[1, 2] => AttackResources.SpecialSubmarineTender23,
	[2, 3] => AttackResources.SpecialSubmarineTender34,
		_ => "???",
	};

	public List<int> GetHelperSubmarineIndexes()
	{
		var ships = Fleet.MembersInstance.ToList();

		var validSubmarines = ships
			.Skip(1)
			.Take(3)
			.Where(ship => ship is not null && ship.MasterShip.IsSubmarine && ship.HPRate > 0.5)
			.Cast<IShipData>()
			.ToList();

		return validSubmarines.Count switch
		{
			2 => new() { ships.IndexOf(validSubmarines[0]), ships.IndexOf(validSubmarines[1]) },
			3 => new() { 1, 3 },
			_ => new()
		};
	}

	public override bool CanTrigger()
	{
		var ships = Fleet.MembersInstance.ToList();

		if (!ships.Any()) return false;

		var flagship = ships.First();
		if (flagship is null) return false;

		if (flagship.MasterShip.ShipType is not ShipTypes.SubmarineTender) return false;
		if (flagship.Level < 30) return false;
		if (flagship.HPRate <= 0.25) return false;

		return GetHelperSubmarineIndexes().Any();
	}

	public override List<SpecialAttackHit> GetAttacks()
	{
		var ships = Fleet.MembersInstance.ToList();

		List<SpecialAttackHit> attacks = new()
		{
			new()
			{
				ShipIndex = 0,
				// No modifier cause the tender don't attack
			}
		};

		attacks.AddRange(GetHelperSubmarineIndexes()
			.Select(index => new SpecialAttackHit()
			{
				ShipIndex = index,
				AccuracyModifier = 1,
				PowerModifier = 1.2 + 0.04 * Math.Sqrt(ships[index]?.Level ?? 0)
			}
			));

		return attacks;
	}
}
