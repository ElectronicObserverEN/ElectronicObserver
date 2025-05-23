using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Window.Wpf.Fleet.ViewModels;

namespace ElectronicObserver.Utility.Data;

public static class NightEquipment
{
	public static List<TotalRate> TotalRate(this IEnumerable<IActivatableEquipment> equipment)
	{
		List<IActivatableEquipment> allEquipment = equipment.Append(new ActivatableEquipmentNoneModel()).ToList();

		List<double> rates = allEquipment
			.Select(r => r.ActivationRate)
			.ToList()
			.TotalRates();

		return rates
			.Zip(allEquipment, (r, e) => new TotalRate(r, e))
			.ToList();
	}

	public static List<NightReconModel> NightRecons(this IFleetData fleet) => fleet.MembersWithoutEscaped!
		.Where(s => s is not null)
		.SelectMany(NightRecons!)
		.OrderByDescending(r => r.Equipment.MasterEquipment.Accuracy)
		.ToList();

	private static List<NightReconModel> NightRecons(this IShipData ship) => ship.AllSlotInstance
		.Zip(ship.Aircraft, (e, s) => (Equipment: e, SlotSize: s))
		.Where(s => s.SlotSize > 0)
		.Where(s => s.Equipment?.MasterEquipment.IconTypeTyped is EquipmentIconType.NightSeaplane)
		.Select(e => new NightReconModel
		{
			Ship = ship,
			Equipment = e.Equipment!,
		})
		.OrderByDescending(r => r.Equipment.MasterEquipment.Accuracy)
		.ToList();

	public static List<FlareModel> Flares(this IFleetData fleet) => fleet.MembersWithoutEscaped!
		.Where(s => s is not null)
		.SelectMany(Flares!)
		.ToList();

	private static List<FlareModel> Flares(this IShipData ship) => ship.AllSlotInstance
		.Where(e => e?.MasterEquipment.IconTypeTyped is EquipmentIconType.StarShell)
		.Select(e => new FlareModel
		{
			Ship = ship,
			Equipment = e!,
		}).ToList();
}
