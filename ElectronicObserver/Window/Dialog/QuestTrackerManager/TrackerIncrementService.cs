using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Avalonia.QuestTrackerManager2;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Data;
using ElectronicObserver.Data.Battle;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager;

public class TrackerIncrementService
{
	public void NodeVisit(IEnumerable<ITrackerViewModel> trackers)
	{
		CompassData compass = KCDatabase.Instance.Battle.Compass;
		FleetData? fleet = KCDatabase.Instance.Fleet.Fleets.Values.FirstOrDefault(f => f.IsInSortie);

		if (fleet is null) return;

		foreach (ITrackerViewModel tracker in trackers.Where(t => t.State is 2))
		{
			tracker.Increment(fleet, compass.MapAreaID, compass.MapInfoID, compass.CellId);
		}
	}

	public void BattleFinished(IEnumerable<ITrackerViewModel> trackers)
	{
		BattleManager bm = KCDatabase.Instance.Battle;
		BattleData battle = bm.SecondBattle ?? bm.FirstBattle;
		FleetData? fleet = KCDatabase.Instance.Fleet.Fleets.Values.FirstOrDefault(f => f.IsInSortie);

		if (battle.ResultHPs is null) return;
		if (fleet is null) return;

		foreach (ITrackerViewModel tracker in trackers.Where(t => t.State is 2))
		{
			tracker.Increment(fleet, bm.Result.Rank, bm.Compass.MapAreaID, bm.Compass.MapInfoID, bm.Compass.CellId);
		}
	}

	public void BossBattleFinished(IEnumerable<ITrackerViewModel> trackers)
	{
		BattleManager bm = KCDatabase.Instance.Battle;
		BattleData battle = bm.SecondBattle ?? bm.FirstBattle;
		FleetData? fleet = KCDatabase.Instance.Fleet.Fleets.Values.FirstOrDefault(f => f.IsInSortie);

		if (battle.ResultHPs is null) return;
		if (bm.Compass.EventID != 5) return;
		if (fleet is null) return;

		foreach (ITrackerViewModel tracker in trackers.Where(t => t.State is 2))
		{
			tracker.Increment(fleet, bm.Result.Rank, bm.Compass.MapAreaID, bm.Compass.MapInfoID);
		}
	}

	public void MapClearedFirstTime(IEnumerable<ITrackerViewModel> trackers)
	{
		BattleManager bm = KCDatabase.Instance.Battle;
		BattleData battle = bm.SecondBattle ?? bm.FirstBattle;
		FleetData? fleet = KCDatabase.Instance.Fleet.Fleets.Values.FirstOrDefault(f => f.IsInSortie);

		if (battle.ResultHPs is null) return;
		if (bm.Compass.EventID != 5) return;
		if (fleet is null) return;
		if (!bm.Result.IsFirstClear) return;

		foreach (ITrackerViewModel tracker in trackers.Where(t => t.State is 2))
		{
			tracker.Increment(fleet, bm.Compass.MapAreaID, bm.Compass.MapInfoID);
		}
	}

	public void PracticeFinished(IEnumerable<ITrackerViewModel> trackers, string winRank)
	{
		IFleetData? fleet = KCDatabase.Instance.Fleet.Fleets.Values
			.FirstOrDefault(f => f.IsInPractice);

		if (fleet is null) return;

		foreach (ITrackerViewModel tracker in trackers.Where(t => t.State is 2))
		{
			tracker.Increment(fleet, winRank);
		}
	}

	public void ExpeditionCompleted(IEnumerable<ITrackerViewModel> trackers, int clearResult, int flagshipId)
	{
		// 遠征失敗
		if (clearResult == 0) return;

		FleetData? fleet = KCDatabase.Instance.Fleet.Fleets.Values
			.FirstOrDefault(f => f.Members.Contains(flagshipId));

		if (fleet == null) return;

		int areaId = fleet.ExpeditionDestination;

		foreach (ITrackerViewModel tracker in trackers.Where(t => t.State is 2))
		{
			tracker.Increment(fleet, areaId);
		}
	}

	public void EquipmentDiscarded(IEnumerable<ITrackerViewModel> trackers, IEnumerable<IEquipmentData> discardedEquipment)
	{
		foreach (ITrackerViewModel tracker in trackers.Where(t => t.State is 2))
		{
			tracker.Increment(discardedEquipment.Select(e => e.EquipmentId));
			tracker.Increment(discardedEquipment.Select(e => e.MasterEquipment.CategoryType));
			tracker.Increment(discardedEquipment.Select(e => e.MasterEquipment.CardType));
			tracker.Increment(discardedEquipment.Select(e => e.MasterEquipment.IconTypeTyped));
		}
	}
}
