using ElectronicObserverTypes;

namespace ElectronicObserver.Utility.Data;

public record FlareModel : IActivatableEquipment
{
	// todo: both required
	public IShipData Ship { get; init; } = null!;
	public IEquipmentData Equipment { get; init; } = null!;

	public double ActivationRate => 0.7;

	public override string ToString() => $"{Equipment.Name} ({Ship.Name})";
}
