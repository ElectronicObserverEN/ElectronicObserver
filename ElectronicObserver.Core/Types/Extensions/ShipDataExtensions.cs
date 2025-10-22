using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Core.Types.Evasion;

namespace ElectronicObserver.Core.Types.Extensions;

public static class ShipDataExtensions
{
	public static IShipDataMaster BaseShip(this IShipDataMaster ship)
	{
		IShipDataMaster temp = ship;

		while (temp.RemodelBeforeShip != null)
		{
			temp = temp.RemodelBeforeShip;
		}

		return temp;
	}

	/// <summary>
	/// 深海棲艦かどうか
	/// </summary>
	public static bool IsAbyssalShip(this IShipDataMaster ship) => ship.ShipID > 1500;

	/// <summary>
	/// Calculates the ship accuracy.
	/// </summary>
	/// <param name="ship">Ship.</param>
	/// <param name="level">Custom level override, ship.Level will be used by default.</param>
	/// <param name="luck">Custom luck override, ship.LuckTotal will be used by default.</param>
	/// <returns>Ship accuracy.</returns>
	public static double Accuracy(this IShipData ship, int? level = null, int? luck = null) =>
		2 * Math.Sqrt(level ?? ship.Level) + 1.5 * Math.Sqrt(luck ?? ship.LuckTotal);

	public static int NextAccuracyLevel(this IShipData ship, int? currentAccuracy = null)
	{
		int targetAccuracy = (currentAccuracy ?? (int)ship.Accuracy()) + 1;
		double luckPart = 1.5 * Math.Sqrt(ship.LuckTotal);

		return (int)Math.Ceiling(Math.Pow((targetAccuracy - luckPart) / 2, 2));
	}

	public static double ShellingEvasion(this IShipData ship) =>
		new ShellingEvasion(ship).PostcapValue;

	public static double TorpedoEvasion(this IShipData ship) =>
		new TorpedoEvasion(ship).PostcapValue;

	public static double AirstrikeEvasion(this IShipData ship) =>
		new AirstrikeEvasion(ship).PostcapValue;

	public static double AswEvasion(this IShipData ship) =>
		new AswEvasion(ship).PostcapValue;

	public static double NightEvasion(this IShipData ship) =>
		new NightEvasion(ship).PostcapValue;

	public static int MainGunCount(this IShipData ship) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.IsMainGun == true);

	public static bool HasMainGun(this IShipData ship, int count = 1) => ship.MainGunCount() >= count;

	public static bool HasMainGunLarge(this IShipData ship, int count = 1) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.CategoryType is
			EquipmentTypes.MainGunLarge or
			EquipmentTypes.MainGunLarge2)
		 >= count;

	public static bool HasMainGunLargeFcr(this IShipData ship, int count = 1) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.EquipmentId is
			EquipmentId.MainGunLarge_16inchMk_ITripleGunKai_FCRType284)
		>= count;

	public static int SecondaryGunCount(this IShipData ship) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.CategoryType == EquipmentTypes.SecondaryGun);

	public static bool HasSecondaryGun(this IShipData ship) => ship.AllSlotInstance
		.Any(e => e?.MasterEquipment.CategoryType == EquipmentTypes.SecondaryGun);

	public static bool HasApShell(this IShipData ship) => ship.AllSlotInstance
		.Any(e => e?.MasterEquipment.CategoryType == EquipmentTypes.APShell);

	public static bool HasAaShell(this IShipData ship, int count) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.CategoryType == EquipmentTypes.AAShell) >= count;

	public static bool HasSeaplane(this IShipData ship) => ship.AllSlotInstance
		.Zip(ship.Aircraft, (e, size) => (e, size))
		.Any(s => s.size > 0 && s.e?.MasterEquipment.IsSeaplane() == true);

	public static bool HasRadar(this IShipData ship, int count = 1) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.IsRadar is true) >= count;

	public static bool HasSurfaceRadar(this IShipData ship) => ship.AllSlotInstance
		.Any(e => e?.MasterEquipment.IsSurfaceRadar is true);

	/// <summary>
	/// Both min and max are inclusive
	/// </summary>
	public static bool HasAirRadar(this IShipData ship, int count = 1, int min = 0, int max = 9999) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.IsAirRadar is true &&
			e.MasterEquipment.AA >= min &&
			e.MasterEquipment.AA <= max)
		>= count;

	public static bool HasHighAccuracyRadar(this IShipData ship) => ship.AllSlotInstance
		.Any(e => e?.MasterEquipment.IsHighAccuracyRadar is true);

	public static bool HasRadarGfcs(this IShipData ship, int count) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.EquipmentId is
			EquipmentId.RadarSmall_GFCSMk_37)
		>= count;

	public static bool HasSuisei634(this IShipData ship, int count = 1) => ship.AllSlotInstance
		.Count(e => e.IsSuisei634()) >= count;

	public static bool HasZuiun(this IShipData ship, int count = 1) => ship.AllSlotInstance
		.Count(e => e.IsZuiun()) >= count;

	public static bool HasFighter(this IShipData ship) => ship.AllSlotInstance
		.Zip(ship.Aircraft, (e, size) => (e, size))
		.Any(s => s.size > 0 && s.e?.MasterEquipment.CategoryType == EquipmentTypes.CarrierBasedFighter);

	public static bool HasBomber(this IShipData ship, int count = 1) => ship.AllSlotInstance
		.Zip(ship.Aircraft, (e, size) => (e, size))
		.Count(s => s.size > 0 && s.e?.MasterEquipment.CategoryType == EquipmentTypes.CarrierBasedBomber)
		>= count;

	public static bool HasAttacker(this IShipData ship) => ship.AllSlotInstance
		.Zip(ship.Aircraft, (e, size) => (e, size))
		.Any(s => s.size > 0 && s.e?.MasterEquipment.CategoryType == EquipmentTypes.CarrierBasedTorpedo);

	public static bool HasJetBomber(this IShipData ship, int count = 1) =>
		ship.AllSlotInstance
			.Zip(ship.Aircraft, (e, size) => (e, size))
			.Count(s => s.size > 0 && s.e?.MasterEquipment.CategoryType is EquipmentTypes.JetBomber)
		>= count;

	public static bool HasTorpedo(this IShipData ship, int count = 1) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.IsTorpedo == true) >= count;

	public static bool HasSkilledLookouts(this IShipData ship) => ship.AllSlotInstance
		.Any(e => e?.MasterEquipment.CategoryType == EquipmentTypes.SurfaceShipPersonnel);

	public static bool HasRegularSkilledLookouts(this IShipData ship) => ship.AllSlotInstance
		.Any(e => e?.EquipmentId == EquipmentId.SurfaceShipPersonnel_SkilledLookouts);

	public static bool HasDestroyerSkilledLookouts(this IShipData ship) => ship.AllSlotInstance
		.Any(e => e?.EquipmentId == EquipmentId.SurfaceShipPersonnel_TorpedoSquadronSkilledLookouts);

	public static bool HasDrum(this IShipData ship) => ship.AllSlotInstance
		.Any(e => e?.MasterEquipment.CategoryType == EquipmentTypes.TransportContainer);

	public static bool IsPt(this IShipDataMaster ship) => ship.ShipID is
		1637 or
		1638 or
		1639 or
		1640 or
		2192 or
		2193 or
		2194;

	/// <summary>
	/// 空母系か (軽空母/正規空母/装甲空母)
	/// </summary>
	public static bool IsAircraftCarrier(this IShipData ship) => ship.MasterShip.IsAircraftCarrier();

	/// <summary>
	/// 空母系か (軽空母/正規空母/装甲空母)
	/// </summary>
	public static bool IsAircraftCarrier(this IShipDataMaster ship) => ship.ShipType is
		ShipTypes.LightAircraftCarrier or
		ShipTypes.AircraftCarrier or
		ShipTypes.ArmoredAircraftCarrier;

	public static bool IsNightCarrier(this IShipData ship) =>
		ship.HasNightAviationPersonnel() ||
		ship.MasterShip.ShipId.IsNightCarrier();

	private static bool HasNightAviationPersonnel(this IShipData ship) => ship.AllSlotInstance
		.Any(e => e?.MasterEquipment.IsNightAviationPersonnel == true);

	public static bool HasNightFighter(this IShipData ship, int count = 1) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.IsNightFighter == true) >= count;

	public static bool HasNightAttacker(this IShipData ship) => ship.AllSlotInstance
		.Any(e => e?.MasterEquipment.IsNightAttacker == true);

	public static bool HasNightAircraft(this IShipData ship, int count = 1) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.IsNightAircraft == true || e?.IsNightCapableAircraft() == true)
		>= count;

	public static bool HasNightPhototubePlane(this IShipData ship) => ship.AllSlotInstance
		.Any(e => e?.EquipmentId == EquipmentId.CarrierBasedBomber_SuiseiModel12_wType31PhotoelectricFuzeBombs);

	public static bool HasSwordfish(this IShipData ship) => ship.AllSlotInstance
		.Any(e => e?.MasterEquipment.IsSwordfish ?? false);

	public static bool HasLateModelTorpedo(this IShipData ship, int count = 1) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.IsLateModelTorpedo() == true) >= count;

	public static bool HasSubmarineEquipment(this IShipData ship) => ship.AllSlotInstance
		.Any(e => e?.MasterEquipment.CategoryType == EquipmentTypes.SubmarineEquipment);

	public static bool HasStarShell(this IShipData ship) => ship.AllSlotInstance
		.Any(e => e?.MasterEquipment.CategoryType == EquipmentTypes.StarShell);

	public static bool HasSearchlight(this IShipData ship) => ship.AllSlotInstance
		.Any(e => e?.MasterEquipment.CategoryType is
			EquipmentTypes.Searchlight or
			EquipmentTypes.SearchlightLarge);

	public static bool HasLargeSearchlight(this IShipData ship) => ship.AllSlotInstance
		.Any(e => e?.MasterEquipment.CategoryType is EquipmentTypes.SearchlightLarge);

	public static int HighAngleGunCount(this IShipData ship) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.IsHighAngleGun is true);

	public static bool HasHighAngleGun(this IShipData ship, int count = 1) =>
		ship.HighAngleGunCount() >= count;

	public static int HighAngleDirectorGunCount(this IShipData ship) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.IsHighAngleGunWithAADirector is true);

	public static bool HasHighAngleDirectorGun(this IShipData ship, int count = 1) =>
		ship.HighAngleDirectorGunCount() >= count;

	public static int HighAngleDirectorWithoutGunCount(this IShipData ship) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.IsHighAngleGun is true &&
			e.MasterEquipment.AA < 8);

	public static bool HasHighAngleWithoutDirectorGun(this IShipData ship, int count = 1) =>
		ship.HighAngleDirectorWithoutGunCount() >= count;

	public static bool HasDirector(this IShipData ship, int count = 1) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.CategoryType is EquipmentTypes.AADirector) >= count;

	/// <summary>
	/// Both min and max are inclusive
	/// </summary>
	public static bool HasAaGun(this IShipData ship, int count = 1, int min = 0, int max = 9999) =>
		ship.AllSlotInstance
			.Count(e => e?.MasterEquipment.CategoryType is EquipmentTypes.AAGun &&
				e.MasterEquipment.AA >= min &&
				e.MasterEquipment.AA <= max)
			>= count;

	public static bool HasPompom(this IShipData ship, int count = 1) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.EquipmentId is
			EquipmentId.AAGun_QF2pounderOctuplePompomGun)
		>= count;

	public static bool HasAaRocketBritish(this IShipData ship, int count = 1) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.EquipmentId is
			EquipmentId.AAGun_20tube7inchUPRocketLaunchers)
		>= count;

	public static bool HasAaRocketMod(this IShipData ship, int count = 1) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.EquipmentId is
			EquipmentId.AAGun_12cm30tubeRocketLauncherKaiNi)
		>= count;

	public static bool HasShigureAaGun(this IShipData ship, int count = 1) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.EquipmentId is
			EquipmentId.AAGun_25mmAntiaircraftMachineGunExtra)
		>= count;

	public static bool HasHighAngleMusashi(this IShipData ship, int count = 1) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.EquipmentId is
			EquipmentId.SecondaryGun_10cmTwinHighangleGunKai_AdditionalMachineGuns)
		>= count;

	public static bool HasHighAngleAmerican(this IShipData ship, int count = 1) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.EquipmentId is
			EquipmentId.MainGunSmall_5inchSingleGunMk_30)
		>= count;

	public static bool HasHighAngleAmericanKai(this IShipData ship, int count = 1) => ship.AllSlotInstance
			.Count(e => e?.MasterEquipment.EquipmentId is
				EquipmentId.MainGunSmall_5inchSingleGunMk_30Kai)
		>= count;

	public static bool HasHighAngleAmericanGfcs(this IShipData ship, int count = 1) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.EquipmentId is
			EquipmentId.MainGunSmall_5inchSingleGunMk_30Kai_GFCSMk_37)
		>= count;

	public static int HighAngleAtlantaCount(this IShipData ship) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.EquipmentId is
			EquipmentId.MainGunMedium_5inchTwinDualpurposeGunMount_ConcentratedDeployment);

	public static bool HasHighAngleAtlanta(this IShipData ship, int count = 1) =>
		ship.HighAngleAtlantaCount() >= count;

	public static int HighAngleAtlantaGfcsCount(this IShipData ship) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.EquipmentId is
			EquipmentId.MainGunMedium_GFCSMk_37_5inchTwinDualpurposeGunMount_ConcentratedDeployment);

	public static bool HasHighAngleAtlantaGfcs(this IShipData ship, int count = 1) =>
		ship.HighAngleAtlantaGfcsCount() >= count;

	public static bool HasHarunaGun(this IShipData ship, int count) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.EquipmentId is
			EquipmentId.MainGunLarge_35_6cmTwinGunMountKaiSan_DazzleCamouflageSpecification or
			EquipmentId.MainGunLarge_35_6cmTwinGunMountKaiYon)
		>= count;

	public static bool HasHarusameGun(this IShipData ship, int count) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.EquipmentId is
			EquipmentId.MainGunSmall_12_7cmTwinGunModelCKaiSanH)
		>= count;

	public static bool HasHatsuzukiGun(this IShipData ship, int count) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.EquipmentId is
			EquipmentId.MainGunSmall_10cmTwinHighangleMountKai_AntiAircraftFireDirectorKai)
		>= count;

	public static bool HasAkizukiGunKai(this IShipData ship, int count) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.EquipmentId is
			EquipmentId.MainGunSmall_10cmTwinHighangleMountKai_AntiAircraftFireDirectorKai or
			EquipmentId.MainGunSmall_10cmTwinHighAngleGunKai)
		>= count;

	public static bool HasAkizukiPotatoGun(this IShipData ship, int count) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.EquipmentId is
			EquipmentId.MainGunSmall_10cmTwinHighAngleGunKai)
		>= count;

	public static bool HasAafd94(this IShipData ship, int count) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.EquipmentId is
			EquipmentId.AADirector_Type94AAFD)
		>= count;

	public static bool HasHighAngleConcentrated(this IShipData ship, int count = 1) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.EquipmentId is
			EquipmentId.SecondaryGun_10cmTwinHighangleGunMountBatteryConcentratedDeployment)
		>= count;

	public static bool HasYamatoRadar(this IShipData ship, int count = 1) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.EquipmentId is
			EquipmentId.RadarLarge_15mDuplexRangefinder_Type21AirRADARKaiNi or
			EquipmentId.RadarLarge_15mDuplexRangefinderKai_Type21RadarKaiNi_SkilledFDC)
		>= count;

	public static bool HasSonar(this IShipData ship, int count = 1) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.IsSonar() is true) >= count;

	public static bool HasAntiSubmarineAircraft(this IShipData ship, int count = 1) => ship.AllSlotInstance
		.Zip(ship.Aircraft, (e, s) => (Equipment: e, Size: s))
		.Count(t => t.Equipment?.MasterEquipment.IsAntiSubmarineAircraft is true && t.Size > 0) >= count;

	public static bool HasSpecialAntiSubmarineAttacker(this IShipData ship, int count = 1) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.CategoryType is EquipmentTypes.CarrierBasedTorpedo &&
			e.MasterEquipment.ASW >= 7)
		>= count;

	public static bool HasAswPatrolAircraft(this IShipData ship, int count = 1) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.CategoryType is
			EquipmentTypes.FlyingBoat or
			EquipmentTypes.ASPatrol or
			EquipmentTypes.Autogyro)
		>= count;

	public static bool HasNightZuiun(this IShipData ship, int count = 1) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.EquipmentId is EquipmentId.SeaplaneBomber_PrototypeNightZuiun_AttackEquipment)
		>= count;

	public static bool IsIseClassK2(this IShipData ship) => ship.MasterShip.ShipId switch
	{
		ShipId.IseKaiNi => true,
		ShipId.HyuugaKaiNi => true,
		_ => false
	};

	public static bool IsDestroyer(this IShipData ship) => ship.MasterShip.ShipType switch
	{
		ShipTypes.Destroyer => true,
		_ => false
	};

	public static bool IsSurfaceShip(this IShipData ship) => ship.MasterShip.ShipType switch
	{
		ShipTypes.Escort => true,
		ShipTypes.Destroyer => true,
		ShipTypes.LightCruiser => true,
		ShipTypes.TorpedoCruiser => true,
		ShipTypes.HeavyCruiser => true,
		ShipTypes.AviationCruiser => true,
		ShipTypes.Battlecruiser => true,
		ShipTypes.Battleship => true,
		ShipTypes.AviationBattleship => true,
		ShipTypes.SuperDreadnoughts => true,
		ShipTypes.Transport => true,
		ShipTypes.SeaplaneTender => true,
		ShipTypes.AmphibiousAssaultShip => true,
		ShipTypes.RepairShip => true,
		ShipTypes.SubmarineTender => true,
		ShipTypes.TrainingCruiser => true,
		ShipTypes.FleetOiler => true,

		_ => false
	};

	public static bool IsNightZuiunCutInShip(this IShipData ship) => ship.MasterShip.ShipType is
		ShipTypes.LightCruiser or
		ShipTypes.AviationCruiser or
		ShipTypes.AviationBattleship or
		ShipTypes.SeaplaneTender;

	public static bool IsSpecialNightCarrier(this IShipData ship) => ship.MasterShip.ShipId is
		ShipId.GrafZeppelin or
		ShipId.GrafZeppelinKai or
		ShipId.Saratoga or
		ShipId.TaiyouKaiNi or
		ShipId.ShinyouKaiNi or
		ShipId.UnyouKaiNi or
		ShipId.KagaKaiNiGo or 
		ShipId.Lexington or 
		ShipId.LexingtonKai;

	public static bool IsArkRoyal(this IShipData ship) => ship.MasterShip.ShipId switch
	{
		ShipId.ArkRoyal => true,
		ShipId.ArkRoyalKai => true,

		_ => false
	};

	public static bool IsBigSeven(this IShipData ship) => ship.MasterShip.ShipClassTyped is
		ShipClass.Nelson or
		ShipClass.Nagato or
		ShipClass.Colorado;

	public static bool IsFlagship(this IShipData ship, IFleetData fleet) => ship.Fleet switch
	{
		-1 => false,
		_ => fleet.MembersInstance[0] switch
		{
			IShipData flagship => ship.MasterID == flagship.MasterID,
			_ => false,
		}
	};

	public static double GetHPDamageBonus(this IShipData ship) => ship.HPRate switch
	{
		_ when ship.HPRate <= 0.25 => 0.4,
		_ when ship.HPRate <= 0.5 => 0.7,
		_ => 1,
	};

	public static double GetLightCruiserDamageBonus(this IShipData ship)
	{
		if (ship.MasterShip.ShipType != ShipTypes.LightCruiser &&
			ship.MasterShip.ShipType != ShipTypes.TorpedoCruiser &&
			ship.MasterShip.ShipType != ShipTypes.TrainingCruiser)
		{
			return 0;
		}

		int single = ship.AllSlotInstance.Count(e => e?.EquipmentId switch
		{
			EquipmentId.MainGunMedium_14cmSingleGun => true,
			EquipmentId.SecondaryGun_15_2cmSingleGun => true,
			_ => false
		});

		int twin = ship.AllSlotInstance.Count(e => e?.EquipmentId switch
		{
			EquipmentId.MainGunMedium_14cmTwinGun => true,
			EquipmentId.MainGunMedium_15_2cmTwinGun => true,
			EquipmentId.MainGunMedium_15_2cmTwinGunKai => true,
			_ => false
		});

		return Math.Sqrt(twin) * 2.0 + Math.Sqrt(single);
	}

	public static double GetItalianDamageBonus(this IShipData ship)
	{
		return ship.MasterShip.ShipId switch
		{
			ShipId.Zara or
			ShipId.ZaraKai or
			ShipId.ZaraDue or
			ShipId.Pola or
			ShipId.PolaKai
				=> Math.Sqrt(ship.AllSlotInstance.Count(e => e?.EquipmentId == EquipmentId.MainGunMedium_203mm53TwinGun)),

			_ => 0,
		};
	}

	public static ShipNationality Nationality(this IShipDataMaster ship)
	{
		if (ship.IsAbyssalShip) return ShipNationality.Unknown;

		return ship.SortID switch
		{
			< 1000 => ShipNationality.Unknown,
			< 30000 => ShipNationality.Japanese,
			< 31000 => ShipNationality.German,
			< 32000 => ShipNationality.Italian,
			< 33000 => ShipNationality.American,
			< 34000 => ShipNationality.British,
			< 35000 => ShipNationality.French,
			< 36000 => ShipNationality.Russian,
			< 37000 => ShipNationality.Swedish,
			< 38000 => ShipNationality.Dutch,
			< 39000 => ShipNationality.Australian,

			_ => ShipNationality.Unknown
		};
	}

	public static bool CanAttackSubmarine(this IShipData ship) => ship.MasterShip.ShipType switch
	{
		ShipTypes.Escort or
		ShipTypes.Destroyer or
		ShipTypes.LightCruiser or
		ShipTypes.TorpedoCruiser or
		ShipTypes.TrainingCruiser or
		ShipTypes.FleetOiler
			=> ship.ASWBase > 0,

		ShipTypes.AviationCruiser or
		ShipTypes.LightAircraftCarrier or
		ShipTypes.AviationBattleship or
		ShipTypes.SeaplaneTender or
		ShipTypes.AmphibiousAssaultShip
			=> ship.AllSlotInstance.Zip(ship.Aircraft, (Equipment, Size) => (Equipment, Size)).Any(s => s.Equipment is { MasterEquipment.IsAntiSubmarineAircraft: true } && s.Size > 0),

		ShipTypes.AircraftCarrier =>
			ship.MasterShip.ShipId is ShipId.KagaKaiNiGo &&
			ship.AllSlotInstance.Zip(ship.Aircraft, (Equipment, Size) => (Equipment, Size)).Any(s => s.Equipment is { MasterEquipment.IsAntiSubmarineAircraft: true } && s.Size > 0),

		_ => false
	};

	public static bool CanNoSonarOpeningAsw(this IShipData ship) => ship.MasterShip is
	{ ShipId: ShipId.JervisKai } or
	{ ShipId: ShipId.JanusKai } or
	{ ShipId: ShipId.JavelinKai } or
	{ ShipId: ShipId.SamuelBRobertsKai } or
	{ ShipId: ShipId.SamuelBRobertsMkII } or
	{ ShipId: ShipId.IsuzuKaiNi } or
	{ ShipId: ShipId.TatsutaKaiNi } or
	{ ShipId: ShipId.YuubariKaiNiD } or
	{ ShipClassTyped: ShipClass.Fletcher, ShipId: not (ShipId.HeywoodLE or ShipId.RichardPLeary) };

	public static bool CanDoOpeningAsw(this IShipData ship) => ship switch
	{
		_ when !ship.CanAttackSubmarine() => false,
		_ when ship.CanNoSonarOpeningAsw() => true,

		{ MasterShip.ShipId: ShipId.HyuugaKaiNi } => ship.HyuugaK2OpeningAswCondition(),

		// if they can attack subs, they can do opening ASW
		{ MasterShip.ShipId: ShipId.TaiyouKai } or
		{ MasterShip.ShipId: ShipId.TaiyouKaiNi } or
		{ MasterShip.ShipId: ShipId.ShinyouKai } or
		{ MasterShip.ShipId: ShipId.ShinyouKaiNi } or
		{ MasterShip.ShipId: ShipId.UnyouKai } or
		{ MasterShip.ShipId: ShipId.UnyouKaiNi } or
		{ MasterShip.ShipId: ShipId.KagaKaiNiGo }
			=> true,

		{ MasterShip.ShipType: ShipTypes.LightAircraftCarrier } => ship.ASWTotal switch
		{
			>= 100 => ship.AswCondition100() || ship.AswCondition65(),
			>= 65 => ship.AswCondition65(),
			>= 50 => ship.MasterShip.ShipId switch
			{
				ShipId.SuzuyaCVLKaiNi or ShipId.KumanoCVLKaiNi => false,
				_ => ship.HasSonar() && (ship.HasSpecialAntiSubmarineAttacker() || ship.HasAswPatrolAircraft()),
			},
			_ => false,
		},

		{ MasterShip.ShipId: ShipId.ShinshuuMaruKai } or
		{ MasterShip.ShipId: ShipId.FusouKaiNi } or
		{ MasterShip.ShipId: ShipId.YamashiroKaiNi } or
		{ MasterShip.ShipId: ShipId.YamatoKaiNiJuu } or
		{ MasterShip.ShipClassTyped: ShipClass.KumanoMaru }
			=> ship.HasSonar() && ship.HasAntiSubmarineAircraft() && ship.ASWTotal >= 100,

		{ MasterShip.ShipType: ShipTypes.Destroyer } or
		{ MasterShip.ShipType: ShipTypes.LightCruiser } or
		{ MasterShip.ShipType: ShipTypes.TrainingCruiser } or
		{ MasterShip.ShipType: ShipTypes.TorpedoCruiser } or
		{ MasterShip.ShipType: ShipTypes.FleetOiler }
			=> ship.HasSonar() && ship.ASWTotal >= 100,

		{ MasterShip.ShipType: ShipTypes.Escort } => ship.ASWTotal switch
		{
			>= 75 => ship.AllSlotInstance.Sum(e => e?.MasterEquipment.ASW ?? 0) >= 4,
			>= 60 => ship.HasSonar(),

			_ => false,
		},

		_ => false,
	};

	public static bool AswCondition65(this IShipData ship) => ship.HasSpecialAntiSubmarineAttacker() || ship.HasAswPatrolAircraft();

	public static bool AswCondition100(this IShipData ship) => ship.HasSonar() && ship.HasAntiSubmarineAircraft();

	private static bool HyuugaK2OpeningAswCondition(this IShipData ship)
	{
		List<IEquipmentData?> eqs = ship.AllSlotInstance
			.Where(eq => eq is not null)
			.ToList();

		if (eqs.Count(eq => eq!.EquipmentId is
				EquipmentId.Autogyro_KaTypeObservationAutogyro or
				EquipmentId.Autogyro_OTypeObservationAutogyroKai or
				EquipmentId.Autogyro_OTypeObservationAutogyroKaiNi)
			>= 2)
		{
			return true;
		}

		return eqs.Any(eq => eq!.EquipmentId is
			EquipmentId.Autogyro_S51J or
			EquipmentId.Autogyro_S51JKai);
	}

	public static bool CanSink(this IShipData ship, IFleetData fleet)
	{
		if (ship.HPRate > 0.25) return false;
		if (fleet.MembersInstance.FirstOrDefault() == ship) return false;
		if (ship.HasDamecon()) return false;
		if (ship.RepairingDockID > -1) return false;

		return fleet.MembersWithoutEscaped!.Contains(ship);
	}

	private static bool HasDamecon(this IShipData ship) => ship.AllSlotInstance
		.Any(e => e?.MasterEquipment.CategoryType is EquipmentTypes.DamageControl);

	public static DamageState GetDamageState(this IShipData ship) => ship.HPRate switch
	{
		> 0.75 => DamageState.Healthy,
		> 0.5 => DamageState.Light,
		> 0.25 => DamageState.Medium,
		> 0 => DamageState.Heavy,
		_ => DamageState.Sunk,
	};

	public static bool CanEquipDaihatsu(this IShipData ship)
		=> ship.MasterShip.EquippableCategoriesTyped.Contains(EquipmentTypes.LandingCraft);

	public static bool CanEquipTank(this IShipData ship)
		=> ship.MasterShip.EquippableCategoriesTyped.Contains(EquipmentTypes.SpecialAmphibiousTank);

	public static bool CanEquipFcf(this IShipData ship)
		=> ship.MasterShip.EquippableCategoriesTyped.Contains(EquipmentTypes.CommandFacility);

	private static List<EquipmentTypes> BulgeTypes { get; } =
	[
		EquipmentTypes.ExtraArmor,
		EquipmentTypes.ExtraArmorMedium,
		EquipmentTypes.ExtraArmorLarge,
	];
	
	public static bool CanEquipBulge(this IShipData ship)
		=> ship.MasterShip.EquippableCategoriesTyped.Intersect(BulgeTypes).Any();
	
	public static bool CanEquipSeaplaneFighter(this IShipData ship)
		=> ship.MasterShip.EquippableCategoriesTyped.Contains(EquipmentTypes.SeaplaneFighter);

	public static bool IsFinalRemodel(this IShipData ship) => ship.MasterShip.IsFinalRemodel();

	public static bool IsFinalRemodel(this IShipDataMaster ship)
	{
		if (ship.RemodelAfterShipID <= 0) return true;

		IShipDataMaster? masterShip = ship;
		List<ShipId> visitedIds = [];
		List<ShipId> finalRemodelIds = [];

		while (true)
		{
			if (masterShip is null) return false;
			if (finalRemodelIds.Contains(ship.ShipId)) return true;
			if (finalRemodelIds.Contains(masterShip.ShipId)) return false;

			if (visitedIds.Contains(masterShip.ShipId))
			{
				finalRemodelIds.Add(masterShip.ShipId);
			}
			else
			{
				visitedIds.Add(masterShip.ShipId);
			}

			masterShip = masterShip.RemodelAfterShip;
		}
	}
}
