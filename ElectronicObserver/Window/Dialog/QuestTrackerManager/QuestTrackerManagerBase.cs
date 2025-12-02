using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ElectronicObserver.Common;
using ElectronicObserver.Core;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Tasks;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels;
using MessagePack;
using MessagePack.Resolvers;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager;

public abstract class QuestTrackerManagerBase : WindowViewModelBase
{
	public ObservableCollection<TrackerViewModel> Trackers { get; } = new();

	protected DateTime LastQuestListUpdate { get; set; } = new(2000, 1, 1);

	// MessagePack has a bug when converting DateTime to json
	// adding these options avoids it by using a different DateTime representation
	protected MessagePackSerializerOptions DateTimeOptions => MessagePackSerializerOptions.Standard
		.WithResolver(CompositeResolver.Create(NativeDateTimeResolver.Instance, TypelessObjectResolver.Instance));

	protected void SubscribeToApis()
	{
		var ao = APIObserver.Instance;

		ao.ApiPort_Port.ResponseReceived += TimerSave;

		ao.ApiGetMember_QuestList.ResponseReceived += QuestUpdated;

		ao.ApiReqMap_Start.ResponseReceived += StartSortie;

		ao.ApiReqMap_Next.ResponseReceived += NextSortie;

		ao.ApiReqSortie_BattleResult.ResponseReceived += BattleFinished;
		ao.ApiReqCombinedBattle_BattleResult.ResponseReceived += BattleFinished;

		ao.ApiReqSortie_BattleResult.ResponseReceived += BossBattleFinished;
		ao.ApiReqCombinedBattle_BattleResult.ResponseReceived += BossBattleFinished;

		ao.ApiReqSortie_BattleResult.ResponseReceived += MapClearedFirstTime;
		ao.ApiReqCombinedBattle_BattleResult.ResponseReceived += MapClearedFirstTime;

		ao.ApiReqPractice_BattleResult.ResponseReceived += PracticeFinished;

		ao.ApiReqMission_Result.ResponseReceived += ExpeditionCompleted;
	}

	private void TimerSave(string apiname, dynamic data)
	{
		if (!DateTimeHelper.IsCrossedHour(LastQuestListUpdate)) return;

		LastQuestListUpdate = DateTime.Now;

		Save();
		Utility.Logger.Add(1, QuestTracking.AutoSavedProgress);
	}

	private void QuestUpdated(string apiname, dynamic data)
	{
		QuestManager quests = KCDatabase.Instance.Quest;

		bool ShouldQuestReset(QuestModel quest) => quest.ResetType switch
		{
			QuestResetType.Daily => DateTimeHelper.IsCrossedDailyQuestReset(LastQuestListUpdate),
			QuestResetType.Weekly => DateTimeHelper.IsCrossedWeeklyQuestReset(LastQuestListUpdate),
			QuestResetType.Monthly => DateTimeHelper.IsCrossedMonthlyQuestReset(LastQuestListUpdate),
			QuestResetType.Quarterly => DateTimeHelper.IsCrossedQuarterlyQuestReset(LastQuestListUpdate),

			QuestResetType.January => DateTimeHelper.IsCrossedYearlyQuestReset(LastQuestListUpdate, 1),
			QuestResetType.February => DateTimeHelper.IsCrossedYearlyQuestReset(LastQuestListUpdate, 2),
			QuestResetType.March => DateTimeHelper.IsCrossedYearlyQuestReset(LastQuestListUpdate, 3),
			QuestResetType.April => DateTimeHelper.IsCrossedYearlyQuestReset(LastQuestListUpdate, 4),
			QuestResetType.May => DateTimeHelper.IsCrossedYearlyQuestReset(LastQuestListUpdate, 5),
			QuestResetType.June => DateTimeHelper.IsCrossedYearlyQuestReset(LastQuestListUpdate, 6),
			QuestResetType.July => DateTimeHelper.IsCrossedYearlyQuestReset(LastQuestListUpdate, 7),
			QuestResetType.August => DateTimeHelper.IsCrossedYearlyQuestReset(LastQuestListUpdate, 8),
			QuestResetType.September => DateTimeHelper.IsCrossedYearlyQuestReset(LastQuestListUpdate, 9),
			QuestResetType.October => DateTimeHelper.IsCrossedYearlyQuestReset(LastQuestListUpdate, 10),
			QuestResetType.November => DateTimeHelper.IsCrossedYearlyQuestReset(LastQuestListUpdate, 11),
			QuestResetType.December => DateTimeHelper.IsCrossedYearlyQuestReset(LastQuestListUpdate, 12),

			_ => false
		};

		//消えている・達成済みの任務の進捗情報を削除
		if (!quests.IsLoadCompleted) return;

		IEnumerable<TrackerViewModel> trackersToReset = Trackers.Where(t => !quests.Quests.ContainsKey(t.QuestId) || ShouldQuestReset(t.Model.Quest));

		foreach (TrackerViewModel tracker in trackersToReset)
		{
			foreach (IQuestTask? task in tracker.Model.Tasks)
			{
				if (task is null) continue;

				task.Progress = 0;
			}
		}

		LastQuestListUpdate = DateTime.Now;
	}

	private void StartSortie(string apiname, dynamic data)
	{
		var compass = KCDatabase.Instance.Battle.Compass;

		var fleet = KCDatabase.Instance.Fleet.Fleets.Values.FirstOrDefault(f => f.IsInSortie);

		if (fleet is null) return;

		foreach (TrackerViewModel tracker in Trackers.Where(t => t.State == 2))
		{
			tracker.Increment(fleet, compass.MapAreaID, compass.MapInfoID, compass.CellId);
		}
	}

	private void NextSortie(string apiname, dynamic data)
	{
		var compass = KCDatabase.Instance.Battle.Compass;

		var fleet = KCDatabase.Instance.Fleet.Fleets.Values.FirstOrDefault(f => f.IsInSortie);

		if (fleet is null) return;

		foreach (TrackerViewModel tracker in Trackers.Where(t => t.State == 2))
		{
			tracker.Increment(fleet, compass.MapAreaID, compass.MapInfoID, compass.CellId);
		}
	}

	private void BattleFinished(string apiname, dynamic data)
	{
		var bm = KCDatabase.Instance.Battle;
		var battle = bm.SecondBattle ?? bm.FirstBattle;
		var hps = battle.ResultHPs;
		var fleet = KCDatabase.Instance.Fleet.Fleets.Values.FirstOrDefault(f => f.IsInSortie);

		if (hps is null) return;
		if (fleet is null) return;

		foreach (TrackerViewModel tracker in Trackers.Where(t => t.State == 2))
		{
			tracker.Increment(fleet, bm.Result.Rank, bm.Compass.MapAreaID, bm.Compass.MapInfoID, bm.Compass.CellId);
		}
	}

	private void BossBattleFinished(string apiname, dynamic data)
	{
		var bm = KCDatabase.Instance.Battle;
		var battle = bm.SecondBattle ?? bm.FirstBattle;
		var hps = battle.ResultHPs;
		var fleet = KCDatabase.Instance.Fleet.Fleets.Values.FirstOrDefault(f => f.IsInSortie);

		if (hps is null) return;
		if (bm.Compass.EventID != 5) return;
		if (fleet is null) return;

		foreach (TrackerViewModel tracker in Trackers.Where(t => t.State == 2))
		{
			tracker.Increment(fleet, bm.Result.Rank, bm.Compass.MapAreaID, bm.Compass.MapInfoID);
		}

		// p.Increment(bm.Result.Rank, bm.Compass.MapAreaID * 10 + bm.Compass.MapInfoID, bm.Compass.EventID == 5);

	}

	private void MapClearedFirstTime(string apiname, dynamic data)
	{
		var bm = KCDatabase.Instance.Battle;
		var battle = bm.SecondBattle ?? bm.FirstBattle;
		var hps = battle.ResultHPs;
		var fleet = KCDatabase.Instance.Fleet.Fleets.Values.FirstOrDefault(f => f.IsInSortie);

		if (hps is null) return;
		if (bm.Compass.EventID != 5) return;
		if (fleet is null) return;
		if (!bm.Result.IsFirstClear) return;

		foreach (TrackerViewModel tracker in Trackers.Where(t => t.State == 2))
		{
			tracker.Increment(fleet, bm.Compass.MapAreaID, bm.Compass.MapInfoID);
		}
	}

	private void PracticeFinished(string apiname, dynamic data)
	{
		IFleetData? fleet = KCDatabase.Instance.Fleet.Fleets.Values
			.FirstOrDefault(f => f.IsInPractice);

		if (fleet is null) return;

		foreach (TrackerViewModel tracker in Trackers.Where(t => t.State == 2))
		{
			tracker.Increment(fleet, (string)data.api_win_rank);
		}
	}

	private void ExpeditionCompleted(string apiname, dynamic data)
	{
		// 遠征失敗
		if ((int)data.api_clear_result == 0) return;

		FleetData? fleet = KCDatabase.Instance.Fleet.Fleets.Values
			.FirstOrDefault(f => f.Members.Contains((int)data.api_ship_id[1]));

		if (fleet == null) return;

		int areaId = fleet.ExpeditionDestination;

		foreach (TrackerViewModel tracker in Trackers.Where(t => t.State == 2))
		{
			tracker.Increment(fleet, areaId);
		}
	}

	public void EquipmentDiscarded(string apiname, Dictionary<string, string> data)
	{
		IEnumerable<IEquipmentData> discardedEquipment = data["api_slotitem_ids"]
			.Split(",".ToCharArray())
			.Select(s => KCDatabase.Instance.Equipments[int.Parse(s)]);

		foreach (TrackerViewModel tracker in Trackers.Where(t => t.State == 2))
		{
			tracker.Increment(discardedEquipment.Select(e => e.EquipmentId));
		}

		foreach (TrackerViewModel tracker in Trackers.Where(t => t.State == 2))
		{
			tracker.Increment(discardedEquipment.Select(e => e.MasterEquipment.CategoryType));
		}

		foreach (TrackerViewModel tracker in Trackers.Where(t => t.State == 2))
		{
			tracker.Increment(discardedEquipment.Select(e => (EquipmentCardType)e.MasterEquipment.CardType));
		}

		foreach (TrackerViewModel tracker in Trackers.Where(t => t.State == 2))
		{
			tracker.Increment(discardedEquipment.Select(e => e.MasterEquipment.IconTypeTyped));
		}
	}

	public abstract void Save();

	public TrackerViewModel? GetTrackerById(int id) => Trackers.FirstOrDefault(t => t.QuestId == id);
}
