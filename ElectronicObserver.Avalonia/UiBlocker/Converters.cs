using Avalonia.Data.Converters;
using ElectronicObserver.Avalonia.UiBlocker.Resources;

namespace ElectronicObserver.Avalonia.UiBlocker;

public static class Converters
{
	public static FuncValueConverter<PositionPreset, string> PositionPresetToDisplayConverter { get; }
		= new(ToDisplay);

	public static string ToDisplay(this PositionPreset preset) => preset switch
	{
		PositionPreset.Custom => PositionPresetResources.Custom,
		PositionPreset.Advance => PositionPresetResources.Advance,
		PositionPreset.Retreat => PositionPresetResources.Retreat,
		PositionPreset.LineAhead => PositionPresetResources.LineAhead,
		PositionPreset.DoubleLine => PositionPresetResources.DoubleLine,
		PositionPreset.Diamond => PositionPresetResources.Diamond,
		PositionPreset.Echelon => PositionPresetResources.Echelon,
		PositionPreset.EchelonEvent => PositionPresetResources.EchelonEvent,
		PositionPreset.LineAbreast => PositionPresetResources.LineAbreast,
		PositionPreset.LineAbreastEvent => PositionPresetResources.LineAbreastEvent,
		PositionPreset.Vanguard => PositionPresetResources.Vanguard,
		PositionPreset.FirstPatrolFormation => PositionPresetResources.FirstPatrolFormation,
		PositionPreset.SecondPatrolFormation => PositionPresetResources.SecondPatrolFormation,
		PositionPreset.ThirdPatrolFormation => PositionPresetResources.ThirdPatrolFormation,
		PositionPreset.FourthPatrolFormation => PositionPresetResources.FourthPatrolFormation,
		PositionPreset.Fullscreen => PositionPresetResources.Fullscreen,

		_ => "???",
	};

	public static FuncValueConverter<UiBlockerTrigger, string> UiBlockerTriggerToDisplayConverter { get; }
		= new(ToDisplay);

	public static string ToDisplay(this UiBlockerTrigger trigger) => trigger switch
	{
		UiBlockerTrigger.Port => UiBlockerTriggerResources.Port,
		UiBlockerTrigger.SortieAdvance => UiBlockerTriggerResources.SortieAdvance,
		UiBlockerTrigger.BattleStarted => UiBlockerTriggerResources.BattleStarted,
		UiBlockerTrigger.BattleEnded => UiBlockerTriggerResources.BattleEnded,

		_ => "???",
	};

	public static FuncValueConverter<ConditionType, string> ConditionTypeToDisplayConverter { get; } = new(ToDisplay);

	public static string ToDisplay(this ConditionType condition) => condition switch
	{
		ConditionType.CanShipSink => ConditionTypeResources.CanShipSink,
		ConditionType.DoubleLineTouch => ConditionTypeResources.DoubleLineTouch,
		ConditionType.EchelonTouch => ConditionTypeResources.EchelonTouch,
		ConditionType.SecondPatrolFormationTouch => ConditionTypeResources.SecondPatrolFormationTouch,
		ConditionType.FourthPatrolFormationTouch => ConditionTypeResources.FourthPatrolFormationTouch,
		ConditionType.IsBattleNode => ConditionTypeResources.IsBattleNode,
		ConditionType.IsBoss => ConditionTypeResources.IsBoss,
		ConditionType.IsEvent => ConditionTypeResources.IsEvent,
		ConditionType.IsCombinedFleet => ConditionTypeResources.IsCombinedFleet,
		ConditionType.BattleRankAtMost => ConditionTypeResources.BattleRankAtMost,
		ConditionType.BattleRankAtLeast => ConditionTypeResources.BattleRankAtLeast,
		ConditionType.Location => ConditionTypeResources.Location,

		_ => "???",
	};
}
