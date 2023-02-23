namespace ElectronicObserverTypes.Attacks;

public sealed record NightZuiunCutinAttack : NightAttack
{
	public NightZuiunCutinKind NightZuiunCutinKind { get; private init; }

	public static NightZuiunCutinAttack NightZuiunCutinZuiun { get; } = new()
	{
		NightAttackKind = NightAttackKind.CutinZuiun,
		NightZuiunCutinKind = NightZuiunCutinKind.Zuiun,
		PowerModifier = 1.24,
		AccuracyModifier = 1,
		RateModifier = 160,
		NumberOfAttacks = 2,
	};

	public static NightZuiunCutinAttack NightZuiunCutinZuiunRadar { get; } = new()
	{
		NightAttackKind = NightAttackKind.CutinZuiun,
		NightZuiunCutinKind = NightZuiunCutinKind.ZuiunRadar,
		PowerModifier = 1.28,
		AccuracyModifier = 1,
		RateModifier = 160,
		NumberOfAttacks = 2,
	};

	public static NightZuiunCutinAttack NightZuiunCutinZuiunZuiun { get; } = new()
	{
		NightAttackKind = NightAttackKind.CutinZuiun,
		NightZuiunCutinKind = NightZuiunCutinKind.ZuiunZuiun,
		PowerModifier = 1.32,
		AccuracyModifier = 1,
		RateModifier = 160,
		NumberOfAttacks = 2,
	};

	public static NightZuiunCutinAttack NightZuiunCutinZuiunZuiunRadar { get; } = new()
	{
		NightAttackKind = NightAttackKind.CutinZuiun,
		NightZuiunCutinKind = NightZuiunCutinKind.ZuiunZuiunRadar,
		PowerModifier = 1.36,
		AccuracyModifier = 1,
		RateModifier = 160,
		NumberOfAttacks = 2,
	};

	private NightZuiunCutinAttack()
	{
		
	}

	public override string Display => NightZuiunCutinKindDisplay(NightZuiunCutinKind);

	private string NightZuiunCutinKindDisplay(NightZuiunCutinKind kind) => kind switch
	{
		NightZuiunCutinKind.Zuiun => AttackResources.CutinZuiun,
		NightZuiunCutinKind.ZuiunRadar => AttackResources.NightZuiunCutinZuiunRadar,
		NightZuiunCutinKind.ZuiunZuiun => AttackResources.NightZuiunCutinZuiunZuiun,
		NightZuiunCutinKind.ZuiunZuiunRadar => AttackResources.NightZuiunCutinZuiunZuiunRadar,

		_ => $"{AttackResources.Unknown}({(int)kind})",
	};
}
