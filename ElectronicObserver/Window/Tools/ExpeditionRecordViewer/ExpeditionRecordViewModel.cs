using System;
using System.Linq;
using ElectronicObserver.Database.Expedition;
using ElectronicObserver.Database.KancolleApi;
using ElectronicObserver.KancolleApi.Types;
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
	public int Id => Model.Id;
	public DateTime ExpeditionStart { get; }
	public IFleetData? Fleet { get; }
	public List<int> ItemList { get; }
	public object MaterialList { get; }
	public int MaterialFuel { get; }
	public int MaterialAmmo { get; }
	public int MaterialSteel { get; }
	public int MaterialBaux { get; }
	public int? ItemOneID { get; }
	public string ItemOneName { get; }
	public int? ItemOneCount { get; }
	public int? ItemTwoID { get; }
	public int? MapAreaID { get; }
	public string? DisplayID { get; }
	public string ItemTwoName { get; }
	public int? ItemTwoCount { get; }
	public string ClearResult { get; }
	private ApiReqMissionResultResponse? response => ParseResponse(Model);
	public string ItemOneString => ItemList.Count > 0 && ItemOneCount > 0 ? ParseUseItem(ItemOneID, response!.ApiGetItem1?.ApiUseitemId) : "";
	public string ItemTwoString => ItemTwoCount > 0 && ItemList.Count > 0 ? ParseUseItem(ItemTwoID, response!.ApiGetItem2?.ApiUseitemId) : "";
	public ExpeditionRecordViewModel(ExpeditionRecord expedition, DateTime expeditionStart)
	{

		Model = expedition;
		ExpeditionStart = expeditionStart.ToLocalTime();
		MapAreaID = KCDatabase.Instance.Mission.Values.FirstOrDefault(s => s.MissionID == expedition.Expedition)?.MapAreaID;
		DisplayID = KCDatabase.Instance.Mission.Values.FirstOrDefault(s => s.MissionID == expedition.Expedition)?.DisplayID;
		Fleet = expedition.Fleet.MakeFleet();
		ItemList = response!.ApiUseitemFlag;
		MaterialList = response!.ApiGetMaterial;
		if(MaterialList.ToString() != "-1")
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
		ItemOneName = ParseUseItemControl(ItemOneID, response!.ApiGetItem1?.ApiUseitemId)!;
		ItemOneCount = response!.ApiGetItem1?.ApiUseitemCount;
		ItemTwoID = ItemList[1];
		ItemTwoName = ParseUseItemControl(ItemTwoID, response!.ApiGetItem2?.ApiUseitemId)!;
		ItemTwoCount = response.ApiGetItem2?.ApiUseitemCount;
		ClearResult = Constants.GetExpeditionResult(response!.ApiClearResult);
	}
	public static ApiReqMissionResultResponse? ParseResponse(ExpeditionRecord expedition)
	{
		if(expedition.ApiFiles.Count > 0)
		{
			try
			{
				ApiFile? result = expedition.ApiFiles.Find(f => f.ApiFileType == ApiFileType.Response && f.Name == "api_req_mission/result");
				if (result is not null)
				{
					ApiReqMissionResultResponse? apiReqMissionResultResponse = JsonSerializer.Deserialize<ApiResponse<ApiReqMissionResultResponse>>(result.Content)?.ApiData;
					if (apiReqMissionResultResponse is not null)
					{
						return apiReqMissionResultResponse;
					}
				}
			}catch
			{
			}
		}
		return default;
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
	public string? ParseUseItemControl(int? kind, int? key)
	{
		return kind switch
		{
			1 => "InstantRepair",
			2 => "InstantConstruction",
			3 => "DevelopmentMaterial",
			4 => Enum.GetName(typeof(UseItemId), key!),
			5 => "FurnitureCoin",
			_ => "",
		};
	}
}
