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
		IEnumerable<Enum> dayAttacks = [];

		if (ship.IsIseClassK2())
		{
			dayAttacks = dayAttacks.Concat(IseClassSpecialAttacks());
		}

		if (ship.IsSurfaceShip())
		{
			dayAttacks = dayAttacks
				.Concat(SurfaceShipDaySpecialAttacks())
				.Concat(SurfaceShipDayNormalAttacks());
		}

		if (ship.MasterShip.IsAircraftCarrier)
		{
			dayAttacks = dayAttacks
				.Concat(CarrierDaySpecialAttacks())
				.Concat(CarrierDayNormalAttacks());
		}

		return dayAttacks.Where(ship.CanDo);
	}

	private static IEnumerable<Enum> IseClassSpecialAttacks()
	{
		yield return DayAttackKind.SeaAirMultiAngle;
		yield return DayAttackKind.ZuiunMultiAngle;
	}

	private static IEnumerable<Enum> SurfaceShipDaySpecialAttacks()
	{
		yield return DayAttackKind.CutinMainMain;
		yield return DayAttackKind.CutinMainAP;
		yield return DayAttackKind.CutinMainRadar;
		yield return DayAttackKind.CutinMainSub;
		yield return DayAttackKind.DoubleShelling;
	}

	private static IEnumerable<Enum> SurfaceShipDayNormalAttacks()
	{
		yield return DayAttackKind.Shelling;
	}

	private static IEnumerable<Enum> CarrierDayNormalAttacks()
	{
		yield return DayAttackKind.AirAttack;
	}

	private static IEnumerable<Enum> CarrierDaySpecialAttacks()
	{
		yield return DayAirAttackCutinKind.JetFighterJetBomberJetBomber;
		yield return DayAirAttackCutinKind.JetFighterJetBomber;
		yield return DayAirAttackCutinKind.JetFighterBomberAttacker;
		yield return DayAirAttackCutinKind.FighterBomberAttacker;
		yield return DayAirAttackCutinKind.BomberBomberAttacker;
		yield return DayAirAttackCutinKind.BomberAttacker;
	}

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

		DayAirAttackCutinKind.JetFighterJetBomberJetBomber => ship.HasJetFighter() && ship.HasJetBomber(2),
		DayAirAttackCutinKind.JetFighterJetBomber => ship.HasJetFighter() && ship.HasJetBomber(),
		DayAirAttackCutinKind.JetFighterBomberAttacker => ship.HasJetFighter() && ship.HasBomber() && ship.HasAttacker(),

		DayAirAttackCutinKind.FighterBomberAttacker => ship.HasFighter() && ship.HasBomber() && ship.HasAttacker(),
		DayAirAttackCutinKind.BomberBomberAttacker => ship.HasBomber(2) && ship.HasAttacker(),
		DayAirAttackCutinKind.BomberAttacker => ship.HasBomber() && ship.HasAttacker(),

		DayAttackKind.AirAttack => ship.HasBomber() || ship.HasAttacker() || ship.HasJetBomber(),

		_ => false
	};
}
