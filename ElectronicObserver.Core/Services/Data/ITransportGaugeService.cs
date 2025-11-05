using System.Collections.Generic;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Extensions;

namespace ElectronicObserver.Core.Services.Data;

public interface ITransportGaugeService
{
	public string GetCurrentEventLandingOperationToolTip(List<IFleetData> fleets);
	public string GetAllEventLandingOperationToolTip(List<IFleetData> fleets);
	public string GetEventLandingOperationToolTip(int areaId, List<IFleetData> fleets);

	public List<TpGauge> GetEventLandingGauges(bool includeNone);

	public static string GetEventName(TpGauge gauge) => gauge.GetGaugeAreaId() switch
	{
		60 => Properties.EventConstants.Spring2025,
		61 => Properties.EventConstants.Fall2025,
		_ => "",
	};
}
