using ElectronicObserver.Data;

namespace ElectronicObserver.Observer.kcsapi.api_req_combined_battle;

public class battle : APIBase
{

	public override void OnResponseReceived(dynamic data)
	{

		KCDatabase.Instance.Battle.LoadFromResponse(APIName, data);
		KCDatabase.Instance.TsunDbSubmission.LoadFromResponse(APIName, data);

		base.OnResponseReceived((object)data);
	}

	public override string APIName => "api_req_combined_battle/battle";
}
