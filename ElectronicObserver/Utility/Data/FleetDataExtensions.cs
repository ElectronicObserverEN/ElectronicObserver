using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Window.Wpf.Fleet.ViewModels;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;

namespace ElectronicObserver.Utility.Data;

public static class FleetDataExtensions
{
	public static bool HasStarShell(this IFleetData fleet) => fleet.MembersWithoutEscaped
		.Any(s => s?.HasStarShell() ?? false);

	public static bool HasSearchlight(this IFleetData fleet) => fleet.MembersWithoutEscaped
		.Any(s => s?.HasSearchlight() ?? false);

	public static List<TotalRate> TotalRate(this IEnumerable<SmokeGeneratorTriggerRate> generatorRates)
	{
		List<IActivatableEquipment> allRates =
		[
			..generatorRates,
		];

		if (allRates.Sum(r => r.ActivationRate) < 1)
		{
			allRates.Add(new ActivatableEquipmentNoneModel());
		}

		return allRates
			.Select(r => new TotalRate(r.ActivationRate, r))
			.ToList();
	}
}
