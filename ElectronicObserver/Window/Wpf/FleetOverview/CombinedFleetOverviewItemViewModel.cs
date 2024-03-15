using System.Collections.Generic;
using ElectronicObserver.Window.Wpf.Fleet.ViewModels;

namespace ElectronicObserver.Window.Wpf.FleetOverview;

public class CombinedFleetOverviewItemViewModel : FleetOverviewItemViewModel
{
	public List<TotalRate> SmokeGeneratorRates { get; set; } = [];
}
