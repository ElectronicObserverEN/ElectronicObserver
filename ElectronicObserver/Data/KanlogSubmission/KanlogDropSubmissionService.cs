using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ElectronicObserver.Data.Battle;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Next;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Start;
using Newtonsoft.Json.Linq;

namespace ElectronicObserver.Data.KanlogSubmission;

public class KanlogDropSubmissionService(
	KCDatabase kcDatabase,
	string version,
	KanlogHttpClient client,
	Action<Exception> logError)
{
	private KCDatabase KcDatabase { get; } = kcDatabase;
	private string Version { get; } = version;
	private KanlogHttpClient Client { get; } = client;
	private Action<Exception> LogError { get; } = logError;

	private int? EventDifficulty { get; set; }
	private int? World { get; set; }
	private int? Map { get; set; }
	private int? Cell { get; set; }

	public void ApiReqMap_Start_ResponseReceived(string apiname, dynamic data)
	{
		ApiReqMapStartResponse response = JsonSerializer.Deserialize<ApiReqMapStartResponse>(data.ToString());

		World = response.ApiMapareaId;
		Map = response.ApiMapinfoNo;
		Cell = response.ApiNo;
		EventDifficulty = KcDatabase.Battle.Compass.MapInfo.EventDifficulty;
	}

	public void ApiReqMap_NextOnResponseReceived(string apiname, dynamic data)
	{
		string json = data.ToString();
		ApiReqMapNextResponse response = JsonSerializer.Deserialize<ApiReqMapNextResponse>(json)!;

		Cell = response.ApiNo;
	}

	public void BattleResult_ResponseReceived(string apiname, dynamic data)
	{
		try
		{
			if (World is not int world) return;
			if (Map is not int map) return;
			if (Cell is not int cell) return;
			if (EventDifficulty is not int eventDifficulty) return;

			BattleManager battle = KcDatabase.Battle;
			if (battle == null) return;

			int shipId = battle.Result.DroppedShipID;

			DropData drop = new()
			{
				V = 1,
				Mid = world * 10 + map,
				Cno = cell,
				Efm = (DropDataEfm)battle.FirstBattle.Searching.FormationEnemy,
				Mlv = (DropDataMlv)eventDifficulty,
				Sid = shipId,
				Ocnt = shipId switch
				{
					> 0 => KcDatabase.Ships.Values.Count(ship => ship.ShipID == shipId),
					_ => 0,
				},
				Rank = MapRank(battle.Result.Rank),
				Es1 = battle.FirstBattle.Initial.EnemyMembers,
				Es2 = battle.FirstBattle.Initial.EnemyMembersEscort,
				Dt = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
				Srv = KCDatabase.Instance.ServerManager.CurrentServer?.Num.ToString() ?? "0",
				Its = ExtractItems(battle)
			};

			Client.DropAsync(Uri.EscapeDataString(Version), [drop]);
		}
		catch (Exception e)
		{
			LogError(e);
		}
	}

	private static DropDataRank MapRank(string rank) => rank switch
	{
		"S" => DropDataRank.S,
		"A" => DropDataRank.A,
		"B" => DropDataRank.B,
		"C" => DropDataRank.C,
		"D" => DropDataRank.D,
		"E" => DropDataRank.E,
		_ => DropDataRank.E,
	};

	private List<DropItem> ExtractItems(BattleManager battle)
	{
		List<DropItem> items = [];
		int itemId = battle.Result.DroppedItemID;

		if (itemId is not -1)
		{
			items.Add(new DropItem
			{
				Id = itemId,
				Ocnt = itemId switch
				{
					> 0 => KCDatabase.Instance.UseItems.Values.Count(item => item.ItemID == itemId),
					_ => 0,
				},
				Cnt = 1
			});
		}

		return items;
	}
}
