using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Attacks;
using ElectronicObserver.Core.Types.Extensions;
using ElectronicObserver.Core.Types.Extensions;

namespace ElectronicObserver.Utility.Data;

public static class ShipNightAttacks
{
	public static IEnumerable<NightAttack> GetNightAttacks(this IShipData ship)
	{
		IEnumerable<NightAttack> nightAttacks = new List<NightAttack>();

		// those Souya forms can't attack at night, there might be others
		if (ship.MasterShip.ShipId is ShipId.Souya645 or ShipId.Souya650) return nightAttacks;
		// Kai version of Luigi and Cappellini can't attack at night (cause base FP and torp is 0 ?)
		if (ship.MasterShip.ShipId is ShipId.LuigiTorelliKai or ShipId.CCappelliniKai) return nightAttacks;

		if (ship.MasterShip.IsSubmarine)
		{
			// sub TCI or normal TCI
			IEnumerable<NightAttack> specialAttack = SubmarineNightSpecialAttacks()
				.Concat(SurfaceShipNightSpecialAttacks())
				.FirstOrDefault(ship.CanDo) switch
				{
					NightAttack attack => new List<NightAttack> { attack },
					_ => Enumerable.Empty<NightAttack>(),
				};

			nightAttacks = nightAttacks
				.Concat(specialAttack)
				.Concat(NightTorpedoAttacks());
		}

		if (ship.IsSurfaceShip())
		{
			if (ship.IsNightZuiunCutInShip())
			{
				nightAttacks = nightAttacks.Concat(ZuiunSpecialAttacks());
			}

			if (ship.IsDestroyer())
			{
				nightAttacks = nightAttacks.Concat(DestroyerSpecialAttacks());
			}

			IEnumerable<NightAttack> specialAttack =
				SurfaceShipNightSpecialAttacks().FirstOrDefault(ship.CanDo) switch
				{
					NightAttack attack => new List<NightAttack> { attack },
					_ => Enumerable.Empty<NightAttack>(),
				};

			nightAttacks = nightAttacks
				.Concat(specialAttack)
				.Concat(SurfaceShipNightNormalAttacks());
		}

		if (ship.IsNightCarrier())
		{
			nightAttacks = nightAttacks
				.Concat(CarrierNightSpecialAttacks())
				.Concat(CarrierNightNormalAttacks());
		}
		else if (ship.IsSpecialNightCarrier() || ship.IsArkRoyal() && ship.HasSwordfish())
		{
			nightAttacks = nightAttacks
				.Concat(new List<NightAttack> { NightAttack.DoubleShelling })
				.Concat(new List<NightAttack> { NightAttack.Shelling });
		}

		return nightAttacks.Where(ship.CanDo);
	}

	private static IEnumerable<NightAttack> ZuiunSpecialAttacks()
	{
		yield return NightZuiunCutinAttack.NightZuiunCutinZuiunZuiunRadar;
		yield return NightZuiunCutinAttack.NightZuiunCutinZuiunZuiun;
		yield return NightZuiunCutinAttack.NightZuiunCutinZuiunRadar;
		yield return NightZuiunCutinAttack.NightZuiunCutinZuiun;
	}

	private static IEnumerable<NightAttack> DestroyerSpecialAttacks()
	{
		yield return NightAttack.CutinTorpedoRadar;
		// yield return NightAttack.CutinTorpedoRadar2;
		yield return NightAttack.CutinTorpedoPicket;
		// yield return NightAttack.CutinTorpedoPicket2;
		yield return NightAttack.CutinTorpedoDestroyerPicket;
		// yield return NightAttack.CutinTorpedoDestroyerPicket2;
		yield return NightAttack.CutinTorpedoDrum;
		// yield return NightAttack.CutinTorpedoDrum2;
	}

	private static IEnumerable<NightAttack> SurfaceShipNightSpecialAttacks()
	{
		yield return NightAttack.CutinTorpedoTorpedo;
		yield return NightAttack.CutinMainMain;
		yield return NightAttack.CutinMainSub;
		yield return NightAttack.CutinMainTorpedo;
		yield return NightAttack.DoubleShelling;
	}

	private static IEnumerable<NightAttack> CarrierNightSpecialAttacks()
	{
		yield return CvnciAttack.CutinAirAttackFighterFighterAttacker;
		yield return CvnciAttack.CutinAirAttackFighterAttacker;
		yield return CvnciAttack.CutinAirAttackPhototube;
		yield return CvnciAttack.CutinAirAttackFighterOtherOther;
	}

	private static IEnumerable<NightAttack> SurfaceShipNightNormalAttacks()
	{
		yield return NightAttack.Shelling;
	}

	private static IEnumerable<NightAttack> NightTorpedoAttacks()
	{
		yield return NightAttack.Torpedo;
	}

	private static IEnumerable<NightAttack> CarrierNightNormalAttacks()
	{
		yield return NightAttack.AirAttack;
	}

	private static IEnumerable<NightAttack> SubmarineNightSpecialAttacks()
	{
		yield return SubmarineTorpedoCutinAttack.CutinTorpedoTorpedoLateModelTorpedoSubmarineEquipment;
		yield return SubmarineTorpedoCutinAttack.CutinTorpedoTorpedoLateModelTorpedo2;
	}

	private static bool CanDo(this IShipData ship, NightAttack attack) => attack switch
	{
		CvnciAttack { CvnciKind: CvnciKind.FighterFighterAttacker } => ship.HasNightFighter(2) && ship.HasNightAttacker(),
		CvnciAttack { CvnciKind: CvnciKind.FighterAttacker } => ship.IsNightCarrier() && ship.HasNightFighter() && ship.HasNightAttacker(),
		CvnciAttack { CvnciKind: CvnciKind.Phototube } => ship.HasNightPhototubePlane() && (ship.HasNightFighter() || ship.HasNightAttacker()),
		CvnciAttack { CvnciKind: CvnciKind.FighterOtherOther } => ship.HasNightFighter() && ship.HasNightAircraft(3),

		SubmarineTorpedoCutinAttack { NightTorpedoCutinKind: NightTorpedoCutinKind.LateModelTorpedoSubmarineEquipment } => ship.HasLateModelTorpedo() && ship.HasSubmarineEquipment(),
		SubmarineTorpedoCutinAttack { NightTorpedoCutinKind: NightTorpedoCutinKind.LateModelTorpedo2 } => ship.HasLateModelTorpedo(2),

		NightZuiunCutinAttack { NightZuiunCutinKind: NightZuiunCutinKind.ZuiunZuiunRadar } => ship.HasMainGun(2) && ship.HasNightZuiun(2) && ship.HasSurfaceRadar(),
		NightZuiunCutinAttack { NightZuiunCutinKind: NightZuiunCutinKind.ZuiunZuiun } => ship.HasMainGun(2) && ship.HasNightZuiun(2),
		NightZuiunCutinAttack { NightZuiunCutinKind: NightZuiunCutinKind.ZuiunRadar } => ship.HasMainGun(2) && ship.HasNightZuiun() && ship.HasSurfaceRadar(),
		NightZuiunCutinAttack { NightZuiunCutinKind: NightZuiunCutinKind.Zuiun } => ship.HasMainGun(2) && ship.HasNightZuiun(),

		{ NightAttackKind: NightAttackKind.CutinTorpedoTorpedo } => ship.HasTorpedo(2),
		{ NightAttackKind: NightAttackKind.CutinMainMain } => ship.HasMainGun(3),
		{ NightAttackKind: NightAttackKind.CutinMainSub } => ship.HasMainGun(2) && ship.HasSecondaryGun(),
		{ NightAttackKind: NightAttackKind.CutinMainTorpedo } => ship.HasMainGun() && ship.HasTorpedo(),
		{ NightAttackKind: NightAttackKind.DoubleShelling } => ship.MainGunCount() + ship.SecondaryGunCount() >= 2,

		{ NightAttackKind: NightAttackKind.CutinTorpedoRadar } => ship.HasMainGun() && ship.HasTorpedo() && ship.HasRadar(),
		{ NightAttackKind: NightAttackKind.CutinTorpedoRadar2 } => ship.CanDo(NightAttack.CutinTorpedoRadar) && ship.DestroyerCutinTwoHitAvailable(),

		{ NightAttackKind: NightAttackKind.CutinTorpedoPicket } => ship.HasTorpedo() && ship.HasSkilledLookouts() && ship.HasRadar(),
		{ NightAttackKind: NightAttackKind.CutinTorpedoPicket2 } => ship.CanDo(NightAttack.CutinTorpedoPicket) && ship.DestroyerCutinTwoHitAvailable(),

		{ NightAttackKind: NightAttackKind.CutinTorpedoDestroyerPicket } => ship.HasTorpedo(2) && ship.HasDestroyerSkilledLookouts(),
		{ NightAttackKind: NightAttackKind.CutinTorpedoDestroyerPicket2 } => ship.CanDo(NightAttack.CutinTorpedoDestroyerPicket) && ship.DestroyerCutinTwoHitAvailable(),

		{ NightAttackKind: NightAttackKind.CutinTorpedoDrum } => ship.HasTorpedo() && ship.HasDestroyerSkilledLookouts() && ship.HasDrum(),
		{ NightAttackKind: NightAttackKind.CutinTorpedoDrum2 } => ship.CanDo(NightAttack.CutinTorpedoDrum) && ship.DestroyerCutinTwoHitAvailable(),

		{ NightAttackKind: NightAttackKind.Shelling } when ship.MasterShip.ShipId is ShipId.ShimaneMaruKai => !ship.HasNightAircraft(),
		{ NightAttackKind: NightAttackKind.Shelling } => true,
		{ NightAttackKind: NightAttackKind.Torpedo } => true,
		{ NightAttackKind: NightAttackKind.AirAttack } => ship.HasNightFighter() || ship.HasNightAttacker(),

		_ => false,
	};

	public static bool DestroyerCutinTwoHitAvailable(this IShipData ship) => ship.Level >= 80;
}
