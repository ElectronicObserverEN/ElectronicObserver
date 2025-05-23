using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Utility.Data;

public record FlareModel : IActivatableEquipment
{
	public required IShipData Ship { get; init; }
	public required IEquipmentData Equipment { get; init; }

	public double ActivationRate => 0.7;

	public override string ToString() => $"{Equipment.Name} ({Ship.Name})";
}
