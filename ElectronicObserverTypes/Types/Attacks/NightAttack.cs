using ElectronicObserver.Core.Types.Attacks;

namespace ElectronicObserver.Core.Types.Attacks;

public record NightAttack : Attack
{
	/// <summary>
	/// API ID
	/// </summary>
	public NightAttackKind NightAttackKind { get; protected init; }

	public int RateModifier { get; protected init; }
	public double NumberOfAttacks { get; protected init; }

	public static NightAttack NormalAttack { get; } = new()
	{
		NightAttackKind = NightAttackKind.NormalAttack,
		PowerModifier = 1,
		AccuracyModifier = 1,
		RateModifier = 0,
		NumberOfAttacks = 1,
	};

	public static NightAttack DoubleShelling { get; } = new()
	{
		NightAttackKind = NightAttackKind.DoubleShelling,
		PowerModifier = 1.2,
		AccuracyModifier = 1.1,
		RateModifier = 0,
		NumberOfAttacks = 2,
	};

	public static NightAttack CutinMainTorpedo { get; } = new()
	{
		NightAttackKind = NightAttackKind.CutinMainTorpedo,
		PowerModifier = 1.3,
		AccuracyModifier = 1.5,
		RateModifier = 115,
		NumberOfAttacks = 2,
	};

	public static NightAttack CutinTorpedoTorpedo { get; } = new()
	{
		NightAttackKind = NightAttackKind.CutinTorpedoTorpedo,
		PowerModifier = 1.5,
		AccuracyModifier = 1.65,
		RateModifier = 122,
		NumberOfAttacks = 2,
	};

	public static NightAttack CutinMainSub { get; } = new()
	{
		NightAttackKind = NightAttackKind.CutinMainSub,
		PowerModifier = 1.75,
		AccuracyModifier = 1.5,
		RateModifier = 130,
		NumberOfAttacks = 1,
	};

	public static NightAttack CutinMainMain { get; } = new()
	{
		NightAttackKind = NightAttackKind.CutinMainMain,
		PowerModifier = 2,
		AccuracyModifier = 2,
		RateModifier = 140,
		NumberOfAttacks = 1,
	};

	public static NightAttack CutinTorpedoRadar { get; } = new()
	{
		NightAttackKind = NightAttackKind.CutinTorpedoRadar,
		PowerModifier = 1.3,
		AccuracyModifier = 1,
		RateModifier = 115,
		NumberOfAttacks = 1,
	};

	public static NightAttack CutinTorpedoPicket { get; } = new()
	{
		NightAttackKind = NightAttackKind.CutinTorpedoPicket,
		PowerModifier = 1.2,
		AccuracyModifier = 1,
		RateModifier = 150,
		NumberOfAttacks = 1,
	};

	public static NightAttack CutinTorpedoDestroyerPicket { get; } = new()
	{
		NightAttackKind = NightAttackKind.CutinTorpedoDestroyerPicket,
		PowerModifier = 1.5,
		AccuracyModifier = 1,
		RateModifier = 125,
		NumberOfAttacks = 1,
	};

	public static NightAttack CutinTorpedoDrum { get; } = new()
	{
		NightAttackKind = NightAttackKind.CutinTorpedoDrum,
		PowerModifier = 1.3,
		AccuracyModifier = 1,
		RateModifier = 122,
		NumberOfAttacks = 1,
	};

	public static NightAttack CutinTorpedoRadar2 { get; } = new()
	{
		NightAttackKind = NightAttackKind.CutinTorpedoRadar2,
		PowerModifier = 1.3,
		AccuracyModifier = 1,
		RateModifier = 115,
		NumberOfAttacks = 2,
	};

	public static NightAttack CutinTorpedoPicket2 { get; } = new()
	{
		NightAttackKind = NightAttackKind.CutinTorpedoPicket2,
		PowerModifier = 1.2,
		AccuracyModifier = 1,
		RateModifier = 150,
		NumberOfAttacks = 2,
	};

	public static NightAttack CutinTorpedoDestroyerPicket2 { get; } = new()
	{
		NightAttackKind = NightAttackKind.CutinTorpedoDestroyerPicket2,
		PowerModifier = 1.5,
		AccuracyModifier = 1,
		RateModifier = 125,
		NumberOfAttacks = 2,
	};

	public static NightAttack CutinTorpedoDrum2 { get; } = new()
	{
		NightAttackKind = NightAttackKind.CutinTorpedoDrum2,
		PowerModifier = 1.3,
		AccuracyModifier = 1,
		RateModifier = 122,
		NumberOfAttacks = 2,
	};

	public static NightAttack CutinZuiun { get; } = new()
	{
		NightAttackKind = NightAttackKind.CutinZuiun,
		PowerModifier = 1.24,
		AccuracyModifier = 1,
		RateModifier = 0,
		NumberOfAttacks = 2,
	};

	public static NightAttack Shelling { get; } = new()
	{
		NightAttackKind = NightAttackKind.Shelling,
		PowerModifier = 1,
		AccuracyModifier = 1,
		RateModifier = 0,
		NumberOfAttacks = 1,
	};

	public static NightAttack AirAttack { get; } = new()
	{
		NightAttackKind = NightAttackKind.AirAttack,
		PowerModifier = 1,
		AccuracyModifier = 1,
		RateModifier = 0,
		NumberOfAttacks = 1,
	};

	public static NightAttack DepthCharge { get; } = new()
	{
		NightAttackKind = NightAttackKind.DepthCharge,
		PowerModifier = 1,
		AccuracyModifier = 1,
		RateModifier = 0,
		NumberOfAttacks = 1,
	};

	public static NightAttack Torpedo { get; } = new()
	{
		NightAttackKind = NightAttackKind.Torpedo,
		PowerModifier = 1,
		AccuracyModifier = 1,
		RateModifier = 0,
		NumberOfAttacks = 1,
	};

	public static NightAttack Rocket { get; } = new()
	{
		NightAttackKind = NightAttackKind.Rocket,
		PowerModifier = 1,
		AccuracyModifier = 1,
		RateModifier = 0,
		NumberOfAttacks = 1,
	};

	public static NightAttack LandingDaihatsu { get; } = new()
	{
		NightAttackKind = NightAttackKind.LandingDaihatsu,
		PowerModifier = 1,
		AccuracyModifier = 1,
		RateModifier = 0,
		NumberOfAttacks = 1,
	};

	public static NightAttack LandingTokuDaihatsu { get; } = new()
	{
		NightAttackKind = NightAttackKind.LandingTokuDaihatsu,
		PowerModifier = 1,
		AccuracyModifier = 1,
		RateModifier = 0,
		NumberOfAttacks = 1,
	};

	public static NightAttack LandingDaihatsuTank { get; } = new()
	{
		NightAttackKind = NightAttackKind.LandingDaihatsuTank,
		PowerModifier = 1,
		AccuracyModifier = 1,
		RateModifier = 0,
		NumberOfAttacks = 1,
	};

	public static NightAttack LandingAmphibious { get; } = new()
	{
		NightAttackKind = NightAttackKind.LandingAmphibious,
		PowerModifier = 1,
		AccuracyModifier = 1,
		RateModifier = 0,
		NumberOfAttacks = 1,
	};

	public static NightAttack LandingTokuDaihatsuTank { get; } = new()
	{
		NightAttackKind = NightAttackKind.LandingTokuDaihatsuTank,
		PowerModifier = 1,
		AccuracyModifier = 1,
		RateModifier = 0,
		NumberOfAttacks = 1,
	};

	protected NightAttack()
	{

	}

	public virtual string Display => AttackDisplay(NightAttackKind);

	/// <summary>
	/// 夜戦攻撃種別を表す文字列を取得します。
	/// </summary>
	public static string AttackDisplay(NightAttackKind attack) => attack switch
	{
		NightAttackKind.NormalAttack => AttackResources.NormalAttack,
		NightAttackKind.DoubleShelling => AttackResources.DoubleShelling,
		NightAttackKind.CutinMainTorpedo => AttackResources.CutinMainTorpedo,
		NightAttackKind.CutinTorpedoTorpedo => AttackResources.CutinTorpedoTorpedo,
		NightAttackKind.CutinMainSub => AttackResources.CutinNightMainSub,
		NightAttackKind.CutinMainMain => AttackResources.CutinNightMainMain,
		NightAttackKind.CutinAirAttack => AttackResources.CutinAirAttack,

		NightAttackKind.CutinTorpedoRadar or
		NightAttackKind.CutinTorpedoRadar2 => AttackResources.CutinTorpedoRadar,

		NightAttackKind.CutinTorpedoPicket or
		NightAttackKind.CutinTorpedoPicket2 => AttackResources.CutinTorpedoPicket,

		NightAttackKind.CutinTorpedoDestroyerPicket or
		NightAttackKind.CutinTorpedoDestroyerPicket2 => AttackResources.CutinTorpedoDestroyerPicket,

		NightAttackKind.CutinTorpedoDrum or
		NightAttackKind.CutinTorpedoDrum2 => AttackResources.CutinTorpedoDrum,

		NightAttackKind.SpecialNelson => AttackResources.SpecialNelson,
		NightAttackKind.SpecialNagato => AttackResources.SpecialNagato,
		NightAttackKind.SpecialMutsu => AttackResources.SpecialMutsu,
		NightAttackKind.SpecialColorado => AttackResources.SpecialColorado,
		NightAttackKind.SpecialKongou => AttackResources.SpecialKongou,
		NightAttackKind.SpecialRichelieu => AttackResources.SpecialRichelieu,
		NightAttackKind.SpecialQueenElizabethClass => AttackResources.SpecialQueenElizabeth,

		NightAttackKind.CutinZuiun => AttackResources.CutinZuiun,

		NightAttackKind.SpecialSubmarineTender23 => AttackResources.SpecialSubmarineTender23,
		NightAttackKind.SpecialSubmarineTender34 => AttackResources.SpecialSubmarineTender34,
		NightAttackKind.SpecialSubmarineTender24 => AttackResources.SpecialSubmarineTender24,

		NightAttackKind.SpecialYamato3Ships => AttackResources.SpecialYamato123,
		NightAttackKind.SpecialYamato2Ships => AttackResources.SpecialYamato12,

		NightAttackKind.Shelling => AttackResources.Shelling,
		NightAttackKind.AirAttack => AttackResources.AirAttack,
		NightAttackKind.DepthCharge => AttackResources.DepthChargeAttack,
		NightAttackKind.Torpedo => AttackResources.TorpedoAttack,

		NightAttackKind.Rocket => AttackResources.RocketAttack,
		NightAttackKind.LandingDaihatsu => AttackResources.LandingDaihatsu,
		NightAttackKind.LandingTokuDaihatsu => AttackResources.LandingTokuDaihatsu,
		NightAttackKind.LandingDaihatsuTank => AttackResources.LandingDaihatsuTank,
		NightAttackKind.LandingAmphibious => AttackResources.TankAttack,
		NightAttackKind.LandingTokuDaihatsuTank => AttackResources.LandingTokuDaihatsuTank,

		_ => $"{AttackResources.Unknown}({(int)attack})",
	};
}
