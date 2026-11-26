using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks;
using ElectronicObserver.Avalonia.UiBlocker.Resources;

namespace ElectronicObserver.Avalonia.UiBlocker;

public static class SystemUiBlockers
{
	private const string BlockerColor = "#80FF0000";

	public static UiBlockerModel TaihaBlocker { get; } = new()
	{
		Id = Guid.Parse("6d794f48-387d-4862-9354-cb1cc0f32dc2"),
		IsSystem = true,
		UiElements =
		[
			new()
			{
				Preset = PositionPreset.Advance,
				BackgroundColor = BlockerColor,
				ShowTriggers = [UiBlockerTrigger.BattleEnded],
				HideTriggers = [UiBlockerTrigger.Port],
				Conditions =
				[
					new() { Type = ConditionType.CanShipSink },
				],
			},
		],
	};

	private static UiBlockerModel DoubleLineTouchBlocker { get; } = new()
	{
		Id = Guid.Parse("95983a39-b3c2-4b87-a035-860d53283562"),
		IsSystem = true,
		UiElements =
		[
			new()
			{
				Preset = PositionPreset.DoubleLine,
				BackgroundColor = BlockerColor,
				ShowTriggers = [UiBlockerTrigger.SortieAdvance],
				HideTriggers = [UiBlockerTrigger.Port, UiBlockerTrigger.BattleStarted],
				Conditions =
				[
					new() { Type = ConditionType.DoubleLineTouch },
					new() { Type = ConditionType.IsBattleNode },
					new() { Invert = true, Type = ConditionType.IsBoss },
					new() { Invert = true, Type = ConditionType.IsCombinedFleet },
				],
			},
			new()
			{
				Preset = PositionPreset.LineAhead,
				BackgroundColor = BlockerColor,
				ShowTriggers = [UiBlockerTrigger.SortieAdvance],
				HideTriggers = [UiBlockerTrigger.Port, UiBlockerTrigger.BattleStarted],
				Conditions =
				[
					new() { Type = ConditionType.DoubleLineTouch },
					new() { Type = ConditionType.IsBoss },
					new() { Invert = true, Type = ConditionType.IsCombinedFleet },
				],
			},
			new()
			{
				Preset = PositionPreset.Diamond,
				BackgroundColor = BlockerColor,
				ShowTriggers = [UiBlockerTrigger.SortieAdvance],
				HideTriggers = [UiBlockerTrigger.Port, UiBlockerTrigger.BattleStarted],
				Conditions =
				[
					new() { Type = ConditionType.DoubleLineTouch },
					new() { Type = ConditionType.IsBoss },
					new() { Invert = true, Type = ConditionType.IsCombinedFleet },
				],
			},
			new()
			{
				Preset = PositionPreset.Echelon,
				BackgroundColor = BlockerColor,
				ShowTriggers = [UiBlockerTrigger.SortieAdvance],
				HideTriggers = [UiBlockerTrigger.Port, UiBlockerTrigger.BattleStarted],
				Conditions =
				[
					new() { Type = ConditionType.DoubleLineTouch },
					new() { Type = ConditionType.IsBoss },
					new() { Invert = true, Type = ConditionType.IsEvent },
					new() { Invert = true, Type = ConditionType.IsCombinedFleet },
				],
			},
			new()
			{
				Preset = PositionPreset.EchelonEvent,
				BackgroundColor = BlockerColor,
				ShowTriggers = [UiBlockerTrigger.SortieAdvance],
				HideTriggers = [UiBlockerTrigger.Port, UiBlockerTrigger.BattleStarted],
				Conditions =
				[
					new() { Type = ConditionType.DoubleLineTouch },
					new() { Type = ConditionType.IsBoss },
					new() { Type = ConditionType.IsEvent },
					new() { Invert = true, Type = ConditionType.IsCombinedFleet },
				],
			},
			new()
			{
				Preset = PositionPreset.LineAbreast,
				BackgroundColor = BlockerColor,
				ShowTriggers = [UiBlockerTrigger.SortieAdvance],
				HideTriggers = [UiBlockerTrigger.Port, UiBlockerTrigger.BattleStarted],
				Conditions =
				[
					new() { Type = ConditionType.DoubleLineTouch },
					new() { Type = ConditionType.IsBoss },
					new() { Invert = true, Type = ConditionType.IsEvent },
					new() { Invert = true, Type = ConditionType.IsCombinedFleet },
				],
			},
			new()
			{
				Preset = PositionPreset.LineAbreastEvent,
				BackgroundColor = BlockerColor,
				ShowTriggers = [UiBlockerTrigger.SortieAdvance],
				HideTriggers = [UiBlockerTrigger.Port, UiBlockerTrigger.BattleStarted],
				Conditions =
				[
					new() { Type = ConditionType.DoubleLineTouch },
					new() { Type = ConditionType.IsBoss },
					new() { Type = ConditionType.IsEvent },
					new() { Invert = true, Type = ConditionType.IsCombinedFleet },
				],
			},
			new()
			{
				Preset = PositionPreset.Vanguard,
				BackgroundColor = BlockerColor,
				ShowTriggers = [UiBlockerTrigger.SortieAdvance],
				HideTriggers = [UiBlockerTrigger.Port, UiBlockerTrigger.BattleStarted],
				Conditions =
				[
					new() { Type = ConditionType.DoubleLineTouch },
					new() { Type = ConditionType.IsBoss },
					new() { Type = ConditionType.IsEvent },
					new() { Invert = true, Type = ConditionType.IsCombinedFleet },
				],
			},
		],
	};

	private static UiBlockerModel EchelonTouchBlocker { get; } = new()
	{
		Id = Guid.Parse("729d87c3-5bb7-47b2-a273-e70daaae1680"),
		IsSystem = true,
		UiElements =
		[
			new()
			{
				Preset = PositionPreset.Echelon,
				BackgroundColor = BlockerColor,
				ShowTriggers = [UiBlockerTrigger.SortieAdvance],
				HideTriggers = [UiBlockerTrigger.Port, UiBlockerTrigger.BattleStarted],
				Conditions =
				[
					new() { Type = ConditionType.EchelonTouch },
					new() { Type = ConditionType.IsBattleNode },
					new() { Invert = true, Type = ConditionType.IsBoss },
					new() { Invert = true, Type = ConditionType.IsEvent },
					new() { Invert = true, Type = ConditionType.IsCombinedFleet },
				],
			},
			new()
			{
				Preset = PositionPreset.EchelonEvent,
				BackgroundColor = BlockerColor,
				ShowTriggers = [UiBlockerTrigger.SortieAdvance],
				HideTriggers = [UiBlockerTrigger.Port, UiBlockerTrigger.BattleStarted],
				Conditions =
				[
					new() { Type = ConditionType.EchelonTouch },
					new() { Type = ConditionType.IsBattleNode },
					new() { Invert = true, Type = ConditionType.IsBoss },
					new() { Type = ConditionType.IsEvent },
					new() { Invert = true, Type = ConditionType.IsCombinedFleet },
				],
			},
			new()
			{
				Preset = PositionPreset.LineAhead,
				BackgroundColor = BlockerColor,
				ShowTriggers = [UiBlockerTrigger.SortieAdvance],
				HideTriggers = [UiBlockerTrigger.Port, UiBlockerTrigger.BattleStarted],
				Conditions =
				[
					new() { Type = ConditionType.EchelonTouch },
					new() { Type = ConditionType.IsBoss },
					new() { Invert = true, Type = ConditionType.IsCombinedFleet },
				],
			},
			new()
			{
				Preset = PositionPreset.DoubleLine,
				BackgroundColor = BlockerColor,
				ShowTriggers = [UiBlockerTrigger.SortieAdvance],
				HideTriggers = [UiBlockerTrigger.Port, UiBlockerTrigger.BattleStarted],
				Conditions =
				[
					new() { Type = ConditionType.EchelonTouch },
					new() { Type = ConditionType.IsBoss },
					new() { Invert = true, Type = ConditionType.IsCombinedFleet },
				],
			},
			new()
			{
				Preset = PositionPreset.Diamond,
				BackgroundColor = BlockerColor,
				ShowTriggers = [UiBlockerTrigger.SortieAdvance],
				HideTriggers = [UiBlockerTrigger.Port, UiBlockerTrigger.BattleStarted],
				Conditions =
				[
					new() { Type = ConditionType.EchelonTouch },
					new() { Type = ConditionType.IsBoss },
					new() { Invert = true, Type = ConditionType.IsCombinedFleet },
				],
			},
			new()
			{
				Preset = PositionPreset.LineAbreast,
				BackgroundColor = BlockerColor,
				ShowTriggers = [UiBlockerTrigger.SortieAdvance],
				HideTriggers = [UiBlockerTrigger.Port, UiBlockerTrigger.BattleStarted],
				Conditions =
				[
					new() { Type = ConditionType.EchelonTouch },
					new() { Type = ConditionType.IsBoss },
					new() { Invert = true, Type = ConditionType.IsEvent },
					new() { Invert = true, Type = ConditionType.IsCombinedFleet },
				],
			},
			new()
			{
				Preset = PositionPreset.LineAbreastEvent,
				BackgroundColor = BlockerColor,
				ShowTriggers = [UiBlockerTrigger.SortieAdvance],
				HideTriggers = [UiBlockerTrigger.Port, UiBlockerTrigger.BattleStarted],
				Conditions =
				[
					new() { Type = ConditionType.EchelonTouch },
					new() { Type = ConditionType.IsBoss },
					new() { Type = ConditionType.IsEvent },
					new() { Invert = true, Type = ConditionType.IsCombinedFleet },
				],
			},
			new()
			{
				Preset = PositionPreset.Vanguard,
				BackgroundColor = BlockerColor,
				ShowTriggers = [UiBlockerTrigger.SortieAdvance],
				HideTriggers = [UiBlockerTrigger.Port, UiBlockerTrigger.BattleStarted],
				Conditions =
				[
					new() { Type = ConditionType.EchelonTouch },
					new() { Type = ConditionType.IsBoss },
					new() { Type = ConditionType.IsEvent },
					new() { Invert = true, Type = ConditionType.IsCombinedFleet },
				],
			},
		],
	};

	private static UiBlockerModel SecondPatrolFormationTouchBlocker { get; } = new()
	{
		Id = Guid.Parse("6d514406-3a6c-480f-80b4-191a3de3b865"),
		IsSystem = true,
		UiElements =
		[
			new()
			{
				Preset = PositionPreset.SecondPatrolFormation,
				BackgroundColor = BlockerColor,
				ShowTriggers = [UiBlockerTrigger.SortieAdvance],
				HideTriggers = [UiBlockerTrigger.Port, UiBlockerTrigger.BattleStarted],
				Conditions =
				[
					new() { Type = ConditionType.SecondPatrolFormationTouch },
					new() { Type = ConditionType.IsBattleNode },
					new() { Invert = true, Type = ConditionType.IsBoss },
					new() { Type = ConditionType.IsCombinedFleet },
				],
			},
			new()
			{
				Preset = PositionPreset.FirstPatrolFormation,
				BackgroundColor = BlockerColor,
				ShowTriggers = [UiBlockerTrigger.SortieAdvance],
				HideTriggers = [UiBlockerTrigger.Port, UiBlockerTrigger.BattleStarted],
				Conditions =
				[
					new() { Type = ConditionType.SecondPatrolFormationTouch },
					new() { Type = ConditionType.IsBoss },
					new() { Type = ConditionType.IsCombinedFleet },
				],
			},
			new()
			{
				Preset = PositionPreset.ThirdPatrolFormation,
				BackgroundColor = BlockerColor,
				ShowTriggers = [UiBlockerTrigger.SortieAdvance],
				HideTriggers = [UiBlockerTrigger.Port, UiBlockerTrigger.BattleStarted],
				Conditions =
				[
					new() { Type = ConditionType.SecondPatrolFormationTouch },
					new() { Type = ConditionType.IsBoss },
					new() { Type = ConditionType.IsCombinedFleet },
				],
			},
			new()
			{
				Preset = PositionPreset.FourthPatrolFormation,
				BackgroundColor = BlockerColor,
				ShowTriggers = [UiBlockerTrigger.SortieAdvance],
				HideTriggers = [UiBlockerTrigger.Port, UiBlockerTrigger.BattleStarted],
				Conditions =
				[
					new() { Type = ConditionType.SecondPatrolFormationTouch },
					new() { Type = ConditionType.IsBoss },
					new() { Type = ConditionType.IsCombinedFleet },
				],
			},
		],
	};

	private static UiBlockerModel FourthPatrolFormationTouchBlocker { get; } = new()
	{
		Id = Guid.Parse("7a402d51-df67-48fd-92a5-f69e906fee16"),
		IsSystem = true,
		UiElements =
		[
			new()
			{
				Preset = PositionPreset.FourthPatrolFormation,
				BackgroundColor = BlockerColor,
				ShowTriggers = [UiBlockerTrigger.SortieAdvance],
				HideTriggers = [UiBlockerTrigger.Port, UiBlockerTrigger.BattleStarted],
				Conditions =
				[
					new() { Type = ConditionType.FourthPatrolFormationTouch },
					new() { Type = ConditionType.IsBattleNode },
					new() { Invert = true, Type = ConditionType.IsBoss },
					new() { Type = ConditionType.IsCombinedFleet },
				],
			},
			new()
			{
				Preset = PositionPreset.FirstPatrolFormation,
				BackgroundColor = BlockerColor,
				ShowTriggers = [UiBlockerTrigger.SortieAdvance],
				HideTriggers = [UiBlockerTrigger.Port, UiBlockerTrigger.BattleStarted],
				Conditions =
				[
					new() { Type = ConditionType.FourthPatrolFormationTouch },
					new() { Type = ConditionType.IsBoss },
					new() { Type = ConditionType.IsCombinedFleet },
				],
			},
			new()
			{
				Preset = PositionPreset.SecondPatrolFormation,
				BackgroundColor = BlockerColor,
				ShowTriggers = [UiBlockerTrigger.SortieAdvance],
				HideTriggers = [UiBlockerTrigger.Port, UiBlockerTrigger.BattleStarted],
				Conditions =
				[
					new() { Type = ConditionType.FourthPatrolFormationTouch },
					new() { Type = ConditionType.IsBoss },
					new() { Type = ConditionType.IsCombinedFleet },
				],
			},
			new()
			{
				Preset = PositionPreset.ThirdPatrolFormation,
				BackgroundColor = BlockerColor,
				ShowTriggers = [UiBlockerTrigger.SortieAdvance],
				HideTriggers = [UiBlockerTrigger.Port, UiBlockerTrigger.BattleStarted],
				Conditions =
				[
					new() { Type = ConditionType.FourthPatrolFormationTouch },
					new() { Type = ConditionType.IsBoss },
					new() { Type = ConditionType.IsCombinedFleet },
				],
			},
		],
	};

	private static UiBlockerModel BossSBlocker { get; } = new()
	{
		Id = Guid.Parse("bcb80373-de26-4cc6-871a-7c7fa30e7a82"),
		IsSystem = true,
		UiElements =
		[
			new()
			{
				Preset = PositionPreset.Retreat,
				BackgroundColor = BlockerColor,
				ShowTriggers = [UiBlockerTrigger.BattleStarted],
				HideTriggers = [UiBlockerTrigger.Port, UiBlockerTrigger.BattleEnded],
				Conditions =
				[
					new() { Invert = true, Type = ConditionType.CanShipSink },
					new() { Invert = true, Type = ConditionType.IsBoss },
					new() { Type = ConditionType.BattleRankAtMost, Rank = BattleRank.A },
				],
			},
			new()
			{
				Preset = PositionPreset.Retreat,
				BackgroundColor = BlockerColor,
				ShowTriggers = [UiBlockerTrigger.BattleEnded],
				HideTriggers = [UiBlockerTrigger.Port, UiBlockerTrigger.SortieAdvance],
				Conditions =
				[
					new() { Invert = true, Type = ConditionType.CanShipSink },
					new() { Invert = true, Type = ConditionType.IsBoss },
				],
			},
			new()
			{
				Preset = PositionPreset.Advance,
				BackgroundColor = BlockerColor,
				ShowTriggers = [UiBlockerTrigger.BattleStarted],
				HideTriggers = [UiBlockerTrigger.Port, UiBlockerTrigger.BattleEnded],
				Conditions =
				[
					new() { Type = ConditionType.IsBoss },
					new() { Type = ConditionType.BattleRankAtMost, Rank = BattleRank.A },
				],
			},
		],
	};

	private static UiBlockerModel Leveling5_2Blocker { get; } = new()
	{
		Id = Guid.Parse("76caf5b9-07e3-4cc4-a387-e91035ac276f"),
		IsSystem = true,
		UiElements =
		[
			new()
			{
				Preset = PositionPreset.Advance,
				BackgroundColor = BlockerColor,
				ShowTriggers = [UiBlockerTrigger.BattleEnded],
				HideTriggers = [UiBlockerTrigger.Port],
				Conditions =
				[
					new() { Type = ConditionType.Location, Map = new() { AreaId = 5, InfoId = 2 } },
				],
			},
		],
	};

	private static UiBlockerModel Leveling5_3_PBlocker { get; } = new()
	{
		Id = Guid.Parse("1acb43d9-6435-491a-b1bf-65a608527744"),
		IsSystem = true,
		UiElements =
		[
			new()
			{
				Preset = PositionPreset.Retreat,
				BackgroundColor = BlockerColor,
				ShowTriggers = [UiBlockerTrigger.BattleEnded],
				HideTriggers = [UiBlockerTrigger.Port, UiBlockerTrigger.SortieAdvance],
				Conditions =
				[
					new() { Invert = true, Type = ConditionType.CanShipSink },
					new()
					{
						Type = ConditionType.Location,
						Map = new() { AreaId = 5, InfoId = 3, },
						CellIds = [9], // 5-3-I
					},
				],
			},
			new()
			{
				Preset = PositionPreset.Custom,
				DesiredTop = 407,
				DesiredLeft = 749,
				DesiredHeight = 50,
				DesiredWidth = 50,
				BackgroundColor = BlockerColor,
				ShowTriggers = [UiBlockerTrigger.SortieAdvance],
				HideTriggers = [UiBlockerTrigger.Port, UiBlockerTrigger.BattleStarted],
				Conditions =
				[
					new()
					{
						Type = ConditionType.Location,
						Map = new() { AreaId = 5, InfoId = 3, },
						CellIds = [15, 22, 23], // 5-3-O
					},
				],
			},
			new()
			{
				Preset = PositionPreset.Advance,
				BackgroundColor = BlockerColor,
				ShowTriggers = [UiBlockerTrigger.BattleEnded],
				HideTriggers = [UiBlockerTrigger.Port],
				Conditions =
				[
					new()
					{
						Type = ConditionType.Location,
						Map = new() { AreaId = 5, InfoId = 3, },
						CellIds = [16], // 5-3-P
					},
				],
			},
		],
	};

	public static List<UiBlockerModel> All { get; } =
	[
		TaihaBlocker,
		DoubleLineTouchBlocker,
		EchelonTouchBlocker,
		SecondPatrolFormationTouchBlocker,
		FourthPatrolFormationTouchBlocker,
		BossSBlocker,
		Leveling5_2Blocker,
		Leveling5_3_PBlocker,
	];

	public static HashSet<Guid> Ids { get; } = [.. All.Select(b => b.Id)];

	public static string? GetName(Guid id) => id switch
	{
		_ when id == TaihaBlocker.Id => DefaultUiBlockerResources.Taiha,
		_ when id == DoubleLineTouchBlocker.Id => DefaultUiBlockerResources.DoubleLineTouch,
		_ when id == EchelonTouchBlocker.Id => DefaultUiBlockerResources.EchelonTouch,
		_ when id == SecondPatrolFormationTouchBlocker.Id => DefaultUiBlockerResources.SecondPatrolFormationTouch,
		_ when id == FourthPatrolFormationTouchBlocker.Id => DefaultUiBlockerResources.FourthPatrolFormationTouch,
		_ when id == BossSBlocker.Id => DefaultUiBlockerResources.BossS,
		_ when id == Leveling5_2Blocker.Id => DefaultUiBlockerResources.Leveling5_2,
		_ when id == Leveling5_3_PBlocker.Id => DefaultUiBlockerResources.Leveling5_3_P,

		_ => null,
	};

	public static string? GetDescription(Guid id) => id switch
	{
		_ when id == TaihaBlocker.Id => DefaultUiBlockerResources.Taiha_Description,
		_ when id == DoubleLineTouchBlocker.Id => DefaultUiBlockerResources.DoubleLineTouch_Description,
		_ when id == EchelonTouchBlocker.Id => DefaultUiBlockerResources.EchelonTouch_Description,
		_ when id == SecondPatrolFormationTouchBlocker.Id => DefaultUiBlockerResources.SecondPatrolFormationTouch_Description,
		_ when id == FourthPatrolFormationTouchBlocker.Id => DefaultUiBlockerResources.FourthPatrolFormationTouch_Description,
		_ when id == BossSBlocker.Id => DefaultUiBlockerResources.BossS_Description,
		_ when id == Leveling5_2Blocker.Id => DefaultUiBlockerResources.Leveling5_2_Description,
		_ when id == Leveling5_3_PBlocker.Id => DefaultUiBlockerResources.Leveling5_3_P_Description,

		_ => null,
	};
}
