using System.Collections.Generic;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Data;
using ElectronicObserver.Data.DiscordRPC;
using ElectronicObserver.Database;
using ElectronicObserver.Database.MapData;

namespace ElectronicObserver.Observer.kcsapi.api_req_map;

public class start : APIBase
{

	public override void OnResponseReceived(dynamic data)
	{


		KCDatabase db = KCDatabase.Instance;

		db.Battle.LoadFromResponse(APIName, data);

		if (Utility.Configuration.Config.Control.EnableDiscordRPC)
		{
			DiscordRpcModel dataForWS = DiscordRpcManager.Instance.GetRPCData();
			dataForWS.TopDisplayText = string.Format(NotifierRes.SortieingTo, db.Battle.Compass.MapAreaID, db.Battle.Compass.MapInfoID, db.Battle.Compass.CellDisplay, db.Battle.Compass.MapInfo.NameEN);
		}

		KCDatabase.Instance.TsunDbSubmission.LoadFromResponse(APIName, data);

		int world = (int)data.api_maparea_id;
		int map = (int)data.api_mapinfo_no;
		object[] cells = (object[])data.api_cell_data;

		using ElectronicObserverContext context = new();

		foreach (dynamic cell in cells)
		{
			CellModel? node = context.Cells.Find((int)cell.api_id);

			if (node is not null)
			{
				// update any other values if we decide to add them
				continue;
			}

			context.Cells.Add(new()
			{
				Id = (int)cell.api_id,
				WorldId = world,
				MapId = map,
				CellId = (int)cell.api_no,
				CellType = (CellType)cell.api_color_no,
			});
		}

		context.SaveChanges();

		base.OnResponseReceived((object)data);



		// 表示順の関係上、UIの更新をしてからデータを更新する
		if (KCDatabase.Instance.Battle.Compass.EventID == 3)
		{
			next.EmulateWhirlpool();
		}

	}


	public override bool IsRequestSupported => true;

	public override void OnRequestReceived(Dictionary<string, string> data)
	{

		KCDatabase.Instance.Fleet.LoadFromRequest(APIName, data);

		int deckID = int.Parse(data["api_deck_id"]);
		int maparea = int.Parse(data["api_maparea_id"]);
		int mapinfo = int.Parse(data["api_mapinfo_no"]);

		Utility.Logger.Add(2, string.Format(NotifierRes.HasSortiedTo, deckID, KCDatabase.Instance.Fleet[deckID].Name, maparea, mapinfo, KCDatabase.Instance.MapInfo[maparea * 10 + mapinfo].NameEN));

		base.OnRequestReceived(data);
	}


	public override string APIName => "api_req_map/start";
}
