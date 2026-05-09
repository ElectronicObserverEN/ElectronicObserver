using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels;

namespace ElectronicObserver.Data.Quest;

public sealed class QuestCompletionEvaluator
{
	private static readonly QuestCompletionEvaluator instance = new();

	public static QuestCompletionEvaluator Instance => instance;

	// Completion baseline keyed by quest ID from the last refresh or evaluation.
	private Dictionary<int, bool> CompletionStates { get; } = new();

	// Raised when an active quest transitions from incomplete to complete.
	public event Action<QuestData> QuestCompleted = delegate { };

	private QuestCompletionEvaluator()
	{
	}

	/// <summary>
	/// Refreshes the baseline from the current completion states without emitting completion notifications.
	/// </summary>
	public void RefreshBaseline()
	{
		KCDatabase db = KCDatabase.Instance;

		// Snapshot completion states for all currently active quests.
		foreach (QuestData quest in db.Quest.Quests.Values)
		{
			CompletionStates[quest.QuestID] = IsComplete(quest);
		}

		// Remove completion states for quests that are no longer active.
		foreach (int questId in CompletionStates.Keys.Except(db.Quest.Quests.Keys).ToList())
		{
			CompletionStates.Remove(questId);
		}
	}

	/// <summary>
	/// Compares the current completion states with the baseline and emits notifications for newly completed quests.
	/// </summary>
	public void Evaluate()
	{
		KCDatabase db = KCDatabase.Instance;

		// Check active quests for a transition from incomplete to complete.
		foreach (QuestData quest in db.Quest.Quests.Values)
		{
			bool isComplete = IsComplete(quest);

			if (CompletionStates.TryGetValue(quest.QuestID, out bool wasComplete) && !wasComplete && isComplete)
			{
				QuestCompleted(quest);
			}

			CompletionStates[quest.QuestID] = isComplete;
		}

		// Remove completion states for quests that are no longer active.
		foreach (int questId in CompletionStates.Keys.Except(db.Quest.Quests.Keys).ToList())
		{
			CompletionStates.Remove(questId);
		}
	}

	private static bool IsComplete(QuestData quest) => GetEffectiveProgress(quest) >= 1.0;

	private static double GetEffectiveProgress(QuestData quest)
	{
		// Keep this priority aligned with the quest list progress display in Window/Wpf/Quest/QuestViewModel.cs.
		// It uses the most precise tracker first, then falls back to legacy QuestProgress and API progress flags.
		if (quest.State == 3) return 1.0;

		KCDatabase db = KCDatabase.Instance;
		TrackerViewModel? tracker = db.QuestTrackerManagers.GetTrackerById(quest.QuestID);
		TrackerViewModel? systemTracker = db.SystemQuestTrackerManager.GetTrackerById(quest.QuestID);

		if (tracker is { Tasks.Count: > 0 }) return tracker.Progress;
		if (systemTracker is { Tasks.Count: > 0 }) return systemTracker.Progress;
		if (db.QuestProgress.Progresses.ContainsKey(quest.QuestID)) return db.QuestProgress[quest.QuestID].ProgressPercentage;

		return quest.Progress switch
		{
			1 => 0.5,
			2 => 0.8,
			_ => 0.0,
		};
	}
}
