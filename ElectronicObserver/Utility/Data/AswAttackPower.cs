using System;
using System.Linq;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Attacks;
using ElectronicObserverTypes.Extensions;

namespace ElectronicObserver.Utility.Data;

public static class AswAttackPower
{
	public static int GetAswAttackPower(this IShipData ship, Enum attack, IFleetData fleet,
		EngagementType engagement = EngagementType.Parallel)
	{
		int aswFit = ship.ASWTotal - ship.ASWBase
			- ship.AllSlotInstance.Where(eq => eq is not null).Sum(eq => eq.MasterEquipment.ASW);
		int eqpower = ship.EquipmentAswPower() + aswFit;
		double basepower = (Math.Sqrt(ship.ASWBase) * 2
							+ eqpower * 1.5 + ship.GetAntiSubmarineEquipmentLevelBonus()
							+ AswAttackConstant(attack))
						   * ship.GetHPDamageBonus()
						   * Damage.EngagementDayAttackMod(engagement)
						   * ship.AswMod();

		basepower = Damage.Cap(basepower, Damage.AswAttackCap);

		return (int)basepower;
	}

	private static int AswAttackConstant(Enum attack) => attack switch
	{
		DayAttackKind.AirAttack => 8,
		_ => 13
	};

	private static int EquipmentAswPower(this IShipData ship)
	{
		int eqpower = 0;
		foreach (IEquipmentData? slot in ship.AllSlotInstance)
		{
			switch (slot?.MasterEquipment.CategoryType)
			{
				case EquipmentTypes.CarrierBasedBomber:
				case EquipmentTypes.CarrierBasedTorpedo:
				case EquipmentTypes.SeaplaneBomber:
				case EquipmentTypes.Sonar:
				case EquipmentTypes.DepthCharge:
				case EquipmentTypes.Autogyro:
				case EquipmentTypes.ASPatrol:
				case EquipmentTypes.SonarLarge:
					eqpower += slot.MasterEquipment.ASW;
					break;
			}
		}

		return eqpower;
	}

	private static double GetAntiSubmarineEquipmentLevelBonus(this IShipData ship) =>
		ship.AllSlotInstance.Sum(eq => eq.DayAswBonus());

	private static double DayAswBonus(this IEquipmentData? eq) => eq?.MasterEquipment.CategoryType switch
	{
		EquipmentTypes.DepthCharge => Math.Sqrt(eq.Level),
		EquipmentTypes.Sonar => Math.Sqrt(eq.Level),

		_ => 0
	};

	public static double AswMod(this IShipData ship)
	{
		// https://twitter.com/KennethWWKK/status/1454281174877028359?s=20

		bool smallSonar = ship.AllSlotInstance.Any(eq => eq?.MasterEquipment.CategoryType == EquipmentTypes.Sonar);
		bool largeSonar = ship.AllSlotInstance.Any(eq => eq?.MasterEquipment.CategoryType == EquipmentTypes.SonarLarge);
		bool sonarType = smallSonar || largeSonar;

		bool depthCharge = ship.AllSlotInstance.Any(eq => eq?.MasterEquipment.IsDepthCharge ?? false);
		bool depthChargeProjector = ship.AllSlotInstance.Any(eq => eq?.MasterEquipment.IsDepthChargeProjector ?? false);
		bool depthChargeType = ship.AllSlotInstance.Any(eq => eq?.MasterEquipment.CategoryType == EquipmentTypes.DepthCharge);

		double oldSynergy = (sonarType, depthChargeType) switch
		{
			(true, true) => 1.15,
			_ => 1
		};

		double newSynergy = (smallSonar, depthCharge, depthChargeProjector) switch
		{
			(true, true, true) => 1.25,
			(false, true, true) => 1.1,
			_ => 1
		};

		return oldSynergy * newSynergy;
	}
}
