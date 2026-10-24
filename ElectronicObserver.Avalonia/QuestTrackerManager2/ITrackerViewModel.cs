using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Quests;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2;

public interface ITrackerViewModel
{
	int QuestId { get; }
	int State { get; }
	QuestResetType QuestResetType { get; }
	string ClearCondition { get; }
	string ConditionDisplay { get; }
	int TaskCount { get; }

	bool ConditionMet(IFleetData fleet);

	void Increment(IFleetData fleet, int compassMapAreaId, int compassMapInfoId, int nodeId);
	void Increment(IFleetData fleet, string resultRank, int compassMapAreaId, int compassMapInfoId, int nodeId);
	void Increment(IFleetData fleet, string resultRank, int compassMapAreaId, int compassMapInfoId);
	void Increment(IFleetData fleet, int compassMapAreaId, int compassMapInfoId);
	void Increment(IFleetData fleet, string winRank);
	void Increment(IFleetData fleet, int areaId);
	void Increment(IEnumerable<EquipmentId> ids);
	void Increment(IEnumerable<EquipmentTypes> categories);
	void Increment(IEnumerable<EquipmentCardType> cardTypes);
	void Increment(IEnumerable<EquipmentIconType> iconTypes);
	void ResetTaskProgress();
}
