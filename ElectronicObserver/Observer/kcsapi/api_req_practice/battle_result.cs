using ElectronicObserver.Data;

namespace ElectronicObserver.Observer.kcsapi.api_req_practice;

public class battle_result : APIBase
{

	public override void OnResponseReceived(dynamic data)
	{

		KCDatabase.Instance.Battle.LoadFromResponse(APIName, data);

		base.OnResponseReceived((object)data);
	}


	public override string APIName => "api_req_practice/battle_result";
}
