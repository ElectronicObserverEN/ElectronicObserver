using System;
using System.Linq;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Attacks;
using ElectronicObserverTypes.Extensions;

namespace ElectronicObserver.Utility.Data;

public static class NightAttackAccuracy
{
	public static double GetNightAttackAccuracy(this IShipData ship, Enum attack, IFleetData fleet)
	{
		int baseAccuracy = fleet.BaseAccuracy();
		double shipAccuracy = ship.Accuracy();
		double equipAccuracy = ship.AllSlotInstance
			.Where(e => e != null)
			.Sum(e => e.MasterEquipment.Accuracy);

		// if night equip is present assume it activates
		return Math.Floor(fleet.NightScoutMod()
						  * (baseAccuracy + fleet.StarShellBonus())
						  + shipAccuracy
						  + equipAccuracy)
			   * ship.ConditionMod()
			   * AttackKindMod(attack)
			   + fleet.SearchlightBonus()
			   + ship.HeavyCruiserBonus();
	}

	private static int BaseAccuracy(this IFleetData fleet) => 69;

	/// <summary>
	/// <see href="https://twitter.com/yukicacoon/status/1542443860109819904"/>
	/// </summary>
	private static double NightScoutMod(this IFleetData fleet) => fleet.MembersWithoutEscaped!
		.Where(s => s is not null)
		.SelectMany(s => s!.AllSlotInstance)
		.Where(e => e is not null)
		.Where(e => e!.MasterEquipment.IsNightSeaplane())
		.DefaultIfEmpty()
		.MaxBy(e => e?.MasterEquipment.Accuracy)
		?.MasterEquipment.Accuracy switch
		{
			null => 1,
			< 2 => 1.1,
			2 => 1.15,
			> 2 => 1.2,
		};

	private static int StarShellBonus(this IFleetData fleet) => fleet switch
	{
		{ } when fleet.HasStarShell() => 5,
		_ => 0
	};

	private static int SearchlightBonus(this IFleetData fleet) => fleet switch
	{
		{ } when fleet.HasSearchlight() => 7,
		_ => 0
	};

	private static double ConditionMod(this IShipData ship) => ship.Condition switch
	{
		int condition when condition > 52 => 1.2,
		int condition when condition > 32 => 1,
		int condition when condition > 22 => 0.8,
		_ => 0.5
	};

	private static double AttackKindMod(Enum attack) => attack switch
	{
		NightAttackKind.CutinMainMain => 2,

		NightAttackKind.CutinTorpedoTorpedo => 1.65,

		NightAttackKind.CutinMainTorpedo => 1.5,
		NightAttackKind.CutinMainSub => 1.5,

		NightAttackKind.DoubleShelling => 1.1,

		NightAttackKind.NormalAttack => 1,

		// todo are we sure this is the same as TCI?
		NightTorpedoCutinKind.LateModelTorpedoSubmarineEquipment => 1.65,
		NightTorpedoCutinKind.LateModelTorpedo2 => 1.65,

		_ => 1
	};

	private static int HeavyCruiserBonus(this IShipData ship) => ship.MasterShip.ShipType switch
	{
		ShipTypes.HeavyCruiser => ship.Night20CmBonus(),
		ShipTypes.AviationCruiser => ship.Night20CmBonus(),
		_ => 0
	};

	private static int Night20CmBonus(this IShipData ship) => ship.AllSlotInstance.Count(e => e?.EquipmentId switch
		{
			EquipmentId.MainGunMedium_20_3cmTwinGun => true,
			EquipmentId.MainGunMedium_20_3cm_No_2TwinGun => true,
			EquipmentId.MainGunMedium_20_3cm_No_3TwinGun => true,
			_ => false
		}) switch
	{
		0 => 0,
		1 => 10,
		_ => 15
	};
}
