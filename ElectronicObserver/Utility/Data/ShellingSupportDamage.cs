using System;
using System.Linq;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Extensions;

namespace ElectronicObserver.Utility.Data;

public static class ShellingSupportDamage
{
	public static int GetShellingSupportDamage(this IShipData ship, EngagementType engagement = EngagementType.Parallel)
	{
		double basepower = ship.BaseDayAttackPower() *
						   ship.GetHPDamageBonus() *
						   Damage.EngagementDayAttackMod(engagement);

		basepower = Math.Floor(Damage.Cap(basepower, Damage.SupportAttackCap));

		return (int)basepower;
	}

	private static double BaseDayAttackPower(this IShipData ship) => ship switch
	{
		{ MasterShip.IsAircraftCarrier: true } => ship.CarrierBasePower(),

		_ => ship.SurfaceShipBasePower()
	};

	private static double SurfaceShipBasePower(this IShipData ship) => ship.FirepowerTotal + 4;

	private static double CarrierBasePower(this IShipData ship) =>
		Math.Floor((ship.FirepowerTotal +
					ship.TorpedoBase +
					ship.AllSlotInstance.Sum(e => e?.MasterEquipment.Torpedo ?? 0) +
					Math.Floor(ship.AllSlotInstance.Sum(e => e?.MasterEquipment.Bomber ?? 0) * 1.3)) * 1.5) + 50 + 4;
}
