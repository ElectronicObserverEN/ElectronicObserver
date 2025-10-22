using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Avalonia.ExpeditionCalculator;

public partial class ExpeditionCalculatorViewModel : ObservableObject
{
	[ObservableProperty] public partial bool? ShowMonthlyExpeditions { get; set; } = false;
	public FleetInfoViewModel FleetInfo { get; } = new();
	public ExpeditionScoreWeights ExpeditionScoreWeights { get; } = new();

	public ObservableCollection<ExpeditionViewModel> Expeditions { get; } = [];

	public ExpeditionCalculatorViewModel()
	{
		UpdateExpeditions();
	}

	// ReSharper disable once UnusedParameterInPartialMethod
	partial void OnShowMonthlyExpeditionsChanged(bool? value)
	{
		UpdateExpeditions();
	}

	private void UpdateExpeditions()
	{
		Expeditions.Clear();

		List<ExpeditionViewModel> expeditions = ExpeditionCalculatorData.Expeditions
			.Where(e => ShowMonthlyExpeditions is null || e.IsMonthly == ShowMonthlyExpeditions)
			.Select(e => new ExpeditionViewModel(e, FleetInfo, ExpeditionScoreWeights))
			.ToList();

		foreach (ExpeditionViewModel expedition in expeditions)
		{
			Expeditions.Add(expedition);
		}
	}
}
