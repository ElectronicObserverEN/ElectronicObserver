using System;
using System.Linq;

namespace ElectronicObserverTypes.Extensions;

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

	/// <summary>
	/// 対潜改修可能値
	/// </summary>
	public static int AswModernizable(this IShipDataMaster ship) => ship switch
	{
		{ ASW: null } => 0,
		{ ASW.Maximum: 0 } => 0,
		_ => AswModernizableLimit
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
