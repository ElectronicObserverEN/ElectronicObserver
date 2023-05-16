namespace ElectronicObserverTypes.Extensions;

public static class EquipmentDataExtensions
{
	/// <summary> 砲系かどうか </summary>
	public static bool IsGun(this IEquipmentDataMaster equip) => equip.CategoryType is
		EquipmentTypes.MainGunSmall or
		EquipmentTypes.MainGunMedium or
		EquipmentTypes.MainGunLarge or
		EquipmentTypes.MainGunLarge2 or
		EquipmentTypes.SecondaryGun;

	/// <summary> 主砲系かどうか </summary>
	public static bool IsMainGun(this IEquipmentDataMaster equip) => equip.CategoryType is
		EquipmentTypes.MainGunSmall or
		EquipmentTypes.MainGunMedium or
		EquipmentTypes.MainGunLarge or
		EquipmentTypes.MainGunLarge2;

	/// <summary> 副砲系かどうか </summary>
	public static bool IsSecondaryGun(this IEquipmentDataMaster equip) => equip.CategoryType is
		EquipmentTypes.SecondaryGun or
		EquipmentTypes.SecondaryGun2;

	/// <summary> 魚雷系かどうか </summary>
	public static bool IsTorpedo(this IEquipmentDataMaster equip) => equip.CategoryType is
		EquipmentTypes.Torpedo or
		EquipmentTypes.SubmarineTorpedo;

	/// <summary> 後期型魚雷かどうか </summary>
	public static bool IsLateModelTorpedo(this IEquipmentDataMaster equip) => equip.EquipmentId is
		EquipmentId.SubmarineTorpedo_LateModelBowTorpedo_6tubes or
		EquipmentId.SubmarineTorpedo_SkilledSonarPersonnel_LateModelBowTorpedo_6tubes or
		EquipmentId.SubmarineTorpedo_LateModel53cmBowTorpedo_8tubes or
		EquipmentId.SubmarineTorpedo_21inch6tubeBowTorpedoLauncher_LateModel or
		EquipmentId.SubmarineTorpedo_Submarine4tubeSternTorpedoLauncher_LateModel or
		EquipmentId.SubmarineTorpedo_LateModelBowTorpedoMount_4tubes or
		EquipmentId.SubmarineTorpedo_SkilledSonarPersonnel_LateModelBowTorpedoMount_4tubes;

	/// <summary> 高角砲かどうか </summary>
	public static bool IsHighAngleGun(this IEquipmentDataMaster equip) => equip.IconTypeTyped is
		EquipmentIconType.HighAngleGun;

	/// <summary> 高角砲+高射装置かどうか </summary>
	public static bool IsHighAngleGunWithAADirector(this IEquipmentDataMaster equip) =>
		equip.IsHighAngleGun() &&
		equip.AA >= 8;

	/// <summary> 集中配備機銃かどうか </summary>
	public static bool IsConcentratedAAGun(this IEquipmentDataMaster equip) =>
		equip.CategoryType is EquipmentTypes.AAGun &&
		equip.AA >= 9;

	/// <summary> 航空機かどうか </summary>
	public static bool IsAircraft(this IEquipmentDataMaster equip) => equip.CategoryType is
		EquipmentTypes.CarrierBasedFighter or
		EquipmentTypes.CarrierBasedBomber or
		EquipmentTypes.CarrierBasedTorpedo or
		EquipmentTypes.SeaplaneBomber or
		EquipmentTypes.Autogyro or
		EquipmentTypes.ASPatrol or
		EquipmentTypes.SeaplaneFighter or
		EquipmentTypes.LandBasedAttacker or
		EquipmentTypes.Interceptor or
		EquipmentTypes.HeavyBomber or
		EquipmentTypes.JetFighter or
		EquipmentTypes.JetBomber or
		EquipmentTypes.JetTorpedo or
		EquipmentTypes.CarrierBasedRecon or
		EquipmentTypes.SeaplaneRecon or
		EquipmentTypes.FlyingBoat or
		EquipmentTypes.LandBasedRecon or
		EquipmentTypes.JetRecon;

	/// <summary> 戦闘に参加する航空機かどうか </summary>
	public static bool IsCombatAircraft(this IEquipmentDataMaster equip) => equip.CategoryType is
		EquipmentTypes.CarrierBasedFighter or
		EquipmentTypes.CarrierBasedBomber or
		EquipmentTypes.CarrierBasedTorpedo or
		EquipmentTypes.SeaplaneBomber or
		EquipmentTypes.Autogyro or
		EquipmentTypes.ASPatrol or
		EquipmentTypes.SeaplaneFighter or
		EquipmentTypes.LandBasedAttacker or
		EquipmentTypes.Interceptor or
		EquipmentTypes.HeavyBomber or
		EquipmentTypes.JetFighter or
		EquipmentTypes.JetBomber or
		EquipmentTypes.JetTorpedo;

	/// <summary> 偵察機かどうか </summary>
	public static bool IsReconAircraft(this IEquipmentDataMaster equip) => equip.CategoryType is
		EquipmentTypes.CarrierBasedRecon or
		EquipmentTypes.SeaplaneRecon or
		EquipmentTypes.FlyingBoat or
		EquipmentTypes.LandBasedRecon or
		EquipmentTypes.JetRecon;

	/// <summary> 対潜攻撃可能な航空機かどうか </summary>
	public static bool IsAntiSubmarineAircraft(this IEquipmentDataMaster equip) => equip.ASW > 0 &&
		equip.CategoryType is
			EquipmentTypes.CarrierBasedBomber or
			EquipmentTypes.CarrierBasedTorpedo or
			EquipmentTypes.SeaplaneBomber or
			EquipmentTypes.Autogyro or
			EquipmentTypes.ASPatrol or
			EquipmentTypes.FlyingBoat or
			EquipmentTypes.LandBasedAttacker or
			EquipmentTypes.HeavyBomber or
			EquipmentTypes.JetBomber or
			EquipmentTypes.JetTorpedo;

	/// <summary> 夜間行動可能な航空機かどうか </summary>
	public static bool IsNightAircraft(this IEquipmentDataMaster equip) =>
		equip.IsNightFighter() || equip.IsNightAttacker();

	/// <summary> 夜間戦闘機かどうか </summary>
	public static bool IsNightFighter(this IEquipmentDataMaster equip) =>
		equip.IconTypeTyped is EquipmentIconType.NightFighter;

	/// <summary> 夜間攻撃機かどうか </summary>
	public static bool IsNightAttacker(this IEquipmentDataMaster equip) =>
		equip.IconTypeTyped == EquipmentIconType.NightAttacker;

	/// <summary> Swordfish 系艦上攻撃機かどうか </summary>
	public static bool IsSwordfish(this IEquipmentDataMaster equip) =>
		equip.CategoryType is EquipmentTypes.CarrierBasedTorpedo &&
		equip.Name.Contains("Swordfish");

	/// <summary> 電探かどうか </summary>
	public static bool IsRadar(this IEquipmentDataMaster equip) => equip.CategoryType is
		EquipmentTypes.RadarSmall or
		EquipmentTypes.RadarLarge or
		EquipmentTypes.RadarLarge2;

	/// <summary> 対空電探かどうか </summary>
	public static bool IsAirRadar(this IEquipmentDataMaster equip) =>
		equip.IsRadar() && equip.AA >= 2;

	/// <summary> 水上電探かどうか </summary>
	public static bool IsSurfaceRadar(this IEquipmentDataMaster equip) =>
		equip.IsRadar() && equip.LOS >= 5;

	/// <summary> ソナーかどうか </summary>
	public static bool IsSonar(this IEquipmentDataMaster equip) => equip.CategoryType is
		EquipmentTypes.Sonar or
		EquipmentTypes.SonarLarge;

	/// <summary> 爆雷かどうか(投射機は含まない) </summary>
	public static bool IsDepthCharge(this IEquipmentDataMaster equip) => equip.EquipmentId is
		EquipmentId.DepthCharge_Type95DepthCharge or
		EquipmentId.DepthCharge_Type2DepthCharge or
		EquipmentId.DepthCharge_LightweightASWTorpedo_InitialTestModel or
		EquipmentId.DepthCharge_Hedgehog_InitialModel or
		EquipmentId.DepthCharge_Type2DepthChargeKaiNi;

	/// <summary> 爆雷投射機かどうか(爆雷/対潜迫撃砲は含まない) </summary>
	public static bool IsDepthChargeProjector(this IEquipmentDataMaster equip) => equip.EquipmentId is
		EquipmentId.DepthCharge_Type94DepthChargeProjector or
		EquipmentId.DepthCharge_Type3DepthChargeProjector or
		EquipmentId.DepthCharge_Type3DepthChargeProjector_CD or
		EquipmentId.DepthCharge_Prototype15cm9tubeASWRocketLauncher or
		EquipmentId.DepthCharge_RUR4AWeaponAlphaKai or
		EquipmentId.DepthCharge_Mk_32ASWTorpedo_Mk_2Thrower;

	public static bool IsAswMortar(this IEquipmentDataMaster equip) => equip.EquipmentId is
		EquipmentId.DepthCharge_Type212cmMortarKai or
		EquipmentId.DepthCharge_Type212cmMortarKai_ConcentratedDeployment;

	/// <summary> 夜間作戦航空要員かどうか </summary>
	public static bool IsNightAviationPersonnel(this IEquipmentDataMaster equip) => equip.EquipmentId is
		EquipmentId.AviationPersonnel_NightOperationAviationPersonnel or
		EquipmentId.AviationPersonnel_NightOperationAviationPersonnel_SkilledCrew;

	/// <summary> 高高度局戦かどうか </summary>
	public static bool IsHightAltitudeFighter(this IEquipmentDataMaster equip) => equip.EquipmentId is
		EquipmentId.Interceptor_Me163B or
		EquipmentId.Interceptor_PrototypeShuusui or
		EquipmentId.Interceptor_Shuusui;

	/// <summary> 対空噴進弾幕が発動可能なロケットランチャーかどうか </summary>
	public static bool IsAARocketLauncher(this IEquipmentDataMaster equip) => equip.EquipmentId is
		EquipmentId.AAGun_12cm30tubeRocketLauncherKaiNi;

	public static bool IsSeaplane(this IEquipmentDataMaster equip) => equip.CategoryType switch
	{
		EquipmentTypes.SeaplaneRecon => true,
		EquipmentTypes.SeaplaneBomber => true,
		_ => false
	};

	public static bool IsNightSeaplane(this IEquipmentDataMaster equip) =>
		equip.IconTypeTyped is EquipmentIconType.NightSeaplane;

	public static bool IsSuisei634(this IEquipmentData? equip) => equip?.EquipmentId switch
	{
		EquipmentId.CarrierBasedBomber_SuiseiModel22_634AirGroup => true,
		EquipmentId.CarrierBasedBomber_SuiseiModel22_634AirGroupSkilled => true,
		EquipmentId.CarrierBasedBomber_SuiseiModel12_634AirGroupwType3ClusterBombs => true,
		_ => false
	};

	public static bool IsZuiun(this IEquipmentData? equip) => equip?.EquipmentId is
		EquipmentId.SeaplaneBomber_Zuiun or
		EquipmentId.SeaplaneBomber_Zuiun_634AirGroup or
		EquipmentId.SeaplaneBomber_ZuiunModel12 or
		EquipmentId.SeaplaneBomber_ZuiunModel12_634AirGroup or
		EquipmentId.SeaplaneBomber_Zuiun_631AirGroup or
		EquipmentId.SeaplaneBomber_Zuiun_634AirGroupSkilled or
		EquipmentId.SeaplaneBomber_ZuiunKaiNi_634AirGroup or
		EquipmentId.SeaplaneBomber_ZuiunKaiNi_634AirGroupSkilled or
		EquipmentId.SeaplaneBomber_PrototypeNightZuiun_AttackEquipment;

	/// <summary>
	/// Aircraft that aren't night aircraft but can still participate in cvnci
	/// </summary>
	public static bool IsNightCapableAircraft(this IEquipmentData? equip) =>
		equip?.MasterEquipment.IsSwordfish == true ||
		equip?.EquipmentId == EquipmentId.CarrierBasedBomber_SuiseiModel12_wType31PhotoelectricFuzeBombs ||
		equip?.EquipmentId == EquipmentId.CarrierBasedBomber_ZeroFighterbomberModel62_IwaiSquadron;

	public static bool UsesSlotSpace(this IEquipmentDataMaster equip) => equip.CategoryType
		is not (EquipmentTypes.Ration or EquipmentTypes.DamageControl or EquipmentTypes.Supplies);

	public static int AirBaseAircraftCount(this IEquipmentDataMaster? equipment) => equipment?.CategoryType switch
	{
		null => 0,

		EquipmentTypes.CarrierBasedRecon or
		EquipmentTypes.CarrierBasedRecon2 or
		EquipmentTypes.LandBasedRecon or
		EquipmentTypes.SeaplaneRecon or
		EquipmentTypes.FlyingBoat => 4,

		EquipmentTypes.HeavyBomber => 9,

		_ => 18,
	};
}
