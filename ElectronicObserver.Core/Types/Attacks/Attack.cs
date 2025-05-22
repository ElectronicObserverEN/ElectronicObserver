namespace ElectronicObserver.Core.Types.Attacks;

public record Attack
{
	public double PowerModifier { get; init; }
	public double AccuracyModifier { get; init; }
}
