namespace ElectronicObserver.Avalonia.UiBlocker;

public enum PositionPreset
{
	Custom = 0,

	/// <summary>
	/// Sortie advance, don't enter night battle.
	/// </summary>
	Advance = 1,

	/// <summary>
	/// Sortie retreat, enter night battle.
	/// </summary>
	Retreat = 2,

	LineAhead = 3,
	DoubleLine = 4,
	Diamond = 5,
	Echelon = 6,
	EchelonEvent = 7,
	LineAbreast = 8,
	LineAbreastEvent = 9,
	Vanguard = 10,

	FirstPatrolFormation = 11,
	SecondPatrolFormation = 12,
	ThirdPatrolFormation = 13,
	FourthPatrolFormation = 14,

	Fullscreen = 99,
}
