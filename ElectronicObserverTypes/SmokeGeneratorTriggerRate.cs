namespace ElectronicObserverTypes;

public record SmokeGeneratorTriggerRate : IActivatableEquipment
{
	public required double ActivationRatePercentage { get; init; }
	public required int SmokeGeneratorCount { get; init; }

	public double ActivationRate => ActivationRatePercentage / 100;

	public bool SmokeGenerator1Active => SmokeGeneratorCount >= 1;
	public bool SmokeGenerator2Active => SmokeGeneratorCount >= 2;
	public bool SmokeGenerator3Active => SmokeGeneratorCount >= 3;
}
