using System.Text;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.BattleNodeId;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.BossKill;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.EquipmentCardTypeScrap;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.EquipmentCategoryScrap;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.EquipmentIconTypeScrap;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.EquipmentScrap;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.Exercise;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.Expedition;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.MapFirstClear;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.NodeReach;
using ElectronicObserver.Core.Types.Data;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks;

public sealed class TaskDisplayerService(IKCDatabase kcDatabase)
{
	private IKCDatabase KcDatabase { get; } = kcDatabase;

	public string ToDisplay(TaskViewModel? task) => task switch
	{
		BossKillTaskViewModel b => $"{b.Progress}/{b.Count} {BossKillCondition(b)}",
		ExpeditionTaskViewModel e => $"{e.Progress}/{e.Count} {ExpeditionCondition(e)}",
		BattleNodeIdTaskViewModel b => $"{b.Progress}/{b.Count} {BattleNodeIdCondition(b)}",
		NodeReachTaskViewModel n => $"{n.Progress}/{n.Count} {NodeReachCondition(n)}",
		MapFirstClearTaskViewModel m => $"{m.Progress}/{m.Count} {MapFirstClearCondition(m)}",
		ExerciseTaskViewModel e => $"{e.Progress}/{e.Count} {ExerciseCondition(e)}",
		EquipmentScrapTaskViewModel e =>
			$"{e.Progress}/{e.Count} {e.SelectedEquipment?.NameEN}{QuestTracking.Discard}{e.Count}{QuestTracking.NumberOfPieces}",
		EquipmentCategoryScrapTaskViewModel e =>
			$"{e.Progress}/{e.Count} {Converters.ToDisplay(e.Category, KcDatabase)}{QuestTracking.Discard}{e.Count}{QuestTracking.NumberOfPieces}",
		EquipmentCardTypeScrapTaskViewModel e =>
			$"{e.Progress}/{e.Count} {QuestTracking.Illust}[{Converters.ToDisplay(e.CardType)}]{QuestTracking.Discard}{e.Count}{QuestTracking.NumberOfPieces}",
		EquipmentIconTypeScrapTaskViewModel e =>
			$"{e.Progress}/{e.Count} {QuestTracking.Icon}[{Converters.ToDisplay(e.IconType)}]{QuestTracking.Discard}{e.Count}{QuestTracking.NumberOfPieces}",

		_ => string.Empty,
	};

	private static string BossKillCondition(BossKillTaskViewModel task)
	{
		StringBuilder sb = new();

		sb.Append(task.Map.Display);
		sb.Append(QuestTracking.ClearConditionBoss);
		sb.Append(RankCondition(task.Rank));
		sb.Append(task.Count);

		return sb.ToString();
	}

	private static string ExpeditionCondition(ExpeditionTaskViewModel task) =>
		$"{task.SelectedExpedition?.DisplayID}: {task.SelectedExpedition?.NameEN} x {task.Count}";

	private static string BattleNodeIdCondition(BattleNodeIdTaskViewModel task)
	{
		StringBuilder sb = new();

		sb.Append(task.Map.Display);
		sb.Append(NodesDisplay(task.Name, task.NodeIds));
		sb.Append(' ');
		sb.Append(RankCondition(task.Rank));
		sb.Append(task.Count);

		return sb.ToString();
	}

	private static string NodeReachCondition(NodeReachTaskViewModel task)
	{
		StringBuilder sb = new();

		sb.Append(task.Map.Display);
		sb.Append(NodesDisplay(task.Name, task.NodeIds));
		sb.Append(QuestTracking.Reach);
		sb.Append(task.Count);

		return sb.ToString();
	}

	private static string MapFirstClearCondition(MapFirstClearTaskViewModel task) =>
		$"{task.Map.Display}{QuestTracking.MapFirstClear}{task.Count}";

	private static string ExerciseCondition(ExerciseTaskViewModel task)
	{
		StringBuilder sb = new();

		sb.Append(QuestTracking.Exercise);
		sb.Append(task.Rank switch
		{
			BattleRank.D or BattleRank.C => $"{task.Rank}{QuestTracking.ClearConditionOrHigher}",
			>= BattleRank.B => $"{task.Rank}{QuestTracking.ClearConditionRankVictories}",
			_ => "×",
		});
		sb.Append(task.Count);

		return sb.ToString();
	}

	private static string RankCondition(BattleRank rank) => rank switch
	{
		BattleRank.Any => QuestTracking.ClearConditionClear,
		BattleRank.D or BattleRank.C => $"{rank}{QuestTracking.ClearConditionOrHigher}",
		BattleRank.B => "",
		BattleRank.A or BattleRank.S or BattleRank.SS => $"{rank}{QuestTracking.ClearConditionRankVictories}",

		_ => QuestTracking.ClearConditionBattle,
	};

	private static string NodesDisplay(string? name, IEnumerable<NodeIdViewModel> nodeIds)
		=> string.IsNullOrEmpty(name) switch
		{
			true => $"({string.Join("・", nodeIds.Select(n => n.Id))})",
			_ => name,
		};
}
