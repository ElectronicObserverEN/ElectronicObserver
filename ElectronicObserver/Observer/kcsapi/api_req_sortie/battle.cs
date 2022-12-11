using System.Collections.Generic;
using ElectronicObserver.Data;

namespace ElectronicObserver.Observer.kcsapi.api_req_sortie;

public class battle : APIBase
{

	public override void OnResponseReceived(dynamic data)
	{

		KCDatabase.Instance.Battle.LoadFromResponse(APIName, data);
		KCDatabase.Instance.TsunDbSubmission.LoadFromResponse(APIName, data);

		base.OnResponseReceived((object)data);
	}

	public override void OnRequestReceived(Dictionary<string, string> data)
	{
		KCDatabase.Instance.Battle.LoadFromRequest(APIName, data);

		base.OnRequestReceived(data);
	}

	public override bool IsRequestSupported => true;

	public override string APIName => "api_req_sortie/battle";
}
