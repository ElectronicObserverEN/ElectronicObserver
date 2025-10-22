using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Attacks;
using ElectronicObserver.Core.Types.Extensions;

namespace ElectronicObserver.Utility.Data;

public static class ShipDayAttacks
{
	public static IEnumerable<Enum> GetDayAttacks(this IShipData ship)
	{
		IEnumerable<Enum> dayAttacks = new List<Enum>();

		if (ship.IsIseClassK2())
		{
			dayAttacks = dayAttacks.Concat(IseClassSpecialAttacks.Cast<Enum>());
		}

		if (ship.IsSurfaceShip())
		{
			dayAttacks = dayAttacks
				.Concat(SurfaceShipDaySpecialAttacks.Cast<Enum>())
				.Concat(SurfaceShipDayNormalAttacks.Cast<Enum>());
		}

		if (ship.MasterShip.IsAircraftCarrier)
		{
			dayAttacks = dayAttacks
				.Concat(CarrierDaySpecialAttacks.Cast<Enum>())
				.Concat(CarrierDayNormalAttacks.Cast<Enum>());
		}

		return dayAttacks.Where(ship.CanDo);
	}

	private static List<DayAttackKind> IseClassSpecialAttacks => new List<DayAttackKind>
	{
		DayAttackKind.SeaAirMultiAngle,
		DayAttackKind.ZuiunMultiAngle
	};

	private static List<DayAttackKind> SurfaceShipDaySpecialAttacks => new List<DayAttackKind>
	{
		DayAttackKind.CutinMainMain,
		DayAttackKind.CutinMainAP,
		DayAttackKind.CutinMainRadar,
		DayAttackKind.CutinMainSub,
		DayAttackKind.DoubleShelling
	};

	private static List<DayAttackKind> SurfaceShipDayNormalAttacks => new List<DayAttackKind>
	{
		DayAttackKind.Shelling
	};

	private static List<DayAttackKind> CarrierDayNormalAttacks => new List<DayAttackKind>
	{
		DayAttackKind.AirAttack
	};

	private static List<DayAirAttackCutinKind> CarrierDaySpecialAttacks => new List<DayAirAttackCutinKind>
	{
		DayAirAttackCutinKind.FighterBomberAttacker,
		DayAirAttackCutinKind.BomberBomberAttacker,
		DayAirAttackCutinKind.BomberAttacker
	};

	private static bool CanDo(this IShipData ship, Enum attack) => attack switch
	{
		DayAttackKind.SeaAirMultiAngle => ship.HasMainGun() && ship.HasSuisei634(2),
		DayAttackKind.ZuiunMultiAngle => ship.HasMainGun() && ship.HasZuiun(2),

		DayAttackKind.CutinMainMain => ship.HasSeaplane() && ship.HasMainGun(2) && ship.HasApShell(),
		DayAttackKind.CutinMainAP => ship.HasSeaplane() && ship.HasMainGun() && ship.HasSecondaryGun() &&
									 ship.HasApShell(),
		DayAttackKind.CutinMainRadar => ship.HasSeaplane() && ship.HasMainGun() && ship.HasSecondaryGun() &&
										ship.HasRadar(),
		DayAttackKind.CutinMainSub => ship.HasSeaplane() && ship.HasMainGun() && ship.HasSecondaryGun(),
		DayAttackKind.DoubleShelling => ship.HasSeaplane() && ship.HasMainGun(2),

		DayAttackKind.Shelling => ship.IsSurfaceShip(),

		DayAirAttackCutinKind.FighterBomberAttacker => ship.HasFighter() && ship.HasBomber() && ship.HasAttacker(),
		DayAirAttackCutinKind.BomberBomberAttacker => ship.HasBomber(2) && ship.HasAttacker(),
		DayAirAttackCutinKind.BomberAttacker => ship.HasBomber() && ship.HasAttacker(),

		DayAttackKind.AirAttack => ship.HasBomber() || ship.HasAttacker() || ship.HasJetBomber(),

		_ => false
	};
}
