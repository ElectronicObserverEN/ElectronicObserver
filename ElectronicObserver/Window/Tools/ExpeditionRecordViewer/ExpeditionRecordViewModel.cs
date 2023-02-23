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
	public ExpeditionRecord? Model { get; }
	public int? Id => Model?.Id;
	public DateTime ExpeditionStart { get; }
	public IFleetData? Fleet { get; }
	public List<int>? ItemList { get; }
	public object? MaterialList { get; }
	public int MaterialFuel { get; }
	public int MaterialAmmo { get; }
	public int MaterialSteel { get; }
	public int MaterialBaux { get; }
	public int? ItemOneID { get; }
	public string? ItemOneName { get; }
	public int? ItemOneCount { get; }
	public int? ItemTwoID { get; }
	public int? MapAreaID { get; }
	public string? DisplayID { get; }
	public string? ItemTwoName { get; }
	public int? ItemTwoCount { get; }
	public string? ClearResult { get; }
	public string? ItemOneString { get; }
	public string? ItemTwoString { get; }
	public ExpeditionRecordViewModel(ExpeditionRecord record, ApiReqMissionResultResponse response, DateTime expeditionStart)
	{
		if (response is null) return;
		Model = record;
		ExpeditionStart = expeditionStart.ToLocalTime();
		MapAreaID = KCDatabase.Instance.Mission.Values.FirstOrDefault(s => s.MissionID == record.Expedition)?.MapAreaID;
		DisplayID = KCDatabase.Instance.Mission.Values.FirstOrDefault(s => s.MissionID == record.Expedition)?.DisplayID;
		Fleet = record.Fleet.MakeFleet();
		ItemList = response!.ApiUseitemFlag;
		MaterialList = response!.ApiGetMaterial;
		if (MaterialList.ToString() != "-1")
		{
			List<int>? list = JsonSerializer.Deserialize<List<int>>(MaterialList!.ToString()!);
			MaterialFuel = list![0];
			MaterialAmmo = list![1];
			MaterialSteel = list![2];
			MaterialBaux = list![3];
		}
		else
		{
			MaterialFuel = 0;
			MaterialAmmo = 0;
			MaterialSteel = 0;
			MaterialBaux = 0;
		}
		ItemOneID = ItemList[0];
		ItemOneName = ParseUseItemControl(ItemOneID, response!.ApiGetItem1?.ApiUseitemId)!.ToString();
		ItemOneCount = response!.ApiGetItem1?.ApiUseitemCount;
		ItemTwoID = ItemList[1];
		ItemTwoName = ParseUseItemControl(ItemTwoID, response!.ApiGetItem2?.ApiUseitemId)!.ToString();
		ItemTwoCount = response.ApiGetItem2?.ApiUseitemCount;
		ClearResult = Constants.GetExpeditionResult(response!.ApiClearResult);
		ItemOneString = ItemList.Count > 0 && ItemOneCount > 0 ? ParseUseItem(ItemOneID, response!.ApiGetItem1?.ApiUseitemId) : "";
		ItemTwoString = ItemTwoCount > 0 && ItemList.Count > 0 ? ParseUseItem(ItemTwoID, response!.ApiGetItem2?.ApiUseitemId) : "";
	}

	public string ParseUseItem(int? kind, int? key)
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

	public UseItemId? ParseUseItemControl(int? kind, int? key)
	{
		return kind switch
		{
			1 => UseItemId.InstantRepair,
			2 => UseItemId.InstantConstruction,
			3 => UseItemId.DevelopmentMaterial,
			4 => (UseItemId)Enum.Parse(typeof(UseItemId), key?.ToString() ?? ""),
			5 => UseItemId.FurnitureCoin,
			_ => UseItemId.Unknown,
		};
	}
}
