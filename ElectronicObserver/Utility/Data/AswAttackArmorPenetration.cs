using System;
using System.Linq;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Utility.Data;

public static class AswAttackArmorPenetration
{
	public static double GetAswArmorPenetration(this IShipData ship)
	{
		int shipBonus = ship.MasterShip.ShipType switch
		{
			ShipTypes.Escort => 1,
			_ => 0
		};

		return ship.AllSlotInstanceMaster.Sum(eq => GetEquipmentArmorPenetration(eq, shipBonus));
	}

	private static double GetEquipmentArmorPenetration(IEquipmentDataMaster? eq, int shipBonus)
	{
		if (eq is null) return 0;
		if (!IsArmorPenetrationEquipment(eq)) return 0;

		return Math.Sqrt(eq.ASW - 2) + shipBonus;
	}

	private static bool IsArmorPenetrationEquipment(IEquipmentDataMaster? eq) => eq?.EquipmentId is
		EquipmentId.DepthCharge_Type95DepthCharge or
		EquipmentId.DepthCharge_Type2DepthCharge or
		EquipmentId.DepthCharge_Type2DepthChargeKaiNi or
		EquipmentId.DepthCharge_LightweightASWTorpedo_InitialTestModel or
		EquipmentId.DepthCharge_Hedgehog_InitialModel or
		
		EquipmentId.DepthCharge_RUR4AWeaponAlphaKai or 
		EquipmentId.DepthCharge_Mk_32ASWTorpedo_Mk_2Thrower;
}
