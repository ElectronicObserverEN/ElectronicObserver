using System;
using System.Linq;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Attacks;
using ElectronicObserverTypes.Extensions;

namespace ElectronicObserver.Utility.Data;

public static class NightAttackRate
{
	public static double GetNightAttackRate(this IShipData ship, NightAttack attack, IFleetData fleet)
	{
		if (attack.NightAttackKind is NightAttackKind.DoubleShelling) return 0.99;

		int attackMod = attack.RateModifier;
		if (attackMod == 0) return 0;

		double luckLevelPart = ship.LuckTotal switch
		{
			< 50 => 15 + ship.LuckTotal + 0.75 * Math.Sqrt(ship.Level),
			_ => 65 + Math.Sqrt(ship.LuckTotal - 50) + 0.8 * Math.Sqrt(ship.Level),
		};

		double baseRate = luckLevelPart
			+ FlagshipBonus(fleet, ship)
			+ ship.HpBonus()
			+ ship.SkilledLookoutsBonus(attack)
			+ fleet.SearchlightBonus(attack)
			+ fleet.StarShellBonus(ship, attack);

		return Math.Floor(baseRate) / attackMod;
	}

	private static double FlagshipBonus(IFleetData fleet, IShipData ship) =>
		fleet.MembersWithoutEscaped!.IndexOf(ship) switch
		{
			0 => 15,
			_ => 0,
		};

	private static int HpBonus(this IShipData ship) => ship.HPRate switch
	{
		> 0.5 => 0,
		> 0.25 => 18,
		_ => 0,
	};

	private static int SkilledLookoutsBonus(this IShipData ship, NightAttack attack) => attack switch
	{
		NightZuiunCutinAttack when ship.HasRegularSkilledLookouts() => 8,
		NightZuiunCutinAttack => 0,

		_ when ship is
		{
			MasterShip.ShipType:
			ShipTypes.Destroyer or
			ShipTypes.LightCruiser or
			ShipTypes.TorpedoCruiser,
		} && ship.HasDestroyerSkilledLookouts() => 8,

		_ when ship.HasRegularSkilledLookouts() => 5,

		_ => 0,
	};

	private static int SearchlightBonus(this IFleetData fleet, NightAttack attack) => attack switch
	{
		NightZuiunCutinAttack when fleet.HasSearchlight() => -10,

		_ when fleet.HasSearchlight() => 7,
		_ => 0,
	};

	private static int StarShellBonus(this IFleetData fleet, IShipData ship, NightAttack attack) => attack switch
	{
		NightZuiunCutinAttack when fleet.CanShipBeforeActivateNightZuiunCutIn(ship) => -10,
		NightZuiunCutinAttack when fleet.HasStarShell() => -10,

		_ when fleet.CanShipBeforeActivateNightZuiunCutIn(ship) => 4,
		_ when fleet.HasStarShell() => 4,
		_ => 0,
	};

	private static bool CanShipBeforeActivateNightZuiunCutIn(this IFleetData fleet, IShipData ship)
	{
		int currentIndex = fleet.MembersWithoutEscaped!.IndexOf(ship);

		return fleet.MembersWithoutEscaped
			.Take(currentIndex)
			.Any(s => s?.GetNightAttacks().Any(a => a.NightAttackKind is NightAttackKind.CutinZuiun) ?? false);
	}
}
