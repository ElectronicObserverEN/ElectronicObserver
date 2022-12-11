using ElectronicObserver.Data;

namespace ElectronicObserver.Observer.kcsapi.api_req_sortie;

public class night_to_day : APIBase
{

	public override void OnResponseReceived(dynamic data)
	{

		KCDatabase.Instance.Battle.LoadFromResponse(APIName, data);
		KCDatabase.Instance.TsunDbSubmission.LoadFromResponse(APIName, data);

		base.OnResponseReceived((object)data);
	}


	public override string APIName => "api_req_sortie/night_to_day";
}
