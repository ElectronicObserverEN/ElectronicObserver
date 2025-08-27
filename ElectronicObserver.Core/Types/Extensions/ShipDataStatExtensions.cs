using System;
using System.Linq;

namespace ElectronicObserver.Core.Types.Extensions;

public static class ShipDataStatExtensions
{
	private static int HpModernizableLimit => 2;
	private static int AswModernizableLimit => 9;

	private static int MarriageHpIncrease(this IShipDataMaster ship) => ship.HPMin switch
	{
		< 30 => 4,
		< 40 => 5,
		< 50 => 6,
		< 70 => 7,
		< 90 => 8,
		_ => 9
	};

	/// <summary>
	/// ケッコンカッコカリ後のHP
	/// </summary>
	public static int HpMaxMarried(this IShipDataMaster ship) =>
		Math.Min(ship.HPMin + ship.MarriageHpIncrease(), ship.HPMax);

	/// <summary>
	/// HP改修可能値(未婚時)
	/// </summary>
	public static int HpMaxModernizable(this IShipDataMaster ship) =>
		Math.Min(ship.HPMax - ship.HPMin, HpModernizableLimit);

	public static int HpMaxModernizable(this IShipData ship) =>
		ship.IsMarried ? ship.MasterShip.HpMaxMarriedModernizable() : ship.MasterShip.HpMaxModernizable();

	/// <summary>
	/// HP改修可能値(既婚時)
	/// </summary>
	public static int HpMaxMarriedModernizable(this IShipDataMaster ship) =>
		Math.Min(ship.HPMax - ship.HPMaxMarried, HpModernizableLimit);


	/// <summary>
	/// 近代化改修後のHP(未婚時)
	/// </summary>
	public static int HpMaxModernized(this IShipDataMaster ship) =>
		Math.Min(ship.HPMin + ship.HPMaxModernizable, ship.HPMax);

	public static int HpMaxModernized(this IShipData ship) =>
		ship.IsMarried ? ship.MasterShip.HpMaxMarriedModernized() : ship.MasterShip.HpMaxModernized();

	/// <summary>
	/// 近代化改修後のHP(既婚時)
	/// </summary>
	public static int HpMaxMarriedModernized(this IShipDataMaster ship) =>
		Math.Min(ship.HPMaxMarried + ship.HPMaxModernizable, ship.HPMax);

	public static int FirepowerTotal(this IShipData ship) =>
		ship.FirepowerBase +
		ship.AllSlotInstance.Sum(e => e?.MasterEquipment.Firepower ?? 0);

	public static int TorpedoTotal(this IShipData ship) =>
		ship.TorpedoBase +
		ship.AllSlotInstance.Sum(e => e?.MasterEquipment.Torpedo ?? 0);

	public static int AaTotal(this IShipData ship) =>
		ship.AABase +
		ship.AllSlotInstance.Sum(e => e?.MasterEquipment.AA ?? 0);

	public static int ArmorTotal(this IShipData ship) =>
		ship.ArmorBase +
		ship.AllSlotInstance.Sum(e => e?.MasterEquipment.Armor ?? 0);

	public static int AswTotal(this IShipData ship) =>
		ship.ASWBase +
		ship.AllSlotInstance.Sum(e => e?.MasterEquipment.ASW ?? 0);

	public static int ExpeditionFirepowerTotal(this IShipData ship) => ship.FirepowerTotal + (int)ship.AllSlotInstance
		.Where(slot => slot is not null)
		.Sum(slot => slot!.MasterEquipment.CategoryType switch
		{
			EquipmentTypes.MainGunSmall => 0.5 * Math.Sqrt(slot.Level),
			EquipmentTypes.MainGunMedium => Math.Sqrt(slot.Level),
			EquipmentTypes.SecondaryGun => 0.15 * Math.Sqrt(slot.Level),

			EquipmentTypes.MainGunLarge or
			EquipmentTypes.MainGunLarge2 => 0.9 * Math.Sqrt(slot.Level),

			EquipmentTypes.APShell or
			EquipmentTypes.AAGun => 0.5 * Math.Sqrt(slot.Level),

			_ => 0,
		});

	public static int ExpeditionAswTotal(this IShipData ship) => ship.ASWTotal + (int)ship.AllSlotInstance
		.Where(slot => slot is not null)
		.Sum(slot => slot!.MasterEquipment.CategoryType switch
		{
			EquipmentTypes.DepthCharge or
			EquipmentTypes.Sonar => Math.Sqrt(slot.Level),

			_ => 0,
		});

	public static int ExpeditionLosTotal(this IShipData ship) => ship.LOSTotal + (int)ship.AllSlotInstance
		.Where(slot => slot is not null)
		.Sum(slot => slot!.MasterEquipment.CategoryType switch
		{
			EquipmentTypes.RadarLarge or
			EquipmentTypes.RadarLarge2 or
			EquipmentTypes.RadarSmall => Math.Sqrt(slot.Level),

			_ => 0,
		});

	public static int ExpeditionAaTotal(this IShipData ship) => ship.AATotal + (int)ship.AllSlotInstance
		.Where(slot => slot is not null)
		.Sum(slot => slot!.MasterEquipment.CategoryType switch
		{
			EquipmentTypes.AAGun => 0.9 * slot.Level,
			_ when slot.MasterEquipment.IsHighAngleGun => 0.3 * slot.Level,
			_ => 0,
		});

	public static bool HasZeroSlotAircraft(this IShipData ship) => ship.AllSlotInstance
		.Zip(ship.Aircraft, (e, a) => (Equipment: e, Aircraft: a))
		.Where(s => s.Equipment is not null)
		.Any(s => s.Equipment.IsAircraft() && s.Aircraft is 0);

	/// <summary>
	/// 対潜改修可能値
	/// </summary>
	public static int AswModernizable(this IShipDataMaster ship) => ship switch
	{
		{ ASW: null } => 0,
		{ ASW.Maximum: 0 } => 0,
		_ => AswModernizableLimit,
	};

	public static int EvasionTotal(this IShipData ship) =>
		ship.EvasionBase +
		ship.AllSlotInstance.Sum(e => e?.MasterEquipment.Evasion ?? 0);

	public static int LosTotal(this IShipData ship) =>
		ship.LOSBase +
		ship.AllSlotInstance.Sum(e => e?.MasterEquipment.LOS ?? 0);

	public static int LuckTotal(this IShipData ship) =>
		ship.LuckBase +
		ship.AllSlotInstance.Sum(e => e?.MasterEquipment.Luck ?? 0);

	public static int BomberTotal(this IShipData ship) =>
		ship.AllSlotInstance.Sum(e => e?.MasterEquipment.Bomber ?? 0);

	public static int AccuracyTotal(this IShipData ship) =>
		ship.AllSlotInstance.Sum(e => e?.MasterEquipment.Accuracy ?? 0);
}
