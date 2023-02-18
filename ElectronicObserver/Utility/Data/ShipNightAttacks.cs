using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Attacks;
using ElectronicObserverTypes.Extensions;

namespace ElectronicObserver.Utility.Data;

public static class ShipNightAttacks
{
	public static IEnumerable<Enum> GetNightAttacks(this IShipData ship)
	{
		IEnumerable<Enum> nightAttacks = new List<Enum>();

		if (ship.MasterShip.IsSubmarine)
		{
			// sub TCI or normal TCI
			IEnumerable<Enum> specialAttack = SubmarineNightSpecialAttacks.Cast<Enum>()
					.Concat(SurfaceShipNightSpecialAttacks.Cast<Enum>())
					.FirstOrDefault(ship.CanDo) switch
			{
				Enum attack => new List<Enum> { attack },
				_ => Enumerable.Empty<Enum>(),
			};

			nightAttacks = nightAttacks
				.Concat(specialAttack)
				.Concat(NightTorpedoAttacks.Cast<Enum>());
		}

		if (ship.IsSurfaceShip())
		{
			if (ship.IsNightZuiunCutInShip())
			{
				nightAttacks = nightAttacks.Concat(ZuiunSpecialAttacks.Cast<Enum>());
			}

			if (ship.IsDestroyer())
			{
				nightAttacks = nightAttacks.Concat(DestroyerSpecialAttacks.Cast<Enum>());
			}

			IEnumerable<Enum> specialAttack =
				SurfaceShipNightSpecialAttacks.Cast<Enum>().FirstOrDefault(ship.CanDo) switch
				{
					NightAttackKind attack => new List<Enum> { attack },
					_ => Enumerable.Empty<Enum>(),
				};

			nightAttacks = nightAttacks
				.Concat(specialAttack)
				.Concat(SurfaceShipNightNormalAttacks.Cast<Enum>());
		}

		if (ship.IsNightCarrier())
		{
			nightAttacks = nightAttacks
				.Concat(CarrierNightSpecialAttacks.Cast<Enum>())
				.Concat(CarrierNightNormalAttacks.Cast<Enum>());
		}
		else if (ship.IsSpecialNightCarrier() || ship.IsArkRoyal() && ship.HasSwordfish())
		{
			nightAttacks = nightAttacks
				.Concat(new List<Enum> { NightAttackKind.DoubleShelling })
				.Concat(new List<Enum> { NightAttackKind.Shelling });
		}

		return nightAttacks.Where(ship.CanDo);
	}

	private static List<NightAttackKind> ZuiunSpecialAttacks => new()
	{
		NightAttackKind.CutinZuiun,
	};

	private static List<NightAttackKind> DestroyerSpecialAttacks => new()
	{
		NightAttackKind.CutinTorpedoRadar,
		//NightAttackKind.CutinTorpedoRadar2,
		NightAttackKind.CutinTorpedoPicket,
		//NightAttackKind.CutinTorpedoPicket2,
		NightAttackKind.CutinTorpedoDestroyerPicket,
		//NightAttackKind.CutinTorpedoDestroyerPicket2,
		NightAttackKind.CutinTorpedoDrum,
		//NightAttackKind.CutinTorpedoDrum2,
	};

	private static List<NightAttackKind> SurfaceShipNightSpecialAttacks => new()
	{
		NightAttackKind.CutinTorpedoTorpedo,
		NightAttackKind.CutinMainMain,
		NightAttackKind.CutinMainSub,
		NightAttackKind.CutinMainTorpedo,
		NightAttackKind.DoubleShelling,
	};

	private static List<CvnciKind> CarrierNightSpecialAttacks => new()
	{
		CvnciKind.FighterFighterAttacker,
		CvnciKind.FighterAttacker,
		CvnciKind.Phototube,
		CvnciKind.FighterOtherOther,
	};

	private static List<NightAttackKind> SurfaceShipNightNormalAttacks => new()
	{
		NightAttackKind.Shelling,
	};

	private static List<NightAttackKind> NightTorpedoAttacks => new()
	{
		NightAttackKind.Torpedo,
	};

	private static List<NightAttackKind> CarrierNightNormalAttacks => new()
	{
		NightAttackKind.AirAttack,
	};

	private static List<NightTorpedoCutinKind> SubmarineNightSpecialAttacks => new()
	{
		NightTorpedoCutinKind.LateModelTorpedoSubmarineEquipment,
		NightTorpedoCutinKind.LateModelTorpedo2,
	};

	private static bool CanDo(this IShipData ship, Enum attack) => attack switch
	{
		NightAttackKind.CutinTorpedoTorpedo => ship.HasTorpedo(2),
		NightAttackKind.CutinMainMain => ship.HasMainGun(3),
		NightAttackKind.CutinMainSub => ship.HasMainGun(2) && ship.HasSecondaryGun(),
		NightAttackKind.CutinMainTorpedo => ship.HasMainGun() && ship.HasTorpedo(),
		NightAttackKind.DoubleShelling => ship.MainGunCount() + ship.SecondaryGunCount() >= 2,

		NightAttackKind.CutinTorpedoRadar => ship.HasMainGun() && ship.HasTorpedo() && ship.HasRadar(),
		NightAttackKind.CutinTorpedoRadar2 => ship.CanDo(NightAttackKind.CutinTorpedoRadar) &&
											  ship.DestroyerCutinTwoHitAvailable(),
		NightAttackKind.CutinTorpedoPicket => ship.HasTorpedo() && ship.HasSkilledLookouts() && ship.HasRadar(),
		NightAttackKind.CutinTorpedoPicket2 => ship.CanDo(NightAttackKind.CutinTorpedoPicket) &&
											   ship.DestroyerCutinTwoHitAvailable(),
		NightAttackKind.CutinTorpedoDestroyerPicket => ship.HasTorpedo(2) && ship.HasDestroyerSkilledLookouts(),
		NightAttackKind.CutinTorpedoDestroyerPicket2 => ship.CanDo(NightAttackKind.CutinTorpedoDestroyerPicket) &&
														ship.DestroyerCutinTwoHitAvailable(),
		NightAttackKind.CutinTorpedoDrum => ship.HasTorpedo() && ship.HasDestroyerSkilledLookouts() &&
											ship.HasDrum(),
		NightAttackKind.CutinTorpedoDrum2 => ship.CanDo(NightAttackKind.CutinTorpedoDrum) &&
											 ship.DestroyerCutinTwoHitAvailable(),

		CvnciKind.FighterFighterAttacker => ship.HasNightFighter(2) && ship.HasNightAttacker(),
		CvnciKind.FighterAttacker => ship.IsNightCarrier() && ship.HasNightFighter() && ship.HasNightAttacker(),
		CvnciKind.Phototube => ship.HasNightPhototoubePlane() &&
							   (ship.HasNightFighter() || ship.HasNightAttacker()),
		CvnciKind.FighterOtherOther => ship.HasNightFighter() && ship.HasNightAircraft(3),

		NightTorpedoCutinKind.LateModelTorpedoSubmarineEquipment => ship.HasLateModelTorp() &&
																	ship.HasSubmarineEquipment(),
		NightTorpedoCutinKind.LateModelTorpedo2 => ship.HasLateModelTorp(2),

		NightAttackKind.CutinZuiun => ship.HasMainGun(2) && ship.HasNightZuiun(),

		NightAttackKind.Shelling => true,
		NightAttackKind.Torpedo => true,
		NightAttackKind.AirAttack => ship.HasNightAircraft(),

		_ => false,
	};

	public static bool DestroyerCutinTwoHitAvailable(this IShipData ship) => ship.Level >= 80;
}
