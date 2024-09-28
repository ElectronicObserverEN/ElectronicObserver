using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Avalonia.ExpeditionCalculator;

public class ExpeditionCalculatorViewModel : ObservableObject
{
	public FleetInfoViewModel FleetInfo { get; } = new();
	public ExpeditionScoreWeights ExpeditionScoreWeights { get; } = new();

	public List<ExpeditionViewModel> Expeditions { get; }

	public ExpeditionCalculatorViewModel()
	{
		Expeditions = ExpeditionCalculatorData.Expeditions
			.Select(e => new ExpeditionViewModel(e, FleetInfo, ExpeditionScoreWeights))
			.ToList();
	}
}
