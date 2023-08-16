using System;
using ElectronicObserverTypes;

namespace ElectronicObserver.Utility.Data;

public record NightReconModel : IActivatableEquipment
{
	// todo: both required
	public IShipData Ship { get; init; } = null!;
	public IEquipmentData Equipment { get; init; } = null!;

	public double ActivationRate => 4 * Math.Floor(Math.Sqrt(Equipment.MasterEquipment.LOS) * Math.Sqrt(Ship.Level)) / 100;

	public override string ToString() => $"{Equipment.Name} ({Ship.Name})";
}
