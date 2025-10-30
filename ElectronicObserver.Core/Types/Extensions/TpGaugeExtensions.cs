using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ElectronicObserver.Core.Types.Data;

namespace ElectronicObserver.Core.Types.Extensions;

public static class TpGaugeExtensions
{
	public static int GetGaugeAreaId(this TpGauge gauge) => gauge switch
	{
		TpGauge.Spring25E2P1 or TpGauge.Spring25E5P1 => 60,
		TpGauge.Fall25E2P2 => 61,
		_ => 0,
	};

	public static int GetGaugeMapId(this TpGauge gauge) => gauge switch
	{
		TpGauge.Spring25E2P1 or TpGauge.Fall25E2P2 => 2,
		TpGauge.Spring25E5P1 => 5,
		_ => 0,
	};

	public static int GetGaugeIndex(this TpGauge gauge) => gauge switch
	{
		TpGauge.Spring25E2P1 or TpGauge.Spring25E5P1 => 1,
		TpGauge.Fall25E2P2 => 2,
		_ => 0,
	};

	public static string GetGaugeName(this TpGauge gauge, IKCDatabase db) => GetMapName(db, gauge.GetGaugeAreaId(), gauge.GetGaugeMapId());

	public static string GetShortGaugeName(this TpGauge gauge) => gauge.GetGaugeAreaId() switch
	{
		>0 => $"E{gauge.GetGaugeMapId()}-{gauge.GetGaugeIndex()}TP",
		_ => "",
	};

	/// <summary>
	/// 輸送作戦成功時の輸送量(減少TP)を求めます。
	/// (S勝利時のもの。A勝利時は int(value * 0.7))
	/// </summary>
	public static int GetTp(this TpGauge gauge, List<IFleetData> fleets) => gauge switch
	{
		TpGauge.Normal => GetNormalTpDamage(fleets) + GetKinuBonus(fleets),
		TpGauge.Spring25E2P1 => GetSpring25E2TankGaugeDamage(fleets) + GetKinuBonus(fleets),
		TpGauge.Spring25E5P1 => GetSpring25E5TankGaugeDamage(fleets) + GetKinuBonus(fleets),
		// TODO : This is a placeholder
		TpGauge.Fall25E2P2 => GetSpring25E2TankGaugeDamage(fleets) + GetKinuBonus(fleets), 
		_ => 0,
	};

	private static int GetKinuBonus(List<IFleetData> fleets) => HasKinuK2(fleets) switch
	{
		true => 8,
		_ => 0,
	};

	private static bool HasKinuK2(List<IFleetData> fleets) => fleets
		.SelectMany(f => f.MembersWithoutEscaped ?? new ReadOnlyCollection<IShipData?>([]))
		.OfType<IShipData>()
		.Where(s => s.HPRate > 0.25)
		.Any(s => s.MasterShip.ShipId is ShipId.KinuKaiNi);

	private static int GetNormalTpDamage(List<IFleetData> fleets)
		=> fleets.Sum(GetNormalTpDamage);

	private static int GetNormalTpDamage(IFleetData fleet)
	{
		if (fleet.MembersWithoutEscaped is null) return 0;

		return fleet.MembersWithoutEscaped
			.OfType<IShipData>()
			.Where(s => s.HPRate > 0.25)
			.Sum(ship => GetEquipmentTpDamage(ship) + GetShipTpDamage(ship));
	}

	private static int GetSpring25E2TankGaugeDamage(List<IFleetData> fleets)
		=> fleets.Sum(GetSpring25E2TankGaugeDamage);

	private static int GetSpring25E2TankGaugeDamage(IFleetData fleet)
	{
		if (fleet.MembersWithoutEscaped is null) return 0;

		return (int)fleet.MembersWithoutEscaped
			.OfType<IShipData>()
			.Where(s => s.HPRate > 0.25)
			.Sum(ship => GetSpring25E2LandingEquipmentTpDamage(ship) + GetSpring25E2LandingShipTpDamage(ship));
	}

	private static int GetSpring25E5TankGaugeDamage(List<IFleetData> fleets)
		=> fleets.Sum(GetSpring25E5TankGaugeDamage);

	/// <summary>
	/// <see href="https://docs.google.com/spreadsheets/d/e/2PACX-1vSUsqzI_qez3RCKWO2zGdNW4LuGq3ybdiUcyW6XqcbMW4ZRquubNS-e9dyok7OMVmHDAunV7pk485OL/pubhtml" />
	/// </summary>
	private static int GetSpring25E5TankGaugeDamage(IFleetData fleet)
	{
		if (fleet.MembersWithoutEscaped is null) return 0;

		return (int)fleet.MembersWithoutEscaped
			.OfType<IShipData>()
			.Where(s => s.HPRate > 0.25)
			.Sum(ship => GetSpring25E5LandingEquipmentTpDamage(ship) + GetSpring25E5LandingShipTpDamage(ship));
	}

	private static int GetShipTpDamage(IShipData ship) => ship.MasterShip.ShipType switch
	{
		ShipTypes.Destroyer => 5,
		ShipTypes.LightCruiser => 2,
		ShipTypes.AviationCruiser => 4,
		ShipTypes.AviationBattleship => 7,
		ShipTypes.SeaplaneTender => 9,
		ShipTypes.AmphibiousAssaultShip => 12,
		ShipTypes.SubmarineTender => 7,
		ShipTypes.TrainingCruiser => 6,
		ShipTypes.FleetOiler => 15,
		ShipTypes.SubmarineAircraftCarrier => 1,
		_ => 0,
	};

	private static int GetEquipmentTpDamage(IShipData ship) => ship.AllSlotInstanceMaster
		.OfType<IEquipmentDataMaster>()
		.Sum(GetEquipmentTpDamage);

	private static int GetEquipmentTpDamage(IEquipmentDataMaster eq) => eq.CategoryType switch
	{
		EquipmentTypes.LandingCraft => 8,
		EquipmentTypes.TransportContainer => 5,
		EquipmentTypes.Ration => 1,
		EquipmentTypes.SpecialAmphibiousTank => 2,
		_ => 0,
	};

	/// <summary>
	/// <see href="https://docs.google.com/spreadsheets/d/1ynon3m-qL7XBtDgi1kOSluVEMUen_zx1a6Bi_f4JTc4" />
	/// </summary>
	private static double GetSpring25E2LandingShipTpDamage(IShipData ship)
		=> GetShipTpDamage(ship) * 0.65;

	/// <summary>
	/// <see href="https://docs.google.com/spreadsheets/d/1ynon3m-qL7XBtDgi1kOSluVEMUen_zx1a6Bi_f4JTc4" />
	/// </summary>
	private static double GetSpring25E2LandingEquipmentTpDamage(IShipData ship)
		=> ship.AllSlotInstanceMaster
			.OfType<IEquipmentDataMaster>()
			.Sum(eq => eq.EquipmentId switch
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
			});

	/// <summary>
	/// <see href="https://docs.google.com/spreadsheets/d/167ccOcDqswVXIq3C1yG_5Twke1PLkw8jz5j7390jd9E" />
	/// </summary>
	private static double GetSpring25E5LandingShipTpDamage(IShipData ship)
		=> GetShipTpDamage(ship) * 0.8;

	/// <summary>
	/// <see href="https://docs.google.com/spreadsheets/d/167ccOcDqswVXIq3C1yG_5Twke1PLkw8jz5j7390jd9E" />
	/// </summary>
	private static double GetSpring25E5LandingEquipmentTpDamage(IShipData ship)
		=> ship.AllSlotInstanceMaster
			.OfType<IEquipmentDataMaster>()
			.Sum(eq => eq.EquipmentId switch
			{
				EquipmentId.LandingCraft_TokuDaihatsuLandingCraft_Type1GunTank => 28.4,
				EquipmentId.LandingCraft_TokuDaihatsuLandingCraft_PanzerIIITypeJ => 21.4,
				EquipmentId.LandingCraft_M4A1DD => 20.4,

				EquipmentId.SpecialAmphibiousTank_SpecialType2AmphibiousTank or EquipmentId.SpecialAmphibiousTank_SpecialType4AmphibiousTankKai => 19.6,

				EquipmentId.LandingCraft_TokuDaihatsu_ChiHaKai => 19.4,
				EquipmentId.LandingCraft_TokuDaihatsuLC_11thTankRegiment => 18.4,
				EquipmentId.LandingCraft_TokuDaihatsu_ChiHa => 17.4,

				EquipmentId.SpecialAmphibiousTank_SpecialType4AmphibiousTank => 16.6,

				EquipmentId.LandingCraft_TokuDaihatsuLandingCraft_PanzerIII_NorthAfricanCorps => 16.4,
				EquipmentId.LandingCraft_DaihatsuLandingCraft_PanzerIINorthAfricanSpecification => 14.4,

				EquipmentId.ArmyInfantry_ArmyInfantryCorps_ChiHaKai => 13,

				EquipmentId.LandingCraft_DaihatsuLC_Type89Tank_LandingForce => 12.4,

				EquipmentId.ArmyInfantry_Type97MediumTankNewTurret_ChiHaKai => 10,
				EquipmentId.ArmyInfantry_Type97MediumTank_ChiHa => 8,

				EquipmentId.ArmyInfantry_ArmyInfantryUnit => 5,

				_ when eq.CategoryType is EquipmentTypes.LandingCraft => 6.4,
				_ when eq.CategoryType is EquipmentTypes.Ration => 0.8,
				_ when eq.CategoryType is EquipmentTypes.TransportContainer => 4,
				_ => 0,
			});

	private static string GetMapName(IKCDatabase db, int areaId, int mapId)
	{
		if (db.MapInfo[areaId * 10 + mapId] is not { } mapData) return $"{areaId}-{mapId}";

		return $"{mapData.NameEN} ({areaId}-{mapId})";
	}
}
