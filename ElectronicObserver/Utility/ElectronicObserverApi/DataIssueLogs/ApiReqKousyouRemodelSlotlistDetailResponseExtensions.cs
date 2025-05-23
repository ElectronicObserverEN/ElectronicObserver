using System.Collections.Generic;
using ElectronicObserver.Core.Types.Serialization.EquipmentUpgrade;
using ElectronicObserver.KancolleApi.Types.ApiReqKousyou.RemodelSlotlistDetail;

namespace ElectronicObserver.Utility.ElectronicObserverApi.DataIssueLogs;

public static class ApiReqKousyouRemodelSlotlistDetailResponseExtensions
{
	public static List<EquipmentUpgradeImprovementCostItemDetail> GetRequiredEquipments(this ApiReqKousyouRemodelSlotlistDetailResponse response)
	{
		if (response.ApiReqSlotId > 0)
		{
			return
			[
				new()
				{
					Id = (int)response.ApiReqSlotId,
					Count = response.ApiReqSlotNum ?? 1,
				},
			];
		}

		return [];
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
