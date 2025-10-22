using System.Collections.Generic;
using ElectronicObserver.Core.Types.Serialization.EquipmentUpgrade;
using ElectronicObserver.KancolleApi.Types.ApiReqKousyou.RemodelSlotlistDetail;

namespace ElectronicObserver.Utility.ElectronicObserverApi.DataIssueLogs;

public static class ApiReqKousyouRemodelSlotlistDetailResponseExtensions
{
	public static List<EquipmentUpgradeImprovementCostItemDetail> GetRequiredEquipments(this ApiReqKousyouRemodelSlotlistDetailResponse response)
	{
		List<EquipmentUpgradeImprovementCostItemDetail> items = [];

		if (response.ApiReqSlotId > 0)
		{
			items.Add(new()
			{
				Id = (int)response.ApiReqSlotId,
				Count = response.ApiReqSlotNum ?? 1,
			});
		}

		if (response.ApiReqSlotId2 > 0)
		{
			items.Add(new()
			{
				Id = (int)response.ApiReqSlotId2,
				Count = response.ApiReqSlotNum2 ?? 1,
			});
		}

		return items;
	}
	
	public static List<EquipmentUpgradeImprovementCostItemDetail> GetRequiredItems(this ApiReqKousyouRemodelSlotlistDetailResponse response)
	{
		List<EquipmentUpgradeImprovementCostItemDetail> items = [];

		if (response.ApiReqUseItemId > 0)
		{
			items.Add(new()
			{
				Id = (int)response.ApiReqUseItemId,
				Count = response.ApiReqUseItemNum ?? 1,
			});
		}

		if (response.ApiReqUseItemId2 > 0)
		{
			items.Add(new()
			{
				Id = (int)response.ApiReqUseItemId2,
				Count = response.ApiReqUseItemNum2 ?? 1,
			});
		}

		return items;
	}
}
