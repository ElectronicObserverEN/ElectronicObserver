using ElectronicObserver.Data;

namespace ElectronicObserver.Observer.kcsapi.api_req_combined_battle;

public class each_battle_water : APIBase
{

	public override void OnResponseReceived(dynamic data)
	{

		KCDatabase.Instance.Battle.LoadFromResponse(APIName, data);
		KCDatabase.Instance.TsunDbSubmission.LoadFromResponse(APIName, data);

		base.OnResponseReceived((object)data);
	}

	public override string APIName => "api_req_combined_battle/each_battle_water";
}
