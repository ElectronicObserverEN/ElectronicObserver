using System;
using System.Linq;
using ElectronicObserver.Database.Expedition;
using ElectronicObserver.KancolleApi.Types.ApiReqMission.Result;
using ElectronicObserver.Window.Tools.SortieRecordViewer;
using ElectronicObserverTypes;
using System.Text.Json;
using System.Collections.Generic;
using ElectronicObserver.Data;
using ElectronicObserver.Database.KancolleApi;
using ElectronicObserver.KancolleApi.Types;

namespace ElectronicObserver.Window.Tools.ExpeditionRecordViewer;

public class ExpeditionRecordViewModel
{
	public ExpeditionRecord Model { get; }
	public DateTime ExpeditionStart { get; }

	public string? DisplayId { get; }
	public string? ClearResult { get; }

	public int Fuel { get; }
	public int Ammo { get; }
	public int Steel { get; }
	public int Bauxite { get; }

	public UseItemId ItemOneId { get; }
	public int? ItemOneCount { get; }
	public string? ItemOneName { get; }

	public UseItemId ItemTwoId { get; }
	public int? ItemTwoCount { get; }
	public string? ItemTwoName { get; }

	public IFleetData? Fleet { get; }

	public ExpeditionRecordViewModel(ExpeditionRecord record, DateTime expeditionStart)
	{
		Model = record;
		ExpeditionStart = expeditionStart.ToLocalTime();
		DisplayId = KCDatabase.Instance.Mission.Values.FirstOrDefault(s => s.MissionID == record.Expedition)?.DisplayID;
		Fleet = record.Fleet.MakeFleet(0);

		ApiReqMissionResultResponse? response = ParseExpeditionResult(record);

		if (response is null) return;

		List<int> materialList = response.ApiGetMaterial switch
		{
			JsonElement { ValueKind: JsonValueKind.Array } materials => materials
				.EnumerateArray()
				.Select(e => e.GetInt32())
				.ToList(),

			_ => new List<int>(Enumerable.Repeat(0, 4)),
		};

		Fuel = materialList[0];
		Ammo = materialList[1];
		Steel = materialList[2];
		Bauxite = materialList[3];

		ItemOneId = ParseUseItemId(response.ApiUseitemFlag[0], response.ApiGetItem1?.ApiUseitemId);
		ItemOneCount = response.ApiGetItem1?.ApiUseitemCount;
		ItemOneName = (response.ApiUseitemFlag.Count > 0 && ItemOneCount > 0) switch
		{
			true => UseItemName(ItemOneId),
			_ => null,
		};

		ItemTwoId = ParseUseItemId(response.ApiUseitemFlag[1], response.ApiGetItem2?.ApiUseitemId);
		ItemTwoCount = response.ApiGetItem2?.ApiUseitemCount;
		ItemTwoName = (ItemTwoCount > 0 && response.ApiUseitemFlag.Count > 0) switch
		{
			true => UseItemName(ItemTwoId),
			_ => null,
		};
		
		ClearResult = Constants.GetExpeditionResult(response.ApiClearResult);
	}

	private static ApiReqMissionResultResponse? ParseExpeditionResult(ExpeditionRecord record)
	{
		try
		{
			ApiFile? apiFile = record.ApiFiles
				.Where(f => f.ApiFileType == ApiFileType.Response)
				.FirstOrDefault(f => f.Name == "api_req_mission/result");

			return apiFile switch
			{
				null => null,
				_ => JsonSerializer.Deserialize<ApiResponse<ApiReqMissionResultResponse>>(apiFile.Content)?.ApiData,
			};
		}
		catch
		{
			return null;
		}
	}

	private static string UseItemName(UseItemId id) => id switch
	{
		UseItemId.Unknown => $"{ConstantsRes.Unknown}({id})",
		_ => KCDatabase.Instance.MasterUseItems[(int)id].NameTranslated,
	};

	private static UseItemId ParseUseItemId(int? kind, int? key)
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
