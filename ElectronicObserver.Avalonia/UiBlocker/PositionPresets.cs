using Avalonia;

namespace ElectronicObserver.Avalonia.UiBlocker;

public static class PositionPresets
{
	private static Rect Advance { get; } = new(330, 270, 210, 190);
	private static Rect Retreat { get; } = new(660, 270, 210, 190);

	private static Rect LineAhead { get; } = new(593, 255, 155, 50);
	private static Rect DoubleLine { get; } = new(789, 255, 155, 50);
	private static Rect Diamond { get; } = new(986, 255, 155, 50);
	private static Rect Echelon { get; } = new(695, 492, 155, 50);
	private static Rect EchelonEvent { get; } = new(593, 492, 155, 50);
	private static Rect LineAbreast { get; } = new(891, 492, 155, 50);
	private static Rect LineAbreastEvent { get; } = new(789, 492, 155, 50);
	private static Rect Vanguard { get; } = new(986, 492, 155, 50);

	private static Rect FirstPatrolFormation { get; } = new(640, 237, 215, 50);
	private static Rect SecondPatrolFormation { get; } = new(888, 237, 215, 50);
	private static Rect ThirdPatrolFormation { get; } = new(640, 442, 215, 50);
	private static Rect FourthPatrolFormation { get; } = new(888, 442, 215, 50);

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
