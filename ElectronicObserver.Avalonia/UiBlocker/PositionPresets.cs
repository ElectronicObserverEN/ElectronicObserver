using Avalonia;

namespace ElectronicObserver.Avalonia.UiBlocker;

public static class PositionPresets
{
	private const int TopRowTop = 254;
	private const int BottomRowTop = 491;
	private const int FormationWidth = 156;
	private const int FormationHeight = 50;
	private const int CombinedFormationWidth = 215;

	private static Rect Advance { get; } = new(330, 270, 210, 190);
	private static Rect Retreat { get; } = new(660, 270, 210, 190);

	private static Rect LineAhead { get; } = new(594, TopRowTop, FormationWidth, FormationHeight);
	private static Rect DoubleLine { get; } = new(787, TopRowTop, FormationWidth, FormationHeight);
	private static Rect Diamond { get; } = new(985, TopRowTop, FormationWidth, FormationHeight);
	private static Rect Echelon { get; } = new(695, BottomRowTop, FormationWidth, FormationHeight);
	private static Rect EchelonEvent { get; } = new(594, BottomRowTop, FormationWidth, FormationHeight);
	private static Rect LineAbreast { get; } = new(891, BottomRowTop, FormationWidth, FormationHeight);
	private static Rect LineAbreastEvent { get; } = new(787, BottomRowTop, FormationWidth, FormationHeight);
	private static Rect Vanguard { get; } = new(985, BottomRowTop, FormationWidth, FormationHeight);

	private static Rect FirstPatrolFormation { get; } = new(640, 237, CombinedFormationWidth, FormationHeight);
	private static Rect SecondPatrolFormation { get; } = new(888, 237, CombinedFormationWidth, FormationHeight);
	private static Rect ThirdPatrolFormation { get; } = new(640, 442, CombinedFormationWidth, FormationHeight);
	private static Rect FourthPatrolFormation { get; } = new(888, 442, CombinedFormationWidth, FormationHeight);

	private static Rect Fullscreen { get; } = new(0, 0, 1200, 720);

	private static Rect None { get; } = new(0, 0, 0, 0);

	public static Rect TryGet(PositionPreset preset) => preset switch
	{
		PositionPreset.Advance => Advance,
		PositionPreset.Retreat => Retreat,

		PositionPreset.LineAhead => LineAhead,
		PositionPreset.DoubleLine => DoubleLine,
		PositionPreset.Diamond => Diamond,
		PositionPreset.Echelon => Echelon,
		PositionPreset.EchelonEvent => EchelonEvent,
		PositionPreset.LineAbreast => LineAbreast,
		PositionPreset.LineAbreastEvent => LineAbreastEvent,
		PositionPreset.Vanguard => Vanguard,

		PositionPreset.FirstPatrolFormation => FirstPatrolFormation,
		PositionPreset.SecondPatrolFormation => SecondPatrolFormation,
		PositionPreset.ThirdPatrolFormation => ThirdPatrolFormation,
		PositionPreset.FourthPatrolFormation => FourthPatrolFormation,

		PositionPreset.Fullscreen => Fullscreen,

		_ => None,
	};
}
