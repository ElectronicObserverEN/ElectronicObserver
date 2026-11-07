using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Avalonia.QuestTrackerManager2;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks;
using ElectronicObserver.Core;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Quests;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager;

public class QuestTrackerManagerService
{
	private TrackerIncrementService TrackerIncrementService { get; } = new();
	private IEnumerable<IQuestTrackerManager> Managers()
	{
		yield return User;
		yield return System;
		yield return System2;
		yield return User2;
	}

	public QuestTrackerManagerViewModel User => field ??= new();
	public SystemQuestTrackerManager System => field ??= new();
	public QuestTrackerManager2ViewModel User2 => field ??= Ioc.Default.GetRequiredService<QuestTrackerManager2ViewModel>();
	public SystemQuestTrackerManager2 System2 => field ??= Ioc.Default.GetRequiredService<SystemQuestTrackerManager2>();

	public QuestTrackerManagerService()
	{
		SubscribeToApis();
	}

	public void SubscribeToApis()
	{
		APIObserver.Instance.ApiPort_Port.ResponseReceived += TimerSave;

		APIObserver.Instance.ApiGetMember_QuestList.ResponseReceived += QuestUpdated;

		APIObserver.Instance.ApiReqMap_Start.ResponseReceived += NodeVisit;
		APIObserver.Instance.ApiReqMap_Next.ResponseReceived += NodeVisit;

		APIObserver.Instance.ApiReqSortie_BattleResult.ResponseReceived += BattleFinished;
		APIObserver.Instance.ApiReqCombinedBattle_BattleResult.ResponseReceived += BattleFinished;

		APIObserver.Instance.ApiReqSortie_BattleResult.ResponseReceived += BossBattleFinished;
		APIObserver.Instance.ApiReqCombinedBattle_BattleResult.ResponseReceived += BossBattleFinished;

		APIObserver.Instance.ApiReqSortie_BattleResult.ResponseReceived += MapClearedFirstTime;
		APIObserver.Instance.ApiReqCombinedBattle_BattleResult.ResponseReceived += MapClearedFirstTime;

		APIObserver.Instance.ApiReqPractice_BattleResult.ResponseReceived += PracticeFinished;

		APIObserver.Instance.ApiReqMission_Result.ResponseReceived += ExpeditionCompleted;
	}

	private void TimerSave(string apiname, dynamic data)
	{
		foreach (IQuestTrackerManager manager in Managers())
		{
			if (!DateTimeHelper.IsCrossedHour(manager.LastTrackerSave)) return;

			manager.LastTrackerSave = DateTime.Now;

			manager.Save();
			Utility.Logger.Add(1, QuestTracking.AutoSavedProgress);
		}
	}

	private void QuestUpdated(string apiname, dynamic data)
	{
		QuestManager quests = KCDatabase.Instance.Quest;

		foreach (IQuestTrackerManager manager in Managers())
		{
			//消えている・達成済みの任務の進捗情報を削除
			if (!quests.IsLoadCompleted) return;

			IEnumerable<ITrackerViewModel> trackersToReset = manager.Trackers
				.Where(t => !quests.Quests.ContainsKey(t.QuestId) || ShouldQuestReset(t.QuestResetType, manager.LastQuestListUpdate));

			foreach (ITrackerViewModel tracker in trackersToReset)
			{
				tracker.ResetTaskProgress();
			}

			manager.LastQuestListUpdate = DateTime.Now;
		}

		static bool ShouldQuestReset(QuestResetType questResetType, DateTime lastUpdate) => questResetType switch
		{
			QuestResetType.Daily => DateTimeHelper.IsCrossedDailyQuestReset(lastUpdate),
			QuestResetType.Weekly => DateTimeHelper.IsCrossedWeeklyQuestReset(lastUpdate),
			QuestResetType.Monthly => DateTimeHelper.IsCrossedMonthlyQuestReset(lastUpdate),
			QuestResetType.Quarterly => DateTimeHelper.IsCrossedQuarterlyQuestReset(lastUpdate),

			QuestResetType.January => DateTimeHelper.IsCrossedYearlyQuestReset(lastUpdate, 1),
			QuestResetType.February => DateTimeHelper.IsCrossedYearlyQuestReset(lastUpdate, 2),
			QuestResetType.March => DateTimeHelper.IsCrossedYearlyQuestReset(lastUpdate, 3),
			QuestResetType.April => DateTimeHelper.IsCrossedYearlyQuestReset(lastUpdate, 4),
			QuestResetType.May => DateTimeHelper.IsCrossedYearlyQuestReset(lastUpdate, 5),
			QuestResetType.June => DateTimeHelper.IsCrossedYearlyQuestReset(lastUpdate, 6),
			QuestResetType.July => DateTimeHelper.IsCrossedYearlyQuestReset(lastUpdate, 7),
			QuestResetType.August => DateTimeHelper.IsCrossedYearlyQuestReset(lastUpdate, 8),
			QuestResetType.September => DateTimeHelper.IsCrossedYearlyQuestReset(lastUpdate, 9),
			QuestResetType.October => DateTimeHelper.IsCrossedYearlyQuestReset(lastUpdate, 10),
			QuestResetType.November => DateTimeHelper.IsCrossedYearlyQuestReset(lastUpdate, 11),
			QuestResetType.December => DateTimeHelper.IsCrossedYearlyQuestReset(lastUpdate, 12),

			_ => false
		};
	}

	private void NodeVisit(string apiname, dynamic data)
	{
		foreach (IQuestTrackerManager manager in Managers())
		{
			TrackerIncrementService.NodeVisit(manager.Trackers);
		}
	}

	private void BattleFinished(string apiname, dynamic data)
	{
		foreach (IQuestTrackerManager manager in Managers())
		{
			TrackerIncrementService.BattleFinished(manager.Trackers);
		}
	}

	private void BossBattleFinished(string apiname, dynamic data)
	{
		foreach (IQuestTrackerManager manager in Managers())
		{
			TrackerIncrementService.BossBattleFinished(manager.Trackers);
		}
	}

	private void MapClearedFirstTime(string apiname, dynamic data)
	{
		foreach (IQuestTrackerManager manager in Managers())
		{
			TrackerIncrementService.MapClearedFirstTime(manager.Trackers);
		}
	}

	private void PracticeFinished(string apiname, dynamic data)
	{
		foreach (IQuestTrackerManager manager in Managers())
		{
			TrackerIncrementService.PracticeFinished(manager.Trackers, (string)data.api_win_rank);
		}
	}

	private void ExpeditionCompleted(string apiname, dynamic data)
	{
		foreach (IQuestTrackerManager manager in Managers())
		{
			TrackerIncrementService.ExpeditionCompleted(manager.Trackers, (int)data.api_clear_result, (int)data.api_ship_id[1]);
		}
	}

	public void EquipmentDiscarded(string apiname, Dictionary<string, string> data)
	{
		IEnumerable<IEquipmentData> discardedEquipment = data["api_slotitem_ids"]
			.Split(",".ToCharArray())
			.Select(s => KCDatabase.Instance.Equipments[int.Parse(s)])
			.OfType<IEquipmentData>();

		foreach (IQuestTrackerManager manager in Managers())
		{
			TrackerIncrementService.EquipmentDiscarded(manager.Trackers, discardedEquipment);
		}
	}

	public void Load()
	{
		foreach (IQuestTrackerManager manager in Managers())
		{
			manager.Load();
		}
	}

	public void Save()
	{
		foreach (IQuestTrackerManager manager in Managers())
		{
			manager.Save();
		}
	}
}
