using System.Collections.Generic;
using ElectronicObserver.Data;

namespace ElectronicObserver.Observer.kcsapi.api_port;

public class airCorpsCondRecoveryWithTimer : APIBase
{
	private int AirCorpsId { get; set; }

	public override bool IsRequestSupported => true;

	public override void OnRequestReceived(Dictionary<string, string> data)
	{
		AirCorpsId = BaseAirCorpsData.GetID(data);

		base.OnRequestReceived(data);
	}

	public override void OnResponseReceived(dynamic data)
	{
		if (data is null) return;

		if (KCDatabase.Instance.BaseAirCorps.TryGetValue(AirCorpsId, out BaseAirCorpsData corps))
		{
			corps.LoadFromResponse(APIName, data);
		}

		base.OnResponseReceived((object)data);
	}

	public override string APIName => "api_port/airCorpsCondRecoveryWithTimer";
}
