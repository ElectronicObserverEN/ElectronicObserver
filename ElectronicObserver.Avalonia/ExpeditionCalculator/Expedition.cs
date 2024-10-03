namespace ElectronicObserver.Avalonia.ExpeditionCalculator;

public class Expedition
{
	public required int Id { get; init; }
	public required string DisplayId { get; init; }
	public required GreatSuccessType GreatSuccessType { get; init; }

	public required TimeSpan Duration { get; init; }

	public int Fuel { get; init; }
	public int Ammo { get; init; }
	public int Steel { get; init; }
	public int Bauxite { get; init; }

	public ExpeditionReward? Item1 { get; init; }
	public ExpeditionReward? Item2 { get; init; }
}
