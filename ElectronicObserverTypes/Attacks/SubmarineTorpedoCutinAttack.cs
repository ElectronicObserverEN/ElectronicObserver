namespace ElectronicObserverTypes.Attacks;

public sealed record SubmarineTorpedoCutinAttack : NightAttack
{
	public NightTorpedoCutinKind NightTorpedoCutinKind { get; private init; }

	public static SubmarineTorpedoCutinAttack CutinTorpedoTorpedoLateModelTorpedoSubmarineEquipment { get; } = new()
	{
		NightAttackKind = NightAttackKind.CutinTorpedoTorpedo,
		NightTorpedoCutinKind = NightTorpedoCutinKind.LateModelTorpedoSubmarineEquipment,
		PowerModifier = 1.75,
		AccuracyModifier = 1.65,
		RateModifier = 105,
		NumberOfAttacks = 2,
	};

	public static SubmarineTorpedoCutinAttack CutinTorpedoTorpedoLateModelTorpedo2 { get; } = new()
	{
		NightAttackKind = NightAttackKind.CutinTorpedoTorpedo,
		NightTorpedoCutinKind = NightTorpedoCutinKind.LateModelTorpedo2,
		PowerModifier = 1.6,
		AccuracyModifier = 1.65,
		RateModifier = 110,
		NumberOfAttacks = 2,
	};

	private SubmarineTorpedoCutinAttack()
	{
		
	}

	public override string Display => SubCutinDisplay(NightTorpedoCutinKind);

	private static string SubCutinDisplay(NightTorpedoCutinKind torpedoCutin) => torpedoCutin switch
	{
		NightTorpedoCutinKind.LateModelTorpedoSubmarineEquipment => AttackResources.LateModelTorpedoSubmarineEquipment,
		NightTorpedoCutinKind.LateModelTorpedo2 => AttackResources.LateModelTorpedo2,

		_ => $"{AttackResources.Unknown}({(int)torpedoCutin})",
	};
}
