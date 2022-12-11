using ElectronicObserver.Data;

namespace ElectronicObserver.Observer.kcsapi.api_req_combined_battle;

public class ld_shooting : APIBase
{
	public override void OnResponseReceived(dynamic data)
	{

		KCDatabase.Instance.Battle.LoadFromResponse(APIName, data);
		KCDatabase.Instance.TsunDbSubmission.LoadFromResponse(APIName, data);

		base.OnResponseReceived((object)data);
	}

	public override string APIName => "api_req_combined_battle/ld_shooting";
}
