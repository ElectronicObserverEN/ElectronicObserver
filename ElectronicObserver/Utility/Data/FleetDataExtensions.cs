using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Extensions;
using ElectronicObserver.Window.Wpf.Fleet.ViewModels;

namespace ElectronicObserver.Utility.Data;

public static class FleetDataExtensions
{
	public static bool HasStarShell(this IFleetData fleet) => fleet.MembersWithoutEscaped
		.Any(s => s?.HasStarShell() ?? false);

	public static bool HasSearchlight(this IFleetData fleet) => fleet.MembersWithoutEscaped
		.Any(s => s?.HasSearchlight() ?? false);

	public static List<TotalRate> TotalRate(this IEnumerable<SmokeGeneratorTriggerRate> generatorRates)
	{
		List<TotalRate> allRates = generatorRates
			.Select(r => new TotalRate(r.ActivationRate, r))
			.ToList();

		double totalRate = allRates.Sum(r => r.Rate);

		if (totalRate < 1)
		{
			allRates.Add(new TotalRate(1 - totalRate, new ActivatableEquipmentNoneModel()));
		}

		return allRates;
	}
}
