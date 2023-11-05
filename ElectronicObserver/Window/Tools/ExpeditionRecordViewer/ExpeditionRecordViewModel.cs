using System;
using System.Linq;
using ElectronicObserver.Database.Expedition;
using ElectronicObserver.KancolleApi.Types.ApiReqMission.Result;
using ElectronicObserver.Window.Tools.SortieRecordViewer;
using ElectronicObserverTypes;
using System.Text.Json;
using System.Collections.Generic;
using ElectronicObserver.Data;

namespace ElectronicObserver.Window.Tools.ExpeditionRecordViewer;

public class ExpeditionRecordViewModel
{
	public ExpeditionRecord Model { get; }
	public int? Id => Model?.Id;
	public DateTime ExpeditionStart { get; }

	public int? MapAreaID { get; }
	public string? DisplayID { get; }

	public IFleetData? Fleet { get; }

	public object? MaterialList { get; }
	public int MaterialFuel { get; }
	public int MaterialAmmo { get; }
	public int MaterialSteel { get; }
	public int MaterialBaux { get; }

	public List<int>? ItemList { get; }
	public int? ItemOneID { get; }
	public string? ItemOneName { get; }
	public int? ItemOneCount { get; }
	public string? ItemOneString { get; }
	public int? ItemTwoID { get; }
	public string? ItemTwoName { get; }
	public int? ItemTwoCount { get; }
	public string? ItemTwoString { get; }

	public string? ClearResult { get; }

	public ExpeditionRecordViewModel(ExpeditionRecord record, ApiReqMissionResultResponse response, DateTime expeditionStart)
	{
		Model = record;
		ExpeditionStart = expeditionStart.ToLocalTime();
		MapAreaID = KCDatabase.Instance.Mission.Values.FirstOrDefault(s => s.MissionID == record.Expedition)?.MapAreaID;
		DisplayID = KCDatabase.Instance.Mission.Values.FirstOrDefault(s => s.MissionID == record.Expedition)?.DisplayID;
		Fleet = record.Fleet.MakeFleet(0);
		ItemList = response.ApiUseitemFlag;
		MaterialList = response.ApiGetMaterial.ToString() != "-1" ? JsonSerializer.Deserialize<List<int>>(response.ApiGetMaterial.ToString()!) : new List<int>(new int[4]);
		ItemOneID = ItemList[0];
		ItemOneName = ParseUseItemControl(ItemOneID, response.ApiGetItem1?.ApiUseitemId).ToString();
		ItemOneCount = response.ApiGetItem1?.ApiUseitemCount;
		ItemOneString = ItemList.Count > 0 && ItemOneCount > 0 ? ParseUseItem(ItemOneID, response.ApiGetItem1?.ApiUseitemId) : "";
		ItemTwoID = ItemList[1];
		ItemTwoName = ParseUseItemControl(ItemTwoID, response.ApiGetItem2?.ApiUseitemId).ToString();
		ItemTwoCount = response.ApiGetItem2?.ApiUseitemCount;
		ItemTwoString = ItemTwoCount > 0 && ItemList.Count > 0 ? ParseUseItem(ItemTwoID, response.ApiGetItem2?.ApiUseitemId) : "";
		ClearResult = Constants.GetExpeditionResult(response.ApiClearResult);
	}

	private string ParseUseItem(int? kind, int? key)
	{
		return kind switch
		{
			1 => ConstantsRes.Bucket,
			2 => ConstantsRes.Flamethrower,
			3 => ConstantsRes.DevMat,
			4 => KCDatabase.Instance.MasterUseItems[(int)key!].NameTranslated,
			5 => ConstantsRes.FurnitureCoin,
			_ => "",
		};
	}

	private UseItemId ParseUseItemControl(int? kind, int? key)
	{
		if (key is not int id) return UseItemId.Unknown;

		return kind switch
		{
			1 => UseItemId.InstantRepair,
			2 => UseItemId.InstantConstruction,
			3 => UseItemId.DevelopmentMaterial,
			4 => (UseItemId)id,
			5 => UseItemId.FurnitureCoin,
			_ => UseItemId.Unknown,
		};
	}
}
