using ElectronicObserver.Core.Types.Attacks;

namespace ElectronicObserver.Core.Types.Attacks;

public record DayAttack
{
	/// <summary>
	/// API ID
	/// </summary>
	public DayAttackKind DayAttackKind { get; protected init; }

	public virtual string Display => AttackDisplay(DayAttackKind);

	protected DayAttack()
	{

	}

	/// <summary>
	/// 昼戦攻撃種別を表す文字列を取得します。
	/// </summary>
	public static string AttackDisplay(DayAttackKind id) => id switch
	{
		DayAttackKind.NormalAttack => AttackResources.NormalAttack,
		DayAttackKind.Laser => AttackResources.LaserAttack,
		DayAttackKind.DoubleShelling => AttackResources.DoubleShelling,
		DayAttackKind.CutinMainSub => AttackResources.CutinMainSub,
		DayAttackKind.CutinMainRadar => AttackResources.CutinMainRadar,
		DayAttackKind.CutinMainAP => AttackResources.CutinMainAp,
		DayAttackKind.CutinMainMain => AttackResources.CutinMainMain,
		DayAttackKind.CutinAirAttack => AttackResources.CutinAirAttack,
		DayAttackKind.SpecialNelson => AttackResources.SpecialNelson,
		DayAttackKind.SpecialNagato => AttackResources.SpecialNagato,
		DayAttackKind.SpecialMutsu => AttackResources.SpecialMutsu,
		DayAttackKind.SpecialColorado => AttackResources.SpecialColorado,
		DayAttackKind.SpecialKongo => AttackResources.SpecialKongou,
		DayAttackKind.SpecialRichelieu => AttackResources.SpecialRichelieu,
		DayAttackKind.SpecialQueenElizabethClass => AttackResources.SpecialQueenElizabeth,
		DayAttackKind.ZuiunMultiAngle => AttackResources.ZuiunMultiAngle,
		DayAttackKind.SeaAirMultiAngle => AttackResources.SeaAirMultiAngle,
		DayAttackKind.SpecialSubmarineTender23 => AttackResources.SpecialSubmarineTender23,
		DayAttackKind.SpecialSubmarineTender34 => AttackResources.SpecialSubmarineTender34,
		DayAttackKind.SpecialSubmarineTender24 => AttackResources.SpecialSubmarineTender24,
		DayAttackKind.SpecialYamato3Ships => AttackResources.SpecialYamato123,
		DayAttackKind.SpecialYamato2Ships => AttackResources.SpecialYamato12,
		DayAttackKind.Shelling => AttackResources.Shelling,
		DayAttackKind.AirAttack => AttackResources.AirAttack,
		DayAttackKind.DepthCharge => AttackResources.DepthChargeAttack,
		DayAttackKind.Torpedo => AttackResources.TorpedoAttack,
		DayAttackKind.Rocket => AttackResources.RocketAttack,
		DayAttackKind.LandingDaihatsu => AttackResources.LandingDaihatsu,
		DayAttackKind.LandingTokuDaihatsu => AttackResources.LandingTokuDaihatsu,
		DayAttackKind.LandingDaihatsuTank => AttackResources.LandingDaihatsuTank,
		DayAttackKind.LandingAmphibious => AttackResources.TankAttack,
		DayAttackKind.LandingTokuDaihatsuTank => AttackResources.LandingTokuDaihatsuTank,

		_ => $"{AttackResources.Unknown}({(int)id})",
	};
}
