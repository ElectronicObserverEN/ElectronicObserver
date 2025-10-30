using System.Collections.Generic;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Core.Services.Data;

public interface ITransportGaugeService
{
	public string GetCurrentEventLandingOperationToolTip(List<IFleetData> fleets);
}
