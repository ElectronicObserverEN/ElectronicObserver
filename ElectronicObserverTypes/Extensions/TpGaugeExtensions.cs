using System.Linq;
using ElectronicObserverTypes.Data;

namespace ElectronicObserverTypes.Extensions;
public static class TpGaugeExtensions
{
	public static string GetGaugeName(this TpGauge gauge, IKCDatabase db) => gauge switch
	{
		TpGauge.Spring25E2 => GetMapName(db, 60, 2),
		TpGauge.Spring25E5 => GetMapName(db, 60, 5),
		_ => "",
	};

	public static int GetTp(this TpGauge gauge, IFleetData fleet) => gauge switch
	{
		TpGauge.Normal => GetNormalTpDamage(fleet),
		TpGauge.Spring25E2 => GetSpring25E2TankGaugeDamage(fleet),
		TpGauge.Spring25E5 => GetSpring25E5TankGaugeDamage(fleet),
		_ => 0,
	};

	private static int GetNormalTpDamage(IFleetData fleet)
	{
		if (fleet.MembersWithoutEscaped is null) return 0;

		return fleet.MembersWithoutEscaped
			.OfType<IShipData>()
			.Where(s => s.HPRate > 0.25)
			.Sum(ship => GetEquipmentTpDamage(ship) + GetShipTpDamage(ship));
	}

	private static int GetSpring25E2TankGaugeDamage(IFleetData fleet)
	{
		if (fleet.MembersWithoutEscaped is null) return 0;

		return (int)fleet.MembersWithoutEscaped
			.OfType<IShipData>()
			.Where(s => s.HPRate > 0.25)
			.Sum(ship => GetSpring25E2LandingEquipmentTpDamage(ship) + GetSpring25E2LandingShipTpDamage(ship));
	}

	/// <summary>
	/// https://docs.google.com/spreadsheets/d/e/2PACX-1vSUsqzI_qez3RCKWO2zGdNW4LuGq3ybdiUcyW6XqcbMW4ZRquubNS-e9dyok7OMVmHDAunV7pk485OL/pubhtml
	/// </summary>
	/// <param name="fleet"></param>
	/// <returns></returns>
	private static int GetSpring25E5TankGaugeDamage(IFleetData fleet)
	{
		if (fleet.MembersWithoutEscaped is null) return 0;

		return (int)fleet.MembersWithoutEscaped
			.OfType<IShipData>()
			.Where(s => s.HPRate > 0.25)
			.Sum(ship => GetSpring25E5LandingEquipmentTpDamage(ship) + GetSpring25E5LandingShipTpDamage(ship));
	}

	private static int GetShipTpDamage(IShipData ship)
	{
		int tp = 0;

		// 艦種ボーナス
		switch (ship.MasterShip.ShipType)
		{

			case ShipTypes.Destroyer:
				tp += 5;
				break;

			case ShipTypes.LightCruiser:
				tp += 2;
				if (ship.ShipID == 487) // 鬼怒改二
					tp += 8;
				break;

			case ShipTypes.AviationCruiser:
				tp += 4;
				break;

			case ShipTypes.AviationBattleship:
				tp += 7;
				break;

			case ShipTypes.SeaplaneTender:
				tp += 9;
				break;

			case ShipTypes.AmphibiousAssaultShip:
				tp += 12;
				break;

			case ShipTypes.SubmarineTender:
				tp += 7;
				break;

			case ShipTypes.TrainingCruiser:
				tp += 6;
				break;

			case ShipTypes.FleetOiler:
				tp += 15;
				break;

			case ShipTypes.SubmarineAircraftCarrier:
				tp += 1;
				break;
		}

		return tp;
	}

	private static int GetEquipmentTpDamage(IShipData ship)
	{
		int tp = 0;

		// 装備ボーナス
		foreach (var eq in ship.AllSlotInstanceMaster.OfType<IEquipmentDataMaster>())
		{
			tp += GetEquipmentTpDamage(eq);
		}

		return tp;
	}

	private static int GetEquipmentTpDamage(IEquipmentDataMaster eq) => eq.CategoryType switch
	{
		EquipmentTypes.LandingCraft => 8,
		EquipmentTypes.TransportContainer => 5,
		EquipmentTypes.Ration => 1,
		EquipmentTypes.SpecialAmphibiousTank => 2,
		_ => 0,
	};

	/// <summary>
	/// We're using https://docs.google.com/spreadsheets/d/1ynon3m-qL7XBtDgi1kOSluVEMUen_zx1a6Bi_f4JTc4 as source
	/// </summary>
	/// <param name="ship"></param>
	/// <returns></returns>
	private static double GetSpring25E2LandingShipTpDamage(IShipData ship)
	{
		return ship.MasterShip.ShipType switch
		{
			ShipTypes.Destroyer => 3.25,
			ShipTypes.LightCruiser when ship.MasterShip.ShipId is ShipId.KinuKaiNi => 9.3,
			ShipTypes.LightCruiser => 1.3,
			ShipTypes.SeaplaneTender => 5.85,
			ShipTypes.AviationCruiser => 2.6,
			ShipTypes.AviationBattleship => 4.55,

			ShipTypes.TrainingCruiser => 3.9,
			ShipTypes.FleetOiler => 9.75,
			ShipTypes.AmphibiousAssaultShip => 7.8,
			ShipTypes.SubmarineAircraftCarrier => 0.65,
			ShipTypes.SubmarineTender => 4.55,

			_ => 0,
		};
	}

	/// <summary>
	/// We're using https://docs.google.com/spreadsheets/d/1ynon3m-qL7XBtDgi1kOSluVEMUen_zx1a6Bi_f4JTc4 as source
	/// </summary>
	/// <param name="ship"></param>
	/// <returns></returns>
	private static double GetSpring25E2LandingEquipmentTpDamage(IShipData ship)
	{
		double tp = 0;

		foreach (IEquipmentDataMaster eq in ship.AllSlotInstanceMaster.OfType<IEquipmentDataMaster>())
		{
			tp += eq.EquipmentId switch
			{
				EquipmentId.LandingCraft_TokuDaihatsuLC_11thTankRegiment => 46.2,
				EquipmentId.LandingCraft_TokuDaihatsuLandingCraft_Type1GunTank => 40.2,
				EquipmentId.LandingCraft_TokuDaihatsuLandingCraft_PanzerIIITypeJ => 32.2,
				EquipmentId.LandingCraft_TokuDaihatsu_ChiHaKai => 28.2,
				EquipmentId.LandingCraft_TokuDaihatsuLandingCraft_PanzerIII_NorthAfricanCorps => 27.2,
				EquipmentId.LandingCraft_M4A1DD => 24.2,
				EquipmentId.LandingCraft_TokuDaihatsu_ChiHa => 22.2,
				EquipmentId.LandingCraft_DaihatsuLandingCraft_PanzerIINorthAfricanSpecification => 21.2,
				EquipmentId.LandingCraft_DaihatsuLC_Type89Tank_LandingForce => 14.2,

				EquipmentId.ArmyInfantry_ArmyInfantryCorps_ChiHaKai => 38,
				EquipmentId.ArmyInfantry_Type97MediumTankNewTurret_ChiHaKai => 23,
				EquipmentId.ArmyInfantry_Type97MediumTank_ChiHa => 17,
				EquipmentId.ArmyInfantry_ArmyInfantryUnit => 15,

				EquipmentId.SpecialAmphibiousTank_SpecialType2AmphibiousTank => 9.3,
				EquipmentId.SpecialAmphibiousTank_SpecialType4AmphibiousTank => 6.3,
				EquipmentId.SpecialAmphibiousTank_SpecialType4AmphibiousTankKai => 8.3,

				_ when eq.CategoryType is EquipmentTypes.LandingCraft => 5.2,
				_ when eq.CategoryType is EquipmentTypes.Ration => 0.65,
				_ when eq.CategoryType is EquipmentTypes.TransportContainer => 3.25,
				_ => 0,
			};
		}

		return tp;
	}

	/// <summary>
	/// https://docs.google.com/spreadsheets/d/e/2PACX-1vSUsqzI_qez3RCKWO2zGdNW4LuGq3ybdiUcyW6XqcbMW4ZRquubNS-e9dyok7OMVmHDAunV7pk485OL/pubhtml
	/// </summary>
	/// <param name="ship"></param>
	/// <returns></returns>
	private static double GetSpring25E5LandingShipTpDamage(IShipData ship) => GetShipTpDamage(ship) * 0.8;

	/// <summary>
	/// https://docs.google.com/spreadsheets/d/e/2PACX-1vSUsqzI_qez3RCKWO2zGdNW4LuGq3ybdiUcyW6XqcbMW4ZRquubNS-e9dyok7OMVmHDAunV7pk485OL/pubhtml
	/// </summary>
	/// <param name="ship"></param>
	/// <returns></returns>
	private static double GetSpring25E5LandingEquipmentTpDamage(IShipData ship)
	{
		double tp = 0;

		foreach (IEquipmentDataMaster eq in ship.AllSlotInstanceMaster.OfType<IEquipmentDataMaster>())
		{
			tp += eq.EquipmentId switch
			{
				EquipmentId.LandingCraft_TokuDaihatsuLandingCraft_Type1GunTank => 28.4,
				EquipmentId.LandingCraft_TokuDaihatsuLandingCraft_PanzerIIITypeJ => 21.4,
				EquipmentId.LandingCraft_M4A1DD => 20.4,
				EquipmentId.LandingCraft_TokuDaihatsu_ChiHaKai => 19.4,
				EquipmentId.LandingCraft_TokuDaihatsuLC_11thTankRegiment => 18.4,

				EquipmentId.LandingCraft_TokuDaihatsuLandingCraft_PanzerIII_NorthAfricanCorps => 0,
				EquipmentId.LandingCraft_TokuDaihatsu_ChiHa => 0,
				EquipmentId.LandingCraft_DaihatsuLandingCraft_PanzerIINorthAfricanSpecification => 0,
				EquipmentId.LandingCraft_DaihatsuLC_Type89Tank_LandingForce => 0,

				EquipmentId.SpecialAmphibiousTank_SpecialType2AmphibiousTank or
				EquipmentId.SpecialAmphibiousTank_SpecialType4AmphibiousTankKai => 19.6,
				EquipmentId.SpecialAmphibiousTank_SpecialType4AmphibiousTank => 16.6,

				EquipmentId.ArmyInfantry_ArmyInfantryCorps_ChiHaKai => 0,
				EquipmentId.ArmyInfantry_Type97MediumTankNewTurret_ChiHaKai => 0,
				EquipmentId.ArmyInfantry_Type97MediumTank_ChiHa => 0,
				EquipmentId.ArmyInfantry_ArmyInfantryUnit => 0,

				_ => GetEquipmentTpDamage(eq) * 0.8,
			};
		}

		return tp;
	}

	private static string GetMapName(IKCDatabase db, int areaId, int mapId)
	{
		if (db.MapInfo[(areaId * 10) + mapId] is not { } mapData) return $"{areaId}-{mapId}";

		return $"{mapData.NameEN} ({areaId}-{mapId})";
	}
}
