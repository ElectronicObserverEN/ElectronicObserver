using System.Collections.Generic;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Core.Services.Data;

public interface ITransportGaugeService
{
	public string GetCurrentEventLandingOperationToolTip(List<IFleetData> fleets);
	public string GetAllEventLandingOperationToolTip(List<IFleetData> fleets);
	public string GetEventLandingOperationToolTip(List<IFleetData> fleets, List<TpGauge> gauges, bool displayEventName);

	public List<TpGauge> GetEventLandingGauges(bool includeNone);
}
