using System;
using System.Linq;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Attacks;
using ElectronicObserver.Core.Types.Attacks.Specials;
using ElectronicObserver.Core.Types.Extensions;

namespace ElectronicObserver.Utility.Data;

public static class NightAttackPower
{
	public static double GetNightAttackPower(this IShipData ship, NightAttack attack, IFleetData? fleet = null)
	{
		double basepower = ship.BaseNightAttackPower() + (fleet?.NightScoutBonus() ?? 0);

		basepower *= ship.GetHPDamageBonus();
		basepower *= NightAttackKindDamageMod(attack, ship);

		basepower += ship.GetLightCruiserDamageBonus() + ship.GetItalianDamageBonus();

		basepower = Math.Floor(Damage.Cap(basepower, Damage.NightAttackCap));

		return basepower;
	}

	public static double GetNightAttackPower(this IShipData ship, SpecialAttack attack, SpecialAttackHit hit, IFleetData fleet, EngagementType engagement = EngagementType.Parallel)
	{
		double basepower = ship.BaseNightAttackPower() + fleet.NightScoutBonus();

		basepower *= ship.GetHPDamageBonus();

		basepower *= hit.PowerModifier;

		if (attack is SubmarineSpecialAttack)
		{
			// No more mod will affect this attack and apparently there's no cap to it
			return Math.Floor(basepower);
		}

		basepower *= attack.GetEngagmentModifier(engagement);

		basepower += ship.GetLightCruiserDamageBonus() + ship.GetItalianDamageBonus();

		basepower = Damage.Cap(basepower, Damage.NightAttackCap);

		return Math.Floor(basepower);
	}

	/// <summary>
	/// <see href="https://twitter.com/yukicacoon/status/1542443860109819904"/>
	/// </summary>
	private static int NightScoutBonus(this IFleetData fleet) => fleet.MembersWithoutEscaped!
		.Where(s => s is not null)
		.SelectMany(s => s!.AllSlotInstance)
		.Where(e => e is not null)
		.Where(e => e!.MasterEquipment.IsNightSeaplane())
		.DefaultIfEmpty()
		.MaxBy(e => e?.MasterEquipment.Accuracy)
		?.MasterEquipment.Accuracy switch
		{
			null => 0,
			< 2 => 5,
			2 => 7,
			> 2 => 9,
		};

	private static double BaseNightAttackPower(this IShipData ship) => ship switch
	{
		_ when ship.IsSurfaceShip() => ship.SurfaceShipBasePower(),
		_ when ship.MasterShip.IsSubmarine => ship.SurfaceShipBasePower(),

		_ when ship.IsNightCarrier() => ship.CarrierBasePower(),
		// todo: Saratoga (assumed she's the same as Graf)

		// Graf - night planes - FP+torp add to her damage
		// Graf - attackers - FP+torp add to her damage
		// Graf - secondary - FP adds to her damage
		// Graf - bombers - bombing doesn't add to her damage
		// should be just surface ship formula
		// Taiyou/Shin'you k2 are the same

		// Ark - swordfish - FP+torp add to her damage
		// Ark - attackers - torp doesn't count
		// Ark - secondary - FP doesn't count
		// Ark - upgrades don't count, maybe only swordfish upgrades (didn't check)
		// Ark - crit bonus from proficiency counts from all planes
		// Ark - fits are ignored
		// needs swordfish to attack
		_ when ship.IsSpecialNightCarrier() => ship.SurfaceShipBasePower(),
		_ when ship.IsArkRoyal() => ship.ArkRoyalBasePower(),

		_ => 0,
	};

	private static double SurfaceShipBasePower(this IShipData ship) =>
		ship.FirepowerTotal
		+ ship.TorpedoTotal
		+ ship.GetNightBattleEquipmentLevelBonus();

	private static double ArkRoyalBasePower(this IShipData ship) =>
		ship.FirepowerBase
		+ ship.TorpedoBase
		+ ship.AllSlotInstance.Where(e => e?.MasterEquipment.IsSwordfish ?? false)
			.Sum(e => e!.MasterEquipment.Firepower + e.MasterEquipment.Torpedo);

	private static double CarrierBasePower(this IShipData ship) =>
		ship.FirepowerBase
		+ ship.AllSlotInstance.Zip(ship.Aircraft, (e, size) => (e, size))
			.Where(slot => (slot.e?.MasterEquipment.IsNightAircraft ?? false) ||
						   (slot.e?.IsNightCapableAircraft() ?? false))
			.Sum(slot => slot.e!.NightPlanePower(slot.size));

	private static double NightPlanePower(this IEquipmentData equip, int size) =>
		equip.MasterEquipment.Firepower
		+ equip.MasterEquipment.Torpedo
		+ equip.NightPlaneDamageBonus() * Math.Sqrt(size)
		+ equip.NightPlaneCountMod() * size
		+ Math.Sqrt(equip.Level);

	private static double NightPlaneDamageBonus(this IEquipmentData equip) =>
		equip.NightPlanePowerMod() * (equip.MasterEquipment.Firepower
									  + equip.MasterEquipment.Torpedo
									  + equip.MasterEquipment.ASW
									  + equip.MasterEquipment.Bomber);

	private static double NightPlaneCountMod(this IEquipmentData equip) => equip switch
	{
		_ when equip.MasterEquipment.IsNightAircraft => 3,
		_ => 0,
	};

	private static double NightPlanePowerMod(this IEquipmentData equip) => equip switch
	{
		_ when equip.MasterEquipment.IsNightAircraft => 0.45,
		_ when equip.IsNightCapableAircraft() => 0.3,
		_ => 0,
	};

	private static double GetNightBattleEquipmentLevelBonus(this IShipData ship) => ship.AllSlotInstance
		.Sum(e => e.NightShellingBonus());

	private static double NightShellingBonus(this IEquipmentData? equip) =>
		equip?.MasterEquipment.CategoryType switch
		{
			EquipmentTypes.MainGunSmall or
			EquipmentTypes.MainGunMedium or
			EquipmentTypes.MainGunLarge or
			EquipmentTypes.Torpedo or
			EquipmentTypes.AAShell or
			EquipmentTypes.APShell or
			EquipmentTypes.LandingCraft or
			EquipmentTypes.Searchlight or
			EquipmentTypes.SubmarineTorpedo or
			EquipmentTypes.AADirector or
			EquipmentTypes.MainGunLarge2 or
			EquipmentTypes.SearchlightLarge or
			EquipmentTypes.SpecialAmphibiousTank or
			EquipmentTypes.SurfaceShipPersonnel => Math.Sqrt(equip.Level),

			EquipmentTypes.SecondaryGun => equip.EquipmentId switch
			{
				EquipmentId.SecondaryGun_12_7cmTwinHighangleGun or
				EquipmentId.SecondaryGun_8cmHighangleGun or
				EquipmentId.SecondaryGun_8cmHighangleGunKai_MachineGun or
				EquipmentId.SecondaryGun_10cmTwinHighangleGunKai_AdditionalMachineGuns => 0.2 * equip.Level,

				EquipmentId.SecondaryGun_15_5cmTripleSecondaryGun or
				EquipmentId.SecondaryGun_15_5cmTripleSecondaryGunKai => 0.3 * equip.Level,

				_ => Math.Sqrt(equip.Level),
			},

			_ => 0,
		};

	private static double NightAttackKindDamageMod(NightAttack attack, IShipData ship) => attack switch
	{
		{ NightAttackKind: NightAttackKind.CutinTorpedoPicket or NightAttackKind.CutinTorpedoPicket2 }
			=> attack.PowerModifier * ship.DGunMod() * ship.DKai3GunMod(),

		{ NightAttackKind: NightAttackKind.CutinTorpedoRadar or NightAttackKind.CutinTorpedoRadar2 }
			=> attack.PowerModifier * ship.DGunMod() * ship.DKai3GunMod(),

		_ => attack.PowerModifier,
	};

	private static double DGunMod(this IShipData ship) => ship.AllSlotInstance
		.Count(e => e?.EquipmentId is
			EquipmentId.MainGunSmall_12_7cmTwinGunModelDKaiNi or
			EquipmentId.MainGunSmall_12_7cmTwinGunModelDKaiSan
		) switch
		{
			0 => 1,
			1 => 1.25,
			_ => 1.25 * 1.125,
		};

	private static double DKai3GunMod(this IShipData ship) => ship.AllSlotInstance
			.Count(e => e?.EquipmentId == EquipmentId.MainGunSmall_12_7cmTwinGunModelDKaiSan) switch
	{
		0 => 1,
		_ => 1.05,
	};
}
