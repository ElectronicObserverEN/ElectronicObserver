using System.Collections.Generic;
using ElectronicObserver.Data;

namespace ElectronicObserver.Observer.kcsapi.api_req_mission;

public class start : APIBase
{

	//private int FleetID;


	public override void OnRequestReceived(Dictionary<string, string> data)
	{

		/*	//checkme: どちらにせよあとで deck が呼ばれるので不要？
		FleetID = int.Parse( data["api_deck_id"] );
		KCDatabase.Instance.Fleet.Fleets[FleetID].LoadFromRequest( APIName, data );
		*/

		int deckID = int.Parse(data["api_deck_id"]);
		int destination = int.Parse(data["api_mission_id"]);

		Utility.Logger.Add(2, string.Format(NotifierRes.HasBeenSentToExpedition, deckID, KCDatabase.Instance.Fleet[deckID].Name, KCDatabase.Instance.Mission[destination].DisplayID, KCDatabase.Instance.Mission[destination].NameEN));

		base.OnRequestReceived(data);
	}

	public override void OnResponseReceived(dynamic data)
	{

		/*
		KCDatabase.Instance.Fleet.Fleets[FleetID].LoadFromResponse( APIName, data );
		*/

		base.OnResponseReceived((object)data);

	}


	public override bool IsRequestSupported => true;
	public override bool IsResponseSupported => true;

	public override string APIName => "api_req_mission/start";
}
