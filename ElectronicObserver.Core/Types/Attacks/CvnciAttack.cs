namespace ElectronicObserver.Core.Types.Attacks;

public sealed record CvnciAttack : NightAttack
{
	public CvnciKind CvnciKind { get; private init; }

	public static CvnciAttack CutinAirAttackFighterFighterAttacker { get; } = new()
	{
		NightAttackKind = NightAttackKind.CutinAirAttack,
		CvnciKind = CvnciKind.FighterFighterAttacker,
		PowerModifier = 1.25,
		AccuracyModifier = 1,
		RateModifier = 105,
		NumberOfAttacks = 1,
	};

	public static CvnciAttack CutinAirAttackFighterAttacker { get; } = new()
	{
		NightAttackKind = NightAttackKind.CutinAirAttack,
		CvnciKind = CvnciKind.FighterAttacker,
		PowerModifier = 1.2,
		AccuracyModifier = 1,
		RateModifier = 115,
		NumberOfAttacks = 1,
	};

	public static CvnciAttack CutinAirAttackPhototube { get; } = new()
	{
		NightAttackKind = NightAttackKind.CutinAirAttack,
		CvnciKind = CvnciKind.Phototube,
		PowerModifier = 1.2,
		AccuracyModifier = 1,
		RateModifier = 115,
		NumberOfAttacks = 1,
	};

	public static CvnciAttack CutinAirAttackFighterOtherOther { get; } = new()
	{
		NightAttackKind = NightAttackKind.CutinAirAttack,
		CvnciKind = CvnciKind.FighterOtherOther,
		PowerModifier = 1.18,
		AccuracyModifier = 1,
		RateModifier = 125,
		NumberOfAttacks = 1,
	};

	private CvnciAttack()
	{

	}

	public override string Display => CvnciDisplay(CvnciKind);

	private static string CvnciDisplay(CvnciKind cvnci) => cvnci switch
	{
		CvnciKind.FighterFighterAttacker => AttackResources.CvnciFfa,
		CvnciKind.FighterAttacker => AttackResources.CvnciFa,
		CvnciKind.Phototube => AttackResources.CvnciPhoto,
		CvnciKind.FighterOtherOther => AttackResources.CvnciFoo,

		_ => $"{AttackResources.Unknown}({(int)cvnci})",
	};
}
