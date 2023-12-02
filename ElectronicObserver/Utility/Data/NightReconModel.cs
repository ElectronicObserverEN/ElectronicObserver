using System;
using ElectronicObserverTypes;

namespace ElectronicObserver.Utility.Data;

public record NightReconModel : IActivatableEquipment
{
	public required IShipData Ship { get; init; }
	public required IEquipmentData Equipment { get; init; }

	public double ActivationRate => 4 * Math.Floor(Math.Sqrt(Equipment.MasterEquipment.LOS) * Math.Sqrt(Ship.Level)) / 100;

	public override string ToString() => $"{Equipment.Name} ({Ship.Name})";
}
