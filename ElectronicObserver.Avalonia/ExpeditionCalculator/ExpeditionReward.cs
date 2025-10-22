using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Avalonia.ExpeditionCalculator;

public class ExpeditionReward
{
	public required UseItemId Type { get; init; }
	public required int Amount { get; init; }
}
